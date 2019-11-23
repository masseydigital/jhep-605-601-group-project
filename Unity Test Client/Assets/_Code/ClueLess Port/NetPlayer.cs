using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClueLess
{
    public class NetPlayer : NetworkBehaviour
    {
        [SyncVar(hook = "OnPlayerNameChanged")] public string playerName;
        public SyncListCard hand = new SyncListCard();

        public Room currentRoom;

        // Deck is a networked object that every player has access to
        Deck deck;

        public void Start()
        {
            hand.Callback = OnHandChanged;

            deck = GameObject.Find("Deck").GetComponent<Deck>();

            // Let me control my own player only
            if (isLocalPlayer)
            {
                Cmd_ChangePlayerName("Player " + Random.Range(0, 1000));

                Cmd_DrawCard();
            }
            else
            {
                OnPlayerNameChanged(playerName);
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
        }
        #endregion Callbacks
    }
}


