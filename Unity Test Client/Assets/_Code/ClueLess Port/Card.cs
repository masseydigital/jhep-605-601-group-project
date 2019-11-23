/* Card.cs
 * Brief:  This is a single card
 * 
 * -- Ids --
 * 0 : Character 1          6 : Weapon 1        12 : Room 1     
 * 1 : Character 2          7 : Weapon 2        13 : Room 2  
 * 2 : Character 3          8 : Weapon 3        14 : Room 3  
 * 3 : Character 4          9 : Weapon 4        15 : Room 4  
 * 4 : Character 5          10 : Weapon 5       16 : Room 5  
 * 5 : Character 6          11 : Weapon 6       17 : Room 6  
 *                                              18 : Room 7  
 *                                              19 : Room 8  
 *                                              20 : Room 9  
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Clueless
{
    [System.Serializable]
    public struct Card
    {
        public int id;              // Id in the order.. 0 = Character 1, 1 = Character 2
        public string name;         // Name of card, i.e. Colonel Mustard
        public int category;        // 0 is character, 1 is weapon, 2 is room

        // Create a new card
        public Card(int id, string name, int category)
        {
            this.id = id;
            this.name = name;
            this.category = category;
        }

        // Returns the full identifier when returned, i.e. 0 : 0 : Colonel Mustard
        public override string ToString()
        {
            return id.ToString() + " : " + category.ToString() + " : " + name;
        }
    }

    // Casting the struct to a useable class
    public class SyncListCard : SyncListStruct<Card>
    {

    }
}


