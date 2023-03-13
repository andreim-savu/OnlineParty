using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Firestore;

public struct IPetRaceRoom
{
    public string _roomCode;
    public string _gameName;
    public List<IPetRacePlayer> _players;
    public string _state;
    public int _turnNo;

    public IPetRaceRoom(string roomCode)
    {
        _gameName = "PetRace";
        _roomCode = roomCode;
        _state = "Lobby";
        _players = new List<IPetRacePlayer>();
        _turnNo = 1;
    }
}

[System.Serializable]
public class IPetRacePlayer
{
    public string id;
    public string name;
    public IPetRacePet pet;
    public List<IPetRacePerk> inventoryPerks;
    public string perkSelectionPerks;
    public bool isReady;
    public int coins;
    public int freeRerolls;
    public int score;
    public bool host;
}


[System.Serializable]
public class IPetRacePet
{
    public string id;
    public string name;
    public List<IPetRacePerk> perks;
    public float movementSpeed; //speed
    public float dodge; //avoid obstacles
    public float regen; //stamina regen
    public float depletion; //stamina depletion
    public int modelNo;

    public IPetRacePet(string _id, string _name, int _movementSpeed, int _dodge, int _regen, int _depletion, int _modelNo)
    {
        id = _id;
        name = _name;
        movementSpeed = _movementSpeed;
        dodge = _dodge;
        regen = _regen;
        depletion = _depletion;
        modelNo = _modelNo;

        perks = new List<IPetRacePerk>();
    }
}

[System.Serializable]
public class IPetRacePerk
{
    public string id;
    public string name;
    public string description;
    public string rarity;
    public int level;

    public IPetRacePerk(string _id, string _name, string _description, string _rarity)
    {
        id = _id;
        name = _name;
        description = _description;
        rarity = _rarity;
        level = 1;
    }
}

public class PetRaceCanvas : GameCanvas
{
    public static PetRaceCanvas Instance;
    List<IPetRacePerk> perksList = new List<IPetRacePerk>();

    [SerializeField] PetRaceRaceCamera raceCamera;
    [SerializeField] PetRaceRacetrack _racetrack;
    [SerializeField] PetRacePet _petObj;

    [SerializeField] GameObject _progressBar;
    [SerializeField] GameObject _petIcon;
    List<RectTransform> _petIcons = new List<RectTransform>();

    public List<PetRacePet> _petObjects = new List<PetRacePet>();

    [SerializeField] Transform _lobbyPetsContainer;
    [SerializeField] LobbyPet _lobbyPet;

    [SerializeField] GameObject _leaderboard;
    [SerializeField] Transform _leaderboardPlayersContainer;
    [SerializeField] PetRaceLeaderboardPlayer _leaderboardPlayerContainer;

    string _roomId;

    IPetRaceRoom _roomData;

