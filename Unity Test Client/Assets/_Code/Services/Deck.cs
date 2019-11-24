using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Deck
{
    private List<string> cards = new List<string>();

    public List<string> Cards
    {
        get { return cards; }
        set { cards = value; }
    }

    public void LoadDeck(List<string> characters, List<string> weapons, List<string> rooms)
    {
        cards.AddRange(characters);
        cards.AddRange(weapons);
        cards.AddRange(rooms);
    }

    public CaseData GetCaseFile(string character, string weapon, string room)
    {
        cards.Remove(character);
        cards.Remove(weapon);
        cards.Remove(room);

        CaseData file = new CaseData();
        file.character = character;
        file.weapon = weapon;
        file.room = room;

        return file;
    }

    public List<string> DealRandom(int num)
    {
        List<string> returnCards = new List<string>();
        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, cards.Count);
            returnCards.Add(cards[index]);
            cards.RemoveAt(index);
        }

        return returnCards;
    }

    /// <summary>
    /// Deals a random set of cards passing
    /// overloaded to pass in a num and deck of cards
    /// </summary>
    /// <param name="num"></param>
    /// <param name="deckCards"></param>
    /// <returns></returns>
    public List<string> DealRandom(int num, ref SyncListString deckCards)
    {
        List<string> returnCards = new List<string>();

        if (deckCards.Count == 0)
        {
            Debug.Log("Deck.DealRandom: The deck is empty!");
            return null;
        }
        if (deckCards.Count < num)
        {
            Debug.Log("Deck.DealRandom: Requested to deal more cards than exists in the deck!");
            return null;
        }

        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, deckCards.Count - 1);

            returnCards.Add(deckCards[index]);

            deckCards.Remove(deckCards[index]);
        }

        return returnCards;
    }

    /// <summary>
    /// Deals a single card from the deck
    /// </summary>
    /// <returns></returns>
    public string DealCard()
    {
        string retCard;

        int index = Random.Range(0, cards.Count);

        retCard = cards[index];

        //Remove the card from the deck
        cards.Remove(retCard);

        return retCard;
    }

    /// <summary>
    /// Deals a single card from the deck
    /// overloaded to pass in a deck
    /// </summary>
    /// <returns></returns>
    public string DealCard(List<string> deckCards)
    {
        string retCard;

        int index = Random.Range(0, deckCards.Count);

        retCard = deckCards[index];

        //Remove the card from the deck
        cards.Remove(retCard);

        return retCard;
    }
}
