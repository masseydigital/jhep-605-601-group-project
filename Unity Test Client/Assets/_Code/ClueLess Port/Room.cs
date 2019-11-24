using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClueLess
{
    [System.Serializable]
    public struct Room
    {
        public int id;
        public string name;
        public int maxOccupancy;
        public int[] occupants;

        public Room(int id, string name, int maxOccupancy)
        {
            this.id = id;
            this.name = name;
            this.maxOccupancy = maxOccupancy;

            this.occupants = new int[maxOccupancy];
            for(int i=0; i<occupants.Length; i++)
            {
                occupants[i] = -1;
            }
        }

        public override string ToString()
        {
            return id.ToString() + " : " + name + " : " + occupants.Length + " : " + Occupancy(); 
        }

        // Adds a player to the room
        public bool addPlayer(int id)
        {
            int i = 0;
            for(; i < occupants.Length; i++)
            {
                if(occupants[i] == -1)
                {
                    occupants[i] = id;
                    return true;
                }
            }
            return false;
        }

        // Remove a player to the room
        public bool removePlayer(int id)
        {
            for(int i = 0; i < occupants.Length; i++)
            {
                if(occupants[i] == id)
                {
                    occupants[i] = -1;
                    return true;
                }
            }

            return false;
        }


        public int Occupancy()
        {
            int num = 0;

            for(int i=0; i<occupants.Length; i++)
            {
                if(occupants[i] != -1)
                {
                    num++;
                }
            }

            return num;
        }
    }

    public class SyncListRoom : SyncListStruct<Room> { }
}
