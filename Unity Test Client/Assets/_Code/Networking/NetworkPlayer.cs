using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerNameChanged")] public string playerName;
    [SyncVar(hook = "OnPlayerIdChanged")] public int id;

    public GameServer server;

    public void Awake()
    {
        DontDestroyOnLoad(this);  
    }

    public void Start()
    {
        // We only want to control our own session
        if (!isLocalPlayer)
            return;

        // Get our local network manager (server)
        server = GameObject.Find("NetworkManager").GetComponent<GameServer>();

        // Sync the names across the network
        Cmd_ChangePlayerName(server.playerName);
    }

    public void Update()
    {
        if (!isLocalPlayer)
            return;
    }

    [Command]
    void Cmd_ChangePlayerName(string n)
    {
        Debug.Log($"Cmd_ChangePlayerName {n}");

        playerName = n;
    }

    [Command]
    void Cmd_ChangePlayerId(int n)
    {
        Debug.Log($"Cmd_ChangePlayerId {n}");

        id = n;
    }

    // Changes the player name
    void OnPlayerNameChanged(string newName)
    {
        Debug.Log($"OnPlayerNameChanged:: Old Name: {playerName} New Name: {newName}");

        playerName = newName;

        gameObject.name = $"{newName}";
    }

    // Changes the player id
    void OnPlayerIdChanged(int newId)
    {
        Debug.Log($"OnPlayerNameChanged:: Old Name: {playerName} New Name: {newId}");

        id = newId;
    }
}
