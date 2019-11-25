/*GameManagerService.cs 
 * Brief:   This class controls the game state and
 *          synchronizes that state across the network.
 *          Each client will have one of these, but the
 *          server is ultimately responsbile for managing state.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManagerService : NetworkBehaviour
{
    [SyncVar(hook = "OnSetWinConditions")] public CaseData winConditions;
    [SyncVar(hook = "OnGameStateUpdate")] public int gameState;
    [SyncVar(hook = "OnTurnUpdate")] public int playerTurn;
    [SyncVar] public SyncListString cards;                                  // These are the cards in the game across the network
    [SyncVar] public SyncListString allCards;                               
    [SyncVar] public SyncListString playerNames;
    [SyncVar] public SyncListInt gameboardState;

    //private Player playerTurn;
    private DBConnection database;
                                                      // This is the deck of cards (6 characters, 6 weapons, 9 rooms)
    public GameData gameData;
    public GameboardUi gameUi;

    public List<string> characters;
    public List<string> weapons;
    public List<string> rooms;

    private float gameStartTimer = 0.0f;        // Keeps track of when the game will start
    private float startTime = 10.0f;            // Game will start after 10 seconds

    public Deck deck;
    public List<NetworkPlayer> networkPlayers;
    public NetworkPlayer myNetworkPlayer;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        database = GameObject.Find("DynamoDB Connection").GetComponent<DBConnection>();

        if (database != null)
            gameData = database.gameData;
    }

    // Start is called before the first frame update
    void Start()
    {
        characters = gameData.characterNames;
        weapons = gameData.weaponNames;
        rooms = gameData.roomNames;

        // We only want the server to set state
        if(isServer)
        //if (NetworkServer.active)
        {
            // Create a new deck
            deck = new Deck();
            deck.LoadDeck(characters, weapons, rooms);

            // This is the list of cards
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                cards.Add(deck.Cards[i]);
                allCards.Add(deck.Cards[i]);
            }
        }
        else 
        {
            // We want to get the servers information
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        int playerIndex = 0;
        string tmpCard = "";

        // If we are the server
        //if (NetworkServer.active)
        if(isServer)
        {
            switch (gameState)
            {
                // Waiting for game to start
                case (0):
                    // Game Start Conditions
                    if (playerNames.Count > 2)
                    {
                        gameState = 1;
                    }
                    break;

                // Initializing game data
                case (1):
                    if(winConditions.character == null || winConditions.character == "")
                    {
                        // Select random win conditions
                        winConditions = deck.GetCaseFile(
                           characters[Random.Range(0, characters.Count)],
                           weapons[Random.Range(0, weapons.Count)],
                           rooms[Random.Range(0, rooms.Count)]
                           );

                        cards.Remove(winConditions.character);
                        cards.Remove(winConditions.weapon);
                        cards.Remove(winConditions.room);
                    }
                    
                    // Deal cards one at a time to the players until all cards are delt
                    while (cards.Count > 0)
                    {
                        playerIndex = playerIndex % networkPlayers.Count;

                        tmpCard = cards[Random.Range(0, cards.Count - 1)];
                        networkPlayers[playerIndex].hand.Add(tmpCard);
                        cards.Remove(tmpCard);
                        
                        playerIndex++;
                    }

                    for(int i = 0; i<networkPlayers.Count; i++)
                    {                        
                        if(networkPlayers[i].hand.Count == 0)
                        {
                            // if we have someone equal to 0, we need to wiat
                            return;
                        }
                    }

                    gameState = 2;
                    break;
                // Game is ongoing
                case (2):

                    break;
            }
        }

        else if (localPlayerAuthority) // we are not the server, but we would like to make a move
        {
            //Debug.Log("I am the local Player");
           
            switch (gameState)
            {
                // Waiting for game to start
                case (0):
                    break;

                // Initializing game data
                case (1):                    
                    break;

                // Game is ongoing
                case (2):
                    break;
            }
        }
    }

    #region Commands
    // Commands are sent from the client to update the server through the On____ event.
    // When the server gets the value it will propogate that to all the cliengs through the syncvar

    void Cmd_UpdateLocation(SyncListInt locations)
    {
        Debug.Log("Cmd_UpdateLocation");
        gameboardState = locations;
    }
    #endregion Commands

    #region TargetRpcs
    // The TargetRpc is only exected on a specific client when called on the server
    void TargetRpc_DealHand(NetworkConnection conn)
    {
        
    }
    #endregion TargetRpcs

    #region SyncEvents
    void OnSuggestion(CaseData suggestion)
    {
        if (suggestion.character == winConditions.character &&
            suggestion.weapon == winConditions.weapon &&
            suggestion.room == winConditions.room)
        {
            // You win!
        }
        else
        {
            // You lose!
        }
    }

    void OnAccusation(CaseData accusation)
    {
        if(accusation.character == winConditions.character &&
            accusation.weapon == winConditions.weapon &&
            accusation.room == winConditions.room)
        {
            // You win!
        }
        else
        {
            // You lose!
        }
    }


    
    /// <summary>
    /// When the location is updated
    /// </summary>
    void OnLocationUpdate()
    {

    }

    /// <summary>
    /// Sets the case file.  This happens at the beginning of the game.
    /// </summary>
    void OnSetWinConditions(CaseData caseData)
    {
        winConditions.character = caseData.character;
        winConditions.weapon = caseData.weapon;
        winConditions.room = caseData.room;
    }

    /// <summary>
    /// Whenever we update the game state
    /// </summary>
    /// <param name="state"></param>
    void OnGameStateUpdate(int state)
    {
        Debug.Log($":: Transitioning state from {gameState} to {state} ::");

        gameState = state;
    }

    /// <summary>
    /// Whenever we update the game state
    /// </summary>
    /// <param name="state"></param>
    void OnTurnUpdate(int turn)
    {
        Debug.Log($":: Transitioning state from {playerTurn} to {turn} ::");

        playerTurn = turn;

        if (playerTurn >= playerNames.Count)
        {
            playerTurn = 0;
        }
    }

    public void EndTurn(int nextTurn)
    {
        Debug.Log("Ending turn: " + nextTurn);

        playerTurn = nextTurn;

        if(playerTurn >= playerNames.Count)
        {
            playerTurn = 0;
        }
    }
    #endregion SyncEvents

    #region Local Methods

    public string MakeSuggestion(CaseData caseData)
    {
        foreach(NetworkPlayer player in networkPlayers)
        {
            // don't check the current player for proof
            if(player.id != playerTurn)
            {
                // TODO: Have user select a card for their proof
                // player.gameUi.cardWindow.SetActive(true);
                // player.gameUi.InitializeProofCards(caseData.character, caseData.room, caseData.weapon);

                if(player.hand.Contains(caseData.character))
                {
                    return (caseData.character);
                }
                if(player.hand.Contains(caseData.room))
                {
                    return (caseData.room);
                }
                if(player.hand.Contains(caseData.weapon))
                {
                    return (caseData.weapon);
                }
            }
        }

        return null;
    }
    #endregion Local Methods
}

// Game States
public enum GameStates
{
    waiting = 0,
    init,
    ongoing,
    complete
};

