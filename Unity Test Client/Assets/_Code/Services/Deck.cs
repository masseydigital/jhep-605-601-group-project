using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
