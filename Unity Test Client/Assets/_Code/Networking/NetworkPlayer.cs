using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerNameChanged")] public string playerName;
    [SyncVar(hook = "OnPlayerIdChanged")] public int id;
    //[SyncVar(hook = "OnPlayerMove")] public int roomId;
    //[SyncVar(hook = "OnPlayerTurn")] public bool isTurn;
    //[SyncVar(hook = "OnPlayerSuggest")] public CaseData suggestion;
    [SyncVar(hook = "OnPlayerAccuse")] public CaseData accusation;

    public GameServer server;
    public GameManagerService gameManager;
    public GameboardUi gameUi;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        gameUi = GameObject.Find("Game Manager").GetComponent<GameboardUi>();

        // We only want to control our own session
        if (!isLocalPlayer)
        {
            // But we want to set up the board for all
            gameUi.ShowPlayerBar(id);
            gameUi.UpdatePlayerName(id, playerName);
            gameUi.ShowPlayerMarker(id);
        }
        else
        {
            // Get our local network manager (server)
            server = GameObject.Find("NetworkManager").GetComponent<GameServer>();

            gameUi.ShowPlayerBar(id);
            gameUi.UpdatePlayerName(id, playerName);
            gameUi.ShowPlayerMarker(id);

            // Sync the names across the network
            Cmd_ChangePlayerName(server.playerName);
        }
    }

    public void Update()
    {
        // If we do not have local control over the player
        if (!isLocalPlayer)
        {
            gameUi.ShowPlayerBar(id);
            gameUi.UpdatePlayerName(id, playerName);
            gameUi.ShowPlayerMarker(id);
        }
        else
        {
            gameUi.ShowPlayerBar(id);
            gameUi.UpdatePlayerName(id, playerName);
            gameUi.ShowPlayerMarker(id);
        }
    }

    [Command]
    void Cmd_ChangePlayerName(string n)
    {
        gameObject.name = $"{n}";

        Debug.Log($"Cmd_ChangePlayerName {n}");

        playerName = n;
    }

    [Command]
    void Cmd_ChangePlayerId(int n)
    {
        Debug.Log($"Cmd_ChangePlayerId {n}");

        id = n;
    }

    [Command]
    void Cmd_MovePlayer()
    {

    }

    [Command]
    void Cmd_Accuse(CaseData accuse)
    {

    }

    [Command]
    void Cmd_Suggest()
    {

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

    // Send Player Move
    void OnPlayerMove()
    {
        Debug.Log($":: OnPlayerMove ::");
    }
}
