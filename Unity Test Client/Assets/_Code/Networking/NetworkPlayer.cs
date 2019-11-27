using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerNameChanged")] public string playerName;
    [SyncVar(hook = "OnPlayerIdChanged")] public int id;
    [SyncVar(hook = "OnPlayerSuggest")] public CaseData suggestion;
    [SyncVar(hook = "OnPlayerAccuse")] public CaseData accusation;
    [SyncVar(hook = "OnDrawHand")] public SyncListString hand;
    [SyncVar(hook = "OnSuggestResult")] public string suggestResult;

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
        gameUi.MovePlayerMarker(id, id*4, 0);

        // We only want to control our own session
        if (!isLocalPlayer)
        {
            
        }
        else
        {
            // Get our local network manager (server)
            server = GameObject.Find("NetworkManager").GetComponent<GameServer>();

            gameManager.networkPlayers.Add(this);
            
            //gameUi.networkPlayer = this;
            //gameUi.gameManager = gameManager;
            gameManager.gameUi = gameUi;
            gameManager.myNetworkPlayer = this;

            // Sync the names across the network
            Cmd_ChangePlayerName(server.playerName);
        }
    }

    public void Update()
    {
        //gameUi.ShowPlayerBar(id);
        //gameUi.UpdatePlayerName(id, playerName);
        //gameUi.MovePlayerMarker(id, id*4, 0);
        gameUi.UpdatePlayerName(id, playerName);

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

    #region Commands
    // The command attribute is called by a client and executed on the server on that same object.
    // The client canonly call this on objects it has authority over
    [Command]
    void Cmd_ChangePlayerName(string n)
    {
        gameObject.name = $"{n}";

        Debug.Log($"Cmd_ChangePlayerName {n}");

        playerName = n;

        gameManager.playerNames.Add(playerName);
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
    void Cmd_Suggest(CaseData caseData)
    {
        suggestResult = null;

        Debug.Log($"Cmd_Acusse -- Player {id} making Suggestion");

        string proof = gameManager.MakeSuggestion(caseData);

        if (proof == null)
        {
            suggestResult = "No proof available";

            // TODO: Give player option to end their turn or make an accusation
        }
        else if (proof.Equals(caseData.character))
        {
            suggestResult = "Proof character is: " + proof;
        }
        else if (proof.Equals(caseData.room))
        {
            suggestResult = "Proof room is: " + proof;
        }
        else if (proof.Equals(caseData.weapon))
        {
            suggestResult = "Proof weapon is: " + proof;
        }

        int nextTurn = gameManager.playerTurn + 1;
        Cmd_EndTurn(nextTurn);
    }

    [Command]
    void Cmd_EndTurn(int turn)
    {
        Debug.Log($"Cmd_EndTurn -- Player: {id} ending turn: {turn}");
        gameManager.EndTurn(turn);
    }

    [Command]
    void Cmd_DrawHand(int numDrawn)
    {
        Debug.Log($"Cmd_DrawHand -- Player: {id} Drawing");

        List<string> h = gameManager.deck.DealRandom(numDrawn, ref gameManager.cards);

        // Convert to our list string
        for(int i=0; i<h.Count; i++)
        {
            hand.Add(h[i]);
        }
    }
    #endregion Commands

    #region Rpcs
    //The ClientRpc attribute is called on the server and executed on all other clients that are currently connected to the server
    // for the given object.  
    #endregion Rpcs

    #region TargetRpcs
    // The TargetRpc is only exected on a specific client when called on the server
    [TargetRpc]
    public void TargetRpc_DealCards(NetworkConnection conn)
    {

    }
    #endregion TargetRpcs

    public void MakeAccusation(CaseData caseData)
    {
        Cmd_Accuse(caseData);
        int nextTurn = gameManager.playerTurn + 1;
        Cmd_EndTurn(nextTurn);
    }

    public void MakeSuggestion(CaseData caseData)
    {
        Cmd_Suggest(caseData);
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

    void OnSuggestResult(string s)
    {
        suggestResult = s;
        if(s != null)
        {
            // TODO: Proof should only be shown to the player who made the suggestion
            gameUi.debugAccuseText.text = s;
        } else
        {
            gameUi.debugAccuseText.text = "";
        }
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

    // When suggestion is made
    void OnDrawHand(SyncListString newHand)
    {
        Debug.Log($":: OnDrawHand ::");

        hand = newHand;
    }

    //Sends the command to draw cards
    public void DrawHand(int i)
    {
        Cmd_DrawHand(i);
    }
}