    public bool _inRace;
    int currentPetRanking = 1;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(this); }
    }

    private void Update()
    {
        if (_inRace) { HandlePetProgress(); }
    }

    void DisplayLobbyPets()
    {
        foreach (Transform obj in _lobbyPetsContainer)
        {
            Destroy(obj.gameObject);
        }

        for (int i = 0; i < _roomData._players.Count; i++)
        {
            if (_roomData._players[i].pet == null) { continue; }
            LobbyPet newPet = Instantiate(_lobbyPet, _lobbyPetsContainer);
            int posX = -3 + 3 * (i / 3);
            int posZ = -3 + 3 * (i % 3);
            newPet.transform.localPosition = new Vector3(posX, newPet.transform.localPosition.y, posZ);
            newPet.InitPet(_roomData._players[i].pet.name, _roomData._players[i].pet.modelNo);
        }
    }

    public async override Task InitCanvas()
    {
        await GetPerks();
        _roomId = DatabaseManager.Instance.RoomId;
        IPetRaceRoom newRoom = new IPetRaceRoom(_roomId);
        string json = JsonUtility.ToJson(newRoom);

        await FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(_roomId).SetRawJsonValueAsync(json);

        codeText.text = _roomId;
        FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(_roomId).ValueChanged += HandleDatabaseChange;
    }

    async Task GetPerks()
    {
        QuerySnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("PetRacePerks").GetSnapshotAsync();

        foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
        {
            IPetRacePerk newPerk = new IPetRacePerk("", "", "", "");
            newPerk.level = 1;
            Dictionary<string, object> perk = documentSnapshot.ToDictionary();

            foreach (KeyValuePair<string, object> pair in perk)
            {
                if (pair.Key == "id") { newPerk.id = (string)pair.Value; }
                if (pair.Key == "name") { newPerk.name = (string)pair.Value; }
                if (pair.Key == "description") { newPerk.description = (string)pair.Value; }
                if (pair.Key == "rarity") { newPerk.rarity = (string)pair.Value; }
            }

            perksList.Add(newPerk);
        }
    }

    async void HandleDatabaseChange(object sender, ValueChangedEventArgs args)
    {
        DataSnapshot roomData = await FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(_roomId).GetValueAsync();

        _roomData = JsonUtility.FromJson<IPetRaceRoom>(roomData.GetRawJsonValue());

        if (_roomData._players.Count == 0) { return; }

        switch(_roomData._state)
        {
            case "Lobby":
                HandleLobby();        
                break;
            case "Loadout":
                HandleLoadout();
                break;
            case "Race":
                HandleRace();
                break;
        }
    }

    void HandleLobby()
    {
        DisplayLobbyPets();
        foreach (IPetRacePlayer player in _roomData._players)
        {
            if (!player.isReady)
            {
                return;
            }
        }

        foreach (IPetRacePlayer player in _roomData._players)
        {
            player.isReady = false;
        }

        _roomData._state = "Loadout";
        UpdateRoom();
    }

    void HandleLoadout()
    {
        foreach (IPetRacePlayer player in _roomData._players)
        {
            if (!player.isReady)
            {
                return;
            }
        }

        _roomData._state = "Race";
        UpdateRoom();
    }


    async void HandleRace()
    {
        await StartRace();
    }

    void UpdateRoom()
    {
        string json = JsonUtility.ToJson(_roomData);

        FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(_roomId).SetRawJsonValueAsync(json);
    }

    public void HandleFinishRace(PetRacePet finishingPet)
    {
        AddScoreAndSetRerolls(finishingPet);
        foreach (PetRacePet pet in _petObjects)
        {
            if (pet.isRacing) { return; }
        }

        foreach (IPetRacePlayer player in _roomData._players)
        {
            player.coins = 10;
            player.isReady = false;
        }

        _roomData._state = "Loadout";
        HideProgressBar();
        raceCamera.StopCamera();

        foreach (PetRacePet pet in _petObjects)
        {
            Destroy(pet.gameObject);
        }
        _petObjects.Clear();
        _racetrack.DestroyTrack();
        _inRace = false;
        _roomData._turnNo++;
        ShowLeaderboard();

        UpdateRoom();
    }

    void AddScoreAndSetRerolls(PetRacePet pet)
    {
        foreach (IPetRacePlayer player in _roomData._players)
        {
            if (pet.id == player.id)
            {
                switch (currentPetRanking)
                {
                    case 1:
                        player.score += 100;
                        break;
                    case 2:
                        player.score += 70;
                        break;
                    case 3:
                        player.score += 50;
                        break;
                    default:
                        player.score += 20;
                        break;
                }
                player.freeRerolls = currentPetRanking - 1;
                currentPetRanking++;
                break;
            }
        }
    }

    void InitProgressBar()
    {
        _progressBar.gameObject.SetActive(true);
        for (int i = 0; i < _petObjects.Count; i++)
        {
            GameObject newIcon = Instantiate(_petIcon, _progressBar.transform);
            _petIcons.Add(newIcon.GetComponent<RectTransform>());
        }
    }

    void HandlePetProgress()
    {
        for (int i = 0; i < _petObjects.Count; i++)
        {
            float progress = _petObjects[i].raceProgress;

            _petIcons[i].anchoredPosition = new Vector2(progress * 700, 0);
        }
    }

    void HideProgressBar()
    {
        _progressBar.gameObject.SetActive(false);
        foreach(RectTransform icon in _petIcons)
        {
            Destroy(icon.gameObject);
        }
        _petIcons.Clear();
    }

    async Task StartRace()
    {
        if (_inRace) { return; }
        HideLeaderboard();
        currentPetRanking = 1;
        foreach (IPetRacePlayer player in _roomData._players)
        {
            PetRacePet newPet = Instantiate(_petObj);
            newPet.InitPet(player.pet);
            _petObjects.Add(newPet);
        }
        List<PetRaceTrack> newTracks = _racetrack.InitRacetrack(_petObjects, 100);
        PlacePetsOnTrack(_petObjects, newTracks);
        InitProgressBar();
        raceCamera.InitCamera(_petObjects);
        _inRace = true;

        await Task.Delay(3000);

        foreach(PetRacePet pet in _petObjects)
        {
            pet.StartRacing();
        }
    }

    public void PlacePetsOnTrack(List<PetRacePet> pet, List<PetRaceTrack> tracks)
    {
        for (int i = 0; i < pet.Count; i++)
        {
            pet[i].transform.parent = tracks[i].transform;
            pet[i].SetTrack(tracks[i]);
        }
    }

    void ShowLeaderboard()
    {
        SetupLeaderboard();
        _leaderboard.SetActive(true);
    }

    void HideLeaderboard()
    {
        _leaderboard.SetActive(false);
    }

    void SetupLeaderboard()
    {
        foreach (Transform item in _leaderboardPlayersContainer)
        {
            Destroy(item.gameObject);
        }
        _roomData._players.Sort(delegate (IPetRacePlayer a, IPetRacePlayer b)
        {
            return b.score.CompareTo(a.score);
        });

        for (int i = 0; i < _roomData._players.Count; i++)
        {
            if (i > 5) { break; }
            PetRaceLeaderboardPlayer card = Instantiate(_leaderboardPlayerContainer, _leaderboardPlayersContainer);
            card.SetUp(i + 1, _roomData._players[i]);
        }
    }
}
