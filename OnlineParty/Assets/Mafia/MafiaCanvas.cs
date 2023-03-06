using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System.Threading.Tasks;
using TMPro;

[System.Serializable]
public class MafiaRoom
{
    public string _roomCode;
    public string _gameName;
    public List<IMafiaPlayer> _players;
    public string _state;

    public MafiaRoom(string roomCode)
    {
        _gameName = "Mafia";
        _roomCode = roomCode;
        _state = "Lobby";
        _players = new List<IMafiaPlayer>();
    }
}

[System.Serializable]
public class IMafiaPlayer
{
    public string id;
    public string username;
    public string role;
    public string targetId;
    public string lastTargetId;
    public bool targetLocked;
    public bool alive;
    public bool host;
}

public class MafiaCanvas : GameCanvas
{
    [SerializeField] Transform lobbyPlayersContainer;
    [SerializeField] GameObject lobbyPlayerContainer;
    [SerializeField] GameObject playersContainer;
    [SerializeField] MafiaPlayer playerObject;
    List<MafiaPlayer> playerObjects = new List<MafiaPlayer>();
    [SerializeField] Light light;

    string _roomId;

    MafiaRoom roomData;

    public async override Task InitCanvas()
    {
        _roomId = DatabaseManager.Instance.RoomId;
        MafiaRoom newRoom = new MafiaRoom(_roomId);
        string json = JsonUtility.ToJson(newRoom);

        await FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(_roomId).SetRawJsonValueAsync(json);

        codeText.text = _roomId;
        FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(_roomId).ValueChanged += HandleDatabaseChange;
    }

    void HandleDatabaseChange(object sender, ValueChangedEventArgs args)
    {
        DataSnapshot snapshot = args.Snapshot;

        roomData = JsonUtility.FromJson<MafiaRoom>(snapshot.GetRawJsonValue());

        switch (roomData._state)
        {
            case "Lobby":
                DisplayLobbyPlayers();
                break;
            case "Roles":
                AssignRoles();
                break;
            case "Mafia":
                HandleMafia();
                break;
            case "Cop":
                HandleCop();
                break;
            case "Medic":
                HandleMedic();
                break;
            case "Day":
                HandleDay();
                break;
            case "Vote":
                HandleVote();
                break;
        }
    }

    void DisplayLobbyPlayers()
    {
        foreach (Transform child in playersContainer.transform) {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < roomData._players.Count; i++)
        {
            MafiaPlayer newPlayer = Instantiate(playerObject, playersContainer.transform);
            newPlayer.InitPlayer(roomData._players[i].username, roomData._players[i].id);
            newPlayer.transform.localPosition = new Vector3(-9 + i * 2, 0, 0);
            playerObjects.Add(newPlayer);
        }
    }

    List<string> GetRolesByNumberOfPlayers(int noPlayers)
    {
        switch(noPlayers)
        {
            case 4:
                return new List<string>() { "Mafia", "Medic", "Village", "Cop" };
            case 5:
                return new List<string>() { "Mafia", "Village", "Village", "Cop", "Village" };
            case 6:
                return new List<string>() { "Mafia", "Village", "Village", "Cop", "Medic", "Mafia" };
            case 7:
                return new List<string>() { "Mafia", "Village", "Village", "Cop", "Medic", "Mafia", "Village" };
            case 8:
                return new List<string>() { "Mafia", "Village", "Village", "Cop", "Medic", "Mafia", "Village", "Village" };
            case 9:
                return new List<string>() { "Mafia", "Village", "Village", "Cop", "Medic", "Mafia", "Village", "Village", "Mafia" };
            case 10:
                return new List<string>() { "Mafia", "Village", "Village", "Cop", "Medic", "Mafia", "Village", "Village", "Mafia", "Village" };
        }
        return new List<string>();
    }
 
    void AssignRoles()
    {
        List<string> roles = GetRolesByNumberOfPlayers(roomData._players.Count);
        for (int i = 0; i < roomData._players.Count; i++)
        {
            int randomRoleIndex = Random.Range(0, roles.Count);
            roomData._players[i].role = roles[randomRoleIndex];
            roles.RemoveAt(randomRoleIndex);
        }
        light.intensity = 0;
        playersContainer.SetActive(false);
        roomData._state = "Mafia";
        UpdateRoom();
    }

    async void HandleMafia()
    {
        foreach (IMafiaPlayer player in roomData._players)
        {
            if (player.role != "Mafia" || !player.alive) { continue; }

            if (!player.targetLocked) { return; }
            player.targetLocked = false;

            print(player.username + " has selected " + player.targetId + " as a target.");
        }

        await WaitForSeconds(2);
        roomData._state = "Cop";
        UpdateRoom();
    }

