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
    [SyncVar(hook = "OnPlayerSuggest")] public CaseData suggestion;
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
        gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManagerService>();

        // But we want to set up the board for all
        gameUi.ShowPlayerBar(id);
        gameUi.UpdatePlayerName(id, playerName);
        gameUi.ShowPlayerMarker(id);

        // We only want to control our own session
        if (!isLocalPlayer)
        {
            
        }
        else
        {
            // Get our local network manager (server)
            server = GameObject.Find("NetworkManager").GetComponent<GameServer>();
            
            gameUi.networkPlayer = this;
            gameUi.gameManager = gameManager;
            gameManager.gameUi = gameUi;

            // Sync the names across the network
            Cmd_ChangePlayerName(server.playerName);
        }
    }

    public void Update()
    {
        gameUi.ShowPlayerBar(id);
        gameUi.UpdatePlayerName(id, playerName);
        gameUi.ShowPlayerMarker(id);

        // If we do not have local control over the player
        if (!isLocalPlayer)
        {

        }
        else
        {
            // This means it's my turn :D
            if(gameManager.gameState == 2 && gameManager.playerTurn == id)
            {
                gameUi.ShowActionButtons();
            }
            else // it's not your turn
            {
                gameUi.HideActionButtons();
            }
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
        Debug.Log($"Cmd_Acusse -- Player {id} making accusation");
    }

    [Command]
    void Cmd_Suggest(CaseData suggest)
    {
        Debug.Log($"Cmd_Acusse -- Player {id} making Suggestion");
    }

    [Command]
    void Cmd_EndTurn(int turn)
    {
        Debug.Log($"Cmd_EndTurn -- Player: {id} ending turn: {turn}");
        gameManager.EndTurn(turn);
    }

    public void MakeAccusation(CaseData caseData)
    {
        Cmd_Accuse(caseData);
        int nextTurn = gameManager.playerTurn + 1;
        Cmd_EndTurn(nextTurn);
    }

    public void MakeSuggestion(CaseData caseData)
    {
        Cmd_Suggest(caseData);
        int nextTurn = gameManager.playerTurn + 1;
        Cmd_EndTurn(nextTurn);
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

    // When an accusation is made
    void OnPlayerAccuse(CaseData player)
    {
        Debug.Log($":: OnPlayerAccuse ::");
    }

    // When suggestion is made
    void OnPlayerSuggest(CaseData player)
    {
        Debug.Log($":: OnPlayerSuggest ::");
    }
}
