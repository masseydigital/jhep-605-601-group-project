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
            return id.ToString() + " : " + name + " : " + occupants.Length + " : " + Occupancy() + " : " + occupants[0]; 
        }

        // Adds a player to the room
        public bool AddPlayer(int id)
        {
            //Make sure we are not already in the room
            for (int j = 0; j < occupants.Length; j++)
            {
                if (occupants[j] == id)
                {
                    Debug.Log("Already in the room!");
                    return false;
                }
            }

            // Check to see if there is an open room
            for (int i = 0; i < occupants.Length; i++)
            {
                if(occupants[i] == -1)
                {
                    Debug.Log($"Adding player: {id} to room.");
                    occupants[i] = id;
                    return true;
                }
            }
            return false;
        }

        // Remove a player to the room
        public bool RemovePlayer(int id)
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