    async void HandleCop()
    {
        foreach (IMafiaPlayer player in roomData._players)
        {
            if (player.role != "Cop") { continue; }

            if (!player.alive)
            {
                await WaitForSeconds(3);
                roomData._state = "Medic";
                UpdateRoom();
                return;
            }

            if (!player.targetLocked) { return; }
            player.targetLocked = false;
            print(player.username + " has selected " + player.targetId + " to investigate.");
        }

        await WaitForSeconds(3);
        roomData._state = "Medic";
        UpdateRoom();
    }

    async void HandleMedic()
    {
        foreach (IMafiaPlayer player in roomData._players)
        {
            if (player.role != "Medic") { continue; }

            if (!player.alive)
            {
                await WaitForSeconds(3);
                roomData._state = "Day";
                UpdateRoom();
                return;
            }

            if (!player.targetLocked) { return; }
            player.targetLocked = false;
            player.lastTargetId = player.targetId;
            print(player.username + " has selected " + player.targetId + " to protect.");
        }
        await WaitForSeconds(2);
        light.intensity = 1;
        playersContainer.SetActive(true);
        roomData._state = "Day";
        UpdateRoom();
    }

    async void HandleDay()
    {
        List<string> mafiaTargets = new List<string>();
        string medicTarget = null;

        foreach (IMafiaPlayer player in roomData._players)
        {
            if (player.role == "Mafia" && player.alive)
            {
                mafiaTargets.Add(player.targetId);
            }

            if (player.role == "Medic" && player.alive)
            {
                medicTarget = player.targetId;
            }

            player.targetId = "";
            player.targetLocked = false;
        }

        string mafiaTarget = mafiaTargets[Random.Range(0, mafiaTargets.Count)];

        if (medicTarget != mafiaTarget)
        {
            print("The mafia has killed " + mafiaTarget);
            foreach (IMafiaPlayer player in roomData._players)
            {
                if (player.id == mafiaTarget)
                {
                    player.alive = false;
                }
            }
        }

        else
        {
            print("The medic has protected " + medicTarget);
        }

        SetPlayerNameDead();

        if (isGameOver()) { return; }

        await WaitForSeconds(2);

        roomData._state = "Vote";
        UpdateRoom();
    }

    async void HandleVote()
    {

        Dictionary<string, int> playerVotes = new Dictionary<string, int>();
        foreach (IMafiaPlayer player in roomData._players)
        {
            if (!player.alive) { continue; }
            if (!player.targetLocked) { return; }

            if (playerVotes.ContainsKey(player.targetId)) { playerVotes[player.targetId]++; }
            else { playerVotes.Add(player.targetId, 0); }
        }

        string votedPlayerId = "";
        int votes = 0;
        bool tie = false;

        foreach (KeyValuePair<string, int> kvp in playerVotes)
        {
            if (votedPlayerId == "") { 
                votedPlayerId = kvp.Key;
                votes = kvp.Value;
                continue;
            }

            if (kvp.Value > votes)
            {
                votedPlayerId = kvp.Key;
                votes = kvp.Value;
                tie = false;
                continue;
            }

            if (kvp.Value == votes)
            {
                tie = true;
                continue;
            }
        }

        foreach (IMafiaPlayer player in roomData._players)
        {
            if (player.id == votedPlayerId && !tie)
            {
                print("The town has voted " + player.username);
                player.alive = false;
            }

            player.targetId = "";
            player.targetLocked = false;
        }

        SetPlayerNameDead();

        if (isGameOver()) { return; }

        await WaitForSeconds(2);

        roomData._state = "Mafia";
        UpdateRoom();
    }

    bool isGameOver()
    {
        int townCount = 0;
        int mafiaCount = 0;

        foreach (IMafiaPlayer player in roomData._players)
        {
            if (!player.alive) { continue; }

            if (player.role == "Mafia") { mafiaCount++; }
            else { townCount++; }
        }

        if (mafiaCount == 0)
        {
            roomData._state = "TownWin";
            print("Town won");
            UpdateRoom();
            return true;
        }

        else if (townCount <= mafiaCount)
        {
            roomData._state = "MafiaWin";
            print("Mafia won");
            UpdateRoom();
            return true;
        }

        return false;
    }

    async Task WaitForSeconds(int seconds)
    {
        await Task.Delay(seconds * 1000);
    }

    void UpdateRoom()
    {
        string json = JsonUtility.ToJson(roomData);

        FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(_roomId).SetRawJsonValueAsync(json);
    }

    void SetPlayerNameDead()
    {
        foreach (MafiaPlayer playerObj in playerObjects)
        {
            foreach (IMafiaPlayer player in roomData._players)
            {
                if (playerObj.id == player.id)
                {
                    if (!player.alive)
                    {
                        playerObj.setTextDead();
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(_roomId).ValueChanged -= HandleDatabaseChange;
    }
}