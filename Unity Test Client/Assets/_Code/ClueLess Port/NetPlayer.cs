using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClueLess
{
    public class NetPlayer : NetworkBehaviour
    {
        [SyncVar] public Player playerInfo;
        [SyncVar(hook = "OnPlayerNameChanged")] public string playerName;
        [SyncVar] public bool hasLost;
        public SyncListCard hand = new SyncListCard();

        public int currentRoom;
        public Gameboard gameboard;
        public GameboardUi gameboardUi;
        public GameServer server;
        public GameManager gameManager;

        // Deck is a networked object that every player has access to
        Deck deck;

        public void Start()
        {
            hand.Callback = OnHandChanged;
       
            deck = GameObject.Find("Deck").GetComponent<Deck>();
            gameboardUi = GameObject.Find("Game Manager").GetComponent<GameboardUi>();
            gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameboard = GameObject.Find("Gameboard").GetComponent<Gameboard>();

            hasLost = false;

            // Let me control my own player only
            if (isLocalPlayer)
            {
                // Get our local network manager (server)
                server = GameObject.Find("NetworkManager").GetComponent<GameServer>();
                gameboardUi.networkPlayer = this;

                Cmd_ChangePlayerName(server.playerName);

                gameManager.myPlayer = this;
            }
            else
            {
                OnPlayerNameChanged(playerName);
            }

            gameboardUi.ShowPlayerBar(playerInfo.id);
            gameboardUi.MovePlayerMarker(playerInfo.id, 20, playerInfo.id * 4);
            currentRoom = playerInfo.id * 4;
        }

        public void Update()
        {
            // Only the local player can do things
            if (isLocalPlayer)
            {
                // If it is our turn
                if(gameManager.playerTurn == playerInfo.id)
                {
                    // We can choose to move somewhere

                    // We can accuse someone

                    // We can make a suggestion 
                }
            }
        }

        // The command attribute is called by a client and executed on the server on that same object.
        // The client canonly call this on objects it has authority over
        [Command]
        void Cmd_ChangePlayerName(string n)
        {
            Debug.Log($"Cmd_ChangePlayerName {n}");

            gameObject.name = $"{n}";

            playerName = n;

            playerInfo.name = n;

            gameboardUi.UpdatePlayerName(playerInfo.id, playerInfo.name);
        }

        /// <summary>
        /// server player getting added a card
        /// </summary>
        /// <param name="card"></param>
        [Command]
        public void Cmd_DrawCard()
        {
            Debug.Log("Cmd_AddCard");

            hand.Add(deck.DrawCard());
        }

        [Command]
        public void Cmd_EndTurn()
        {
            Debug.Log("Cmd_EndTurn");

            gameManager.NextTurn();
        }

        [Command]
        public void Cmd_MakeSuggestion(CaseData suggestion)
        {
            Debug.Log("NetPlayer.Cmd_MakeSuggestion: " + suggestion.character + ", " + suggestion.room + ", " + suggestion.weapon);

            gameManager.UpdateCurrentSuggestion(suggestion);
        }

        [Command]
        public void Cmd_MakeAccusation(CaseData accusation)
        {
            Debug.Log("NetPlayer.Cmd_MakeAccusation: " + accusation.character + ", " + accusation.room + ", " + accusation.weapon);

            gameManager.CheckWinConditions(accusation);
        }

        [Command]
        public void Cmd_InitializeProof()
        {
            gameManager.NextProofTurn();
        }

        [Command]
        public void Cmd_MakeProof(int suggestionCardIndex)
        {
            Debug.Log($"NetPlayer.Cmd_MakeProof: Proving suggestion card {suggestionCardIndex}");
            Debug.Log($"NetPlayer.Cmd_MakeProof: {playerInfo.name}({playerInfo.id}) proving suggeestion card {suggestionCardIndex}");
            gameManager.MakeProof(suggestionCardIndex);
        }

        #region Callbacks
        void OnHandChanged(SyncListCard.Operation op, int itemIndex)
        {
            Debug.Log("OnHandChanged: " + op);

            switch (op)
            { 
                // Add operation
                case SyncList<Card>.Operation.OP_ADD:
                    break;
                // Clear Operation
                case SyncList<Card>.Operation.OP_CLEAR:
                    hand.Clear();
                    break;
                //Insert Operation
                case SyncList<Card>.Operation.OP_INSERT:
                    break;
                // Remove operation
                case SyncList<Card>.Operation.OP_REMOVE:
                    
                    break;
                // RemoveAt Operation
                case SyncList<Card>.Operation.OP_REMOVEAT:
                    hand.RemoveAt(itemIndex);
                    break;
                // Set Operation
                case SyncList<Card>.Operation.OP_SET:
                    break;
            }

        }

        // Changes the player name
        void OnPlayerNameChanged(string newName)
        {
            Debug.Log($"OnPlayerNameChanged:: Old Name: {playerName} New Name: {newName}");

            playerName = newName;

            gameObject.name = $"{newName}";

            playerInfo.name = newName;

            gameboardUi.UpdatePlayerName(playerInfo.id, playerInfo.name);
        }
        #endregion Callbacks
    }
}


