using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;

[System.Serializable]
struct Player
{
    public string id;
    public string username;
}

[System.Serializable]
struct Room
{
    public string _roomCode;
    public string _gameName;
    public List<Player> _players;
    public string _state;

    public Room(string state, string gameName, string roomCode, List<Player> players)
    {
        _gameName = gameName;
        _roomCode = roomCode;
        _state = state;
        _players = players;
    }
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    public string RoomId;

    DatabaseReference reference;

    private void Awake()
    {
        if (!Instance) { Instance = this; }
        else { Destroy(this); }
    }

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void HandleChange(object sender, ValueChangedEventArgs args)
    {
        DataSnapshot snapshot = args.Snapshot;
        print(snapshot.Value);
    }

    async public Task CreateRoom(string gameName)
    {
        string roomId = GenerateRandomId();
        DataSnapshot roomData = await FirebaseDatabase.DefaultInstance.GetReference("rooms").Child(roomId).GetValueAsync();
        if (roomData.Value != null)
        {
            await CreateRoom(gameName);
            return;
        }

        RoomId = roomId;
    }

    string GenerateRandomId()
    {
        string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string id = "";

        for (int i = 0; i < 4; i++)
        {
            id += characters[Random.Range(0, characters.Length)];
        }

        return id;
    }
}