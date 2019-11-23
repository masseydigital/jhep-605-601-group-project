/* Deck.cs
 * Brief:  This class holds a list of cards and is updated
 *          across the network
 * 
 * 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

namespace Clueless
{
    public class Deck : NetworkBehaviour
    {
        public SyncListCard cards = new SyncListCard();  // a synced list of all the cards in the deck

        //public TextMeshProUGUI displayText;

        public override void OnStartServer()
        {
            GenerateTest();
        }

        public void Start()
        {
            cards.Callback = OnDeckChanged;
        }

        #region Methods
        /// <summary>
        /// Creates the deck based on an input of card names
        /// there should be 21 cards in the deck.. 6 players, 6 weapons, and 9 rooms
        /// </summary>
        /// <param name="cardNames"></param>
        public void CreateDeck(List<Card> cards)
        {
            for(int i=0; i<cards.Count; i++)
            {
                //Cmd_AddNewCard(i, cards[i].name, cards[i].category);
                cards.Add(new Card(i, cards[i].name, cards[i].category));
            }
        }

        [Command]
        public void Cmd_DrawCard(SyncListCard hand)
        {
            Card c = DrawCard();

            hand.Add(c);
        }

        /// <summary>
        /// Draws a card from the deck given an id
        /// </summary>
        /// <param name="id"></param>
        public Card DrawCard()
        {
            Card c = cards[0];

            cards.Remove(c);

            return c;
        }

        /// <summary>
        /// Shuffles the cards in the deck
        /// </summary>
        public void Shuffle()
        {
            System.Random random = new System.Random();

            for (int i = 0; i < cards.Count; i++)
            {
                int j = random.Next(i, cards.Count);
                Card temporary = cards[i];
                cards[i] = cards[j];
                cards[j] = temporary;
            }
        }

        /// <summary>
        /// Broadcasts a deck change
        /// </summary>
        /// <param name="op"></param>
        /// <param name="itemIndex"></param>
        public void OnDeckChanged(SyncListStruct<Card>.Operation op, int itemIndex)
        {
            Debug.Log("OnDeckChanged:" + op);

            // For testing
            /*
            displayText.text = "";
            for(int i=0; i<cards.Count; i++)
            {
                displayText.text += cards[i].name + "\n";
            }
            */
        }

        public void GenerateTest()
        {
            cards.Add(new Card(0, "Colonel Mustard", 0));
            cards.Add(new Card(1, "Ms Scarlet", 0));
            cards.Add(new Card(2, "Professor Plum", 0));
            cards.Add(new Card(3, "Mr Green", 0));
            cards.Add(new Card(4, "Mrs White", 0));
            cards.Add(new Card(5, "Ms Peacock", 0));

            cards.Add(new Card(6, "Lead Pipe", 1));
            cards.Add(new Card(7, "Gun", 1));
            cards.Add(new Card(8, "Knife", 1));
            cards.Add(new Card(9, "Goblet", 1));
            cards.Add(new Card(10, "Rope", 1));
            cards.Add(new Card(11, "Axe", 1));

            cards.Add(new Card(12, "Ball Room", 2));
            cards.Add(new Card(13, "Billiard", 2));
            cards.Add(new Card(14, "Conservatory", 2));
            cards.Add(new Card(15, "Dining Room", 2));
            cards.Add(new Card(16, "Hall", 2));
            cards.Add(new Card(17, "Kitchen", 2));
            cards.Add(new Card(18, "Library", 2));
            cards.Add(new Card(19, "Lounge", 2));
            cards.Add(new Card(20, "Study", 2));
        }
        #endregion Methods
    }
}
