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
        [SyncVar] public SyncListInt gameboardState;
        public SyncListCard winConditions;      // Three cards of differing categories
        public SyncListPlayer playerNames;      // List of all players

        // Start is called before the first frame update
        void Start()
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
                        if (playerNames.Count > 2)
                        {
                            gameState = 1;
                        }
                        break;

                    // Initializing game data
                    case (1):
                        if (winConditions.Count < 3)
                        {
                            // Select the win conditions
                            
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
        #endregion
    }

    public enum GameStates
    {
        waiting = 0,
        init,
        ongoing,
        complete
    };
}


