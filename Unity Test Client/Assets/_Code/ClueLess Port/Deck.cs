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

namespace ClueLess
{
    public class Deck : NetworkBehaviour
    {
        // Current cards in the deck
        public SyncListCard cards = new SyncListCard();  

        // All original Cards
        public SyncListCard allCards = new SyncListCard();

        public override void OnStartServer()
        {
            GenerateTest();

            CreateDeck(cards);
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
        public void CreateDeck(SyncListCard cards)
        {
            for(int i=0; i<cards.Count; i++)
            {
                allCards.Add(new Card(i, cards[i].name, cards[i].category));
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
        /// Gets a specific card
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Card GetCard(int category)
        {
            Card c;

            for(int i=0; i<cards.Count; i++)
            {
                if(cards[i].category == category)
                {
                    c = cards[i];
                    cards.Remove(c);
                    return c;
                }
            }

            // We don't have any of this type of card :(
            return new Card(-1, "NULL", -1);
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
        }

        public void GenerateTest()
        {
            cards.Add(new Card(0, "Col Mustard", 0));
            cards.Add(new Card(1, "Miss Scarlet", 0));
            cards.Add(new Card(2, "Mr Green", 0));
            cards.Add(new Card(3, "Mrs Peacock", 0));
            cards.Add(new Card(4, "Mrs White", 0));
            cards.Add(new Card(5, "Prof Plum", 0));

            cards.Add(new Card(6, "axe", 1));
            cards.Add(new Card(7, "goblet", 1));
            cards.Add(new Card(8, "knife", 1));
            cards.Add(new Card(9, "lead pipe", 1));
            cards.Add(new Card(10, "pistol", 1));
            cards.Add(new Card(11, "rope", 1));

            cards.Add(new Card(12, "Ball Room", 2));
            cards.Add(new Card(13, "Billiard Room", 2));
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
