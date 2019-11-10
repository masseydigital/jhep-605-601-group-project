using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<string> cards;
    private List<string> characters;

    public void LoadDeck(List<string> characters, List<string> weapons, List<string> rooms)
    {
        cards.AddRange(characters);
        cards.AddRange(weapons);
        cards.AddRange(rooms);
    }

    public CaseFile GetCaseFile(string character, string weapon, string room)
    {
        cards.Remove(character);
        cards.Remove(weapon);
        cards.Remove(room);

        CaseFile file = new CaseFile();
        file.Character = character;
        file.Weapon = weapon;
        file.Room = room; ;

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
