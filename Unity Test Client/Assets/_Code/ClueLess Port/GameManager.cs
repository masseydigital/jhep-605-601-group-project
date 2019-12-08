using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClueLess
{
    public class GameManager : NetworkBehaviour
    {
        [SyncVar(hook = "OnGameStateUpdate")] public int gameState;
        [SyncVar(hook = "OnTurnUpdate")] public int playerTurn;
        [SyncVar(hook = "OnSetSuggestion")] public CaseData currentSuggestion;
        public SyncListCard winConditions;      // Three cards of differing categories

        public Deck deck;
        public GameServer server;
        public GameboardUi gameboardUi;
        public DBConnection database;
        public GameData gameData;
        public NetPlayer myPlayer;

        private void Awake()
        {
            if (server == null)
            {
                server = GameObject.Find("NetworkManager").GetComponent<GameServer>();
            }

            if(database == null)
            {
               database = GameObject.Find("DynamoDB Connection").GetComponent<DBConnection>();
               gameData = database.gameData;
            }
            else
            {
                gameData = database.gameData;
            }                
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void OnStartClient()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            // If we are the server
            //if (NetworkServer.active)
            if (isServer)
            {
                switch (gameState)
                {
                    // Waiting for game to start
                    case (0):
                        // Game Start Conditions
                        if (server.players.Count >= 2)
                        {
                            gameState = 1;
                        }
                        playerTurn = -1;
                        break;

                    // Initializing game data
                    case (1):
                        if (winConditions.Count < 3)
                        {
                            deck.Shuffle();
                            // Select the win conditions... 0 is character, 1 is weapon, 2 is room
                            winConditions.Add(deck.GetCard(0));
                            winConditions.Add(deck.GetCard(1));
                            winConditions.Add(deck.GetCard(2));
                        }

                        int p = 0;
                        int d = deck.cards.Count;
                        // Deal out the rest of the cards
                        for(int i=0; i<d; i++)
                        {
                            Card c = deck.DrawCard();

                            server.players[p].hand.Add(c);

                            p++;

                            if (p >= server.players.Count)
                            {
                                p = 0;
                            }
                        }

                        playerTurn = Random.Range(0, server.players.Count);
                        TurnCheck();
                        gameboardUi.UpdateCrowns(playerTurn);
                        gameState = 2;
                        break;
                    // Game is ongoing
                    case (2):
                        break;
                }
            }

            else if (localPlayerAuthority) // we are not the server, but we would like to make a move
            {
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


        #region Callbacks
        /// <summary>
        /// Sets the case file.  This happens at the beginning of the game.
        /// </summary>
        void OnSetWinConditions(SyncListCard cards)
        {
            winConditions[0] = cards[0];    // Character
            winConditions[1] = cards[1];    // Weapon
            winConditions[2] = cards[2];    // Room
        }
        
        /// <summary>
        /// Sets the suggestion cards. This happens whenever a player makes a suggestion.
        /// </summary>
        void OnSetSuggestion(CaseData suggestion)
        {
            currentSuggestion = suggestion;

            Debug.Log("GameManager.OnSetSuggestion: Current suggestion: " + suggestion.character + "-"
            + suggestion.room + "-" + suggestion.weapon);

            if (playerTurn != myPlayer.playerInfo.id)
            {
                gameboardUi.OpenSuggestionWindow(currentSuggestion);
            }
            else
            {
                gameboardUi.CloseSuggestionWindow();
            }
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
            Debug.Log($":: Transitioning turn from {playerTurn} to {turn} ::");

            playerTurn = turn;

            gameboardUi.UpdateCrowns(playerTurn);

            // If it is our players turn then party
            if (playerTurn == myPlayer.playerInfo.id)
            {
                gameboardUi.ShowActionButtons();
            }
            else
            {
                gameboardUi.HideActionButtons();
            }
        }
        #endregion

        public void NextTurn()
        {
            playerTurn++;

            TurnCheck();
        }

        public void TurnCheck()
        {
            if(playerTurn > server.players.Count-1)
            {
                playerTurn = 0;
            }
        }

        public void UpdateCurrentSuggestion(CaseData suggestion)
        {
            Debug.Log("GameManager.UpdateCurrentSuggestion: " + suggestion.character + "," + suggestion.room
            + "," + suggestion.weapon);
            currentSuggestion = suggestion;
        }        
    }

    public enum GameStates
    {
        waiting = 0,
        init,
        ongoing,
        complete
    };
}


