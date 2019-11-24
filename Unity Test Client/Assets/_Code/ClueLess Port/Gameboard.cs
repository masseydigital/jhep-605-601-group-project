using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// This is all of the board rooms and hallways
// This is where players can move their icon too
// 0 - Study
// 1 - Hallway
// 2 - Library
// 3 - Hallway
// 4 - Conservatory
// 5 - Hallway
// 6 - Hallway
// 7 - Hallway
// 8 - Hall
// 9 - Hallway 
// 10 - Billard Room
// 11 - Hallway
// 12 - Ball Rooom
// 13 - Hallway
// 14 - Hallway 
// 15 - Hallway 
// 16 - Lounge
// 17 - Hallway
// 18 - Dining Room
// 19 - Hallway
// 20 - Kitchen

// -1 =  unoccupied
// 0-5 = playerId

namespace ClueLess
{
    public class Gameboard : NetworkBehaviour
    {
        public SyncListRoom rooms = new SyncListRoom();

        public GameboardUi gameboardUi;

        public override void OnStartServer()
        {
            GenerateTest();
        }

        public void Start()
        {
            rooms.Callback = OnRoomsChanged;
        }

        public void OnRoomsChanged(SyncListStruct<Room>.Operation op, int itemIndex)
        {
            Debug.Log("OnRoomChanged: " + op);
        }

        public void InitializeBoard()
        {
            GenerateTest();
        }

        /// <summary>
        /// Test set of rooms
        /// </summary>
        public void GenerateTest()
        {
            rooms.Add(new Room(0, "Study", 6));
            rooms.Add(new Room(1, "Hallway", 1));
            rooms.Add(new Room(2, "Library", 6));
            rooms.Add(new Room(3, "Hallway", 1));
            rooms.Add(new Room(4, "Conservatory", 6));
            rooms.Add(new Room(5, "Hallway", 1));
            rooms.Add(new Room(6, "Hallway", 1));
            rooms.Add(new Room(7, "Hallway", 1));
            rooms.Add(new Room(8, "Hall", 6));
            rooms.Add(new Room(9, "Hallway", 1));
            rooms.Add(new Room(10, "Billiard Room", 6));
            rooms.Add(new Room(11, "Hallway", 1));
            rooms.Add(new Room(12, "Ball Room", 6));
            rooms.Add(new Room(13, "Hallway", 1));
            rooms.Add(new Room(14, "Hallway", 1));
            rooms.Add(new Room(15, "Hallway", 1));
            rooms.Add(new Room(16, "Lounge", 6));
            rooms.Add(new Room(17, "Hallway", 1));
            rooms.Add(new Room(18, "Dining Room", 6));
            rooms.Add(new Room(19, "Hallway", 1));
            rooms.Add(new Room(20, "Kitchen", 6));
        }

        public void move(int playerid, int from, int to)
        {
            if(!rooms[to].addPlayer(playerid))
            {
                Debug.Log("Failed to add player to room!");
                return;
            }

            if(!rooms[from].removePlayer(playerid))
            {
                Debug.Log("Failed to remove player from room!");
            }
        }
    }
}
