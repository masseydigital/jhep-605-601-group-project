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
                    Debug.Log("My hand is: +" + myNetworkPlayer.hand.Count);
                    if(myNetworkPlayer.hand.Count == 0)
                    {
                        int numToDraw = 18 / playerNames.Count;
                        myNetworkPlayer.DrawHand(numToDraw);
                        //EndTurn(playerTurn + 1);
                    }
                    
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
                if(player.hand.Contains(caseData.character))
                {
                    return caseData.character;
                } else if(player.hand.Contains(caseData.room))
                {
                    return caseData.room;
                } else if(player.hand.Contains(caseData.weapon))
                {
                    return caseData.weapon;
                }
            }
        }

        return null;
    }

    /*
    public bool addPlayer(Player player)
    {
        if (players.Count <= GameDefines.PARTY_SIZE_LIMIT)
        {
            // TODO: Alert others that a player has joined the game

            //players.Add(player);

            return true;
        }
        else
        {
            // TODO: Alert player that party size limit has been reached
            return false;
        }
    }

    public bool removePlayer(Player player)
    {
        //if (players.Exists(player))
        if(player != null)
        {
            if (playerTurn == player)
            {
                // Go to next player turn
                incrementTurn();
            }

            // TODO: Alert other players that someone is leaving

            players.Remove(player);

            return true;
        }
        else
        {
            // Requested player not in game
            return false;
        }
    }

    public int getNumPlayers()
    {
        return players.Count;
    }

    public bool setPlayerTurn(Player player)
    {
        //if (players.Exists(player))
        if(player != null)
        {
            playerTurn = player;

            // TODO: Alert players of change in turn

            return true;
        }
        else
        {
            // The player is not in the game
            return false;
        }
    }

    public bool incrementTurn()
    {
        int playerTurnIndex = 0;
        int playerCount = getNumPlayers();;
        
        if (playerCount == 1)
        {
            // Only one player in game so make it their turn
            playerTurn = players[0];

            // TODO: Alert players of change in turn

            return true;
        }
        else if (playerCount <= 0)
        {
            return false;
        }
        else
        {
            //if (players.Exists(playerTurn))
            if(playerTurn != null)
            {
                playerTurnIndex = players.IndexOf(playerTurn);

                // Set player turn to next player in players list
                playerTurn = players[(playerTurnIndex + 1) % playerCount];

                // TODO: Alert players of change in turn

                return true;
            }
            else
            {

            }
        }

        return false;
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

    public Player getTurn()
    {
        return playerTurn;
    }

    public bool checkWinner(Player player)
    {
        if (player.winLoose == 1) return true;
        else return false;
    }

    public bool checkLoser(Player player)
    {
        if (player.winLoose != 1) return true;
        else return false;
    }
    */
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

