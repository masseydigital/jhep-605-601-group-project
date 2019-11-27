using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClueLess
{
    [System.Serializable]
    public struct Player
    {
        public int id;
        public string name;
        public Player(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return id.ToString() + " : " + name;
        }
    }

    // Casting the struct to a useable class
    public class SyncListPlayer : SyncListStruct<Player> { }
}
