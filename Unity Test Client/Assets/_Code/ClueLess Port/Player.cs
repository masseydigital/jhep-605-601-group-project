using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Clueless
{
    public class Player : NetworkBehaviour
    {
        public SyncListCard hand = new SyncListCard();
        [SyncVar(hook = "OnPlayerNameChanged")] public string playerName;

        Deck deck;

        public void Start()
        {
            hand.Callback = OnHandChanged;

            deck = GameObject.Find("Deck").GetComponent<Deck>();

            if (isLocalPlayer)
            {
                Cmd_ChangePlayerName("Player " + Random.Range(0, 1000));

                Card c = deck.DrawCard();

                hand.Add(c);
                //Cmd_AddCard(c);
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
        public void Cmd_AddCard(Card card)
        {
            Debug.Log("Cmd_AddCard");

            hand.Add(card);
        }

        #region Callbacks
        void OnHandChanged(SyncListStruct<Card>.Operation op, int itemIndex)
        {
            Debug.Log("OnHandChanged: " + op);    
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


