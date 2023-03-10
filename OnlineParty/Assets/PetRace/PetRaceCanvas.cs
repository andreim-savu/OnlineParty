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

    public IPetRaceRoom(string roomCode)
    {
        _gameName = "PetRace";
        _roomCode = roomCode;
        _state = "Lobby";
        _players = new List<IPetRacePlayer>();
    }
}

[System.Serializable]
public class IPetRacePlayer
{
    public string id;
    public string name;
    public IPetRacePet pet;
    public List<IPetRacePerk> inventoryPerks;
    public List<IPetRacePerk> perkSelectionPerks;
    public bool isReady;
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

    public IPetRacePet(string _id, string _name, int _movementSpeed, int _dodge, int _regen, int _depletion)
    {
        id = _id;
        name = _name;
        movementSpeed = _movementSpeed;
        dodge = _dodge;
        regen = _regen;
        depletion = _depletion;

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

    string _roomId;

    IPetRaceRoom _roomData;

    public bool _inRace;

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
            int posX = -3 + 3 * (i % 3);
            int posZ = -3 + 3 * (i / 3);
            newPet.transform.localPosition = new Vector3(posX, newPet.transform.localPosition.y, posZ);
            newPet.InitPet(_roomData._players[i].pet.name);
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
            AssignPerksToPlayer(player);
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

    public void HandleFinishRace()
    {
        foreach (PetRacePet pet in _petObjects)
        {
            if (pet.isRacing) { return; }
        }

        foreach (IPetRacePlayer player in _roomData._players)
        {
            player.isReady = false;
            AssignPerksToPlayer(player);
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

        UpdateRoom();
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

    void AssignPerksToPlayer(IPetRacePlayer player)
    {
        List<IPetRacePerk> tempPerks = new List<IPetRacePerk>();

        List<IPetRacePerk> potentialPerks = new List<IPetRacePerk>();

        List<IPetRacePerk> commonPerks = new List<IPetRacePerk>();
        List<IPetRacePerk> rarePerks = new List<IPetRacePerk>();
        List<IPetRacePerk> epicPerks = new List<IPetRacePerk>();
        List<IPetRacePerk> legendaryPerks = new List<IPetRacePerk>();

        foreach (IPetRacePerk item in perksList)
        {
            tempPerks.Add(new IPetRacePerk(item.id, item.name, item.description, item.rarity));
        }

        foreach (IPetRacePerk perk in player.inventoryPerks)
        {
            foreach (IPetRacePerk perkInList in tempPerks)
            {
                if (perk.id == perkInList.id)
                {
                    if (perk.level == 5)
                    {
                        perkInList.level = 5;
                        continue;
                    }
                    else
                    {
                        perkInList.level = perk.level + 1;
                        if (perkInList.rarity == "Common") { commonPerks.Add(perkInList); }
                        else if (perkInList.rarity == "Rare") { rarePerks.Add(perkInList); }
                        else if (perkInList.rarity == "Epic") { epicPerks.Add(perkInList); }
                        else if (perkInList.rarity == "Legendary") { legendaryPerks.Add(perkInList); }
                    }
                }
            }
        }

        foreach (IPetRacePerk perk in player.pet.perks)
        {
            foreach (IPetRacePerk perkInList in tempPerks)
            {
                if (perk.id == perkInList.id)
                {
                    if (perk.level == 5)
                    {
                        perkInList.level = 5;
                        continue;
                    }
                    else
                    {
                        perkInList.level = perk.level + 1;
                        if (perkInList.rarity == "Common") { commonPerks.Add(perkInList); }
                        else if (perkInList.rarity == "Rare") { rarePerks.Add(perkInList); }
                        else if (perkInList.rarity == "Epic") { epicPerks.Add(perkInList); }
                        else if (perkInList.rarity == "Legendary") { legendaryPerks.Add(perkInList); }
                    }
                }
            }
        }

        foreach (IPetRacePerk perk in tempPerks)
        {
            if (perk.level == 1)
            {
                if (perk.rarity == "Common") { commonPerks.Add(perk); }
                else if (perk.rarity == "Rare") { rarePerks.Add(perk); }
                else if (perk.rarity == "Epic") { epicPerks.Add(perk); }
                else if (perk.rarity == "Legendary") { legendaryPerks.Add(perk); }
            }
        }


        for (int i = 0; i < 3; i++)
        {
            int randomNo = Random.Range(0, 101);
            if (randomNo <= 65)
            {
                int randomIndex = Random.Range(0, commonPerks.Count);
                potentialPerks.Add(commonPerks[randomIndex]);
                commonPerks.RemoveAt(randomIndex);
            }
            else if(randomNo <= 85)
            {
                int randomIndex = Random.Range(0, rarePerks.Count);
                potentialPerks.Add(rarePerks[randomIndex]);
                rarePerks.RemoveAt(randomIndex);
            }
            else if(randomNo <= 95)
            {
                int randomIndex = Random.Range(0, epicPerks.Count);
                potentialPerks.Add(epicPerks[randomIndex]);
                epicPerks.RemoveAt(randomIndex);
            }
            else
            {
                int randomIndex = Random.Range(0, legendaryPerks.Count);
                potentialPerks.Add(legendaryPerks[randomIndex]);
                legendaryPerks.RemoveAt(randomIndex);
            }
        }

        player.perkSelectionPerks = potentialPerks;
    }
}
