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

        #region Methods

        // callback syncs this across the network???
        public void OnRoomsChanged(SyncListStruct<Room>.Operation op, int itemIndex)
        {
            Debug.Log("OnRoomChanged: " + op);
            // TODO should we update the room images here?
            gameboardUi.UpdateRoomImages();
        }

        public void InitializeBoard()
        {
            GenerateTest();
        }

        // Returns a list of available secret passages from room: roomid
        // it's up to the caller to determine if the room is full before moving...
        public List<int> GetSecretPassages(int roomid)
        {
            List<int> secretPassages = new List<int>();
            switch(roomid)
            {
                // study --> billard room
                case 0:
                    secretPassages.Add(10);
                    break;
                // hallways
                case 1: 
                case 2:
                case 3:
                    break;
                // conservatory --> billard room
                case 4:
                    secretPassages.Add(10);
                    break;
                case 5:
                case 6:
                case 7:
                    break;
                // hall --> library and dining room
                case 8:
                    secretPassages.Add(2);
                    secretPassages.Add(18);
                    break;
                case 9:
                    break;
                // billard room --> study, conservatory, kitchen, lounge
                case 10:
                    secretPassages.Add(0);
                    secretPassages.Add(4);
                    secretPassages.Add(19);
                    secretPassages.Add(16);
                    break;
                case 11:
                    break;
                // ballroom --> library, dining room
                case 12:
                    secretPassages.Add(2);
                    secretPassages.Add(18);
                    break;
                case 13:
                case 14:
                case 15:
                    break;
                // lounge --> billard room
                case 16:
                    secretPassages.Add(10);
                    break;
                case 17:
                    break;
                // dining room --> hall, ballroom
                case 18:
                    secretPassages.Add(8);
                    secretPassages.Add(12);
                    break;
                case 19:
                    break;
                // kitchen --> billard room
                case 20:
                    secretPassages.Add(10);
                    break;
                default:
                    break;
            }   

            return secretPassages;
            
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

        /*
         *
         * Your options of moving are limited to the following:
            o If you are in a room, you may do one of the following:
                − Move through one of the doors to the hallway (if it is not blocked).

                − Take a secret passage to a diagonally opposite room (if there is one) and make a
                suggestion.

                − If you were moved to the room by another player making a suggestion, you may, if
                you wish, stay in that room and make a suggestion. Otherwise you may move
                through a doorway or take a secret passage as described above.

            o If you are in a hallway, you must do the following:
                − Move to one of the two rooms accessible from that hallway and make a suggestion

            If all of the exits are blocked (i.e., there are people in all of the hallways) and you are not in
            one of the corner rooms (with a secret passage), and you weren’t moved to the room by
            another player making a suggestion, you lose your turn (except for maybe making an
            accusation)

            Your first move must be to the hallway that is adjacent to your home square. The inactive
            characters stay in their home squares until they are moved to a room by someone making a
            suggestion. 
         *
         */

        // commands come from the client, so as the server we need to determine if the move is legit
        // syncvars will automatically sync (so the SyncListRoom should update whenever we change it)
        [Command]
        public void Cmd_MovePlayer(int playerid, int from, int to)
        {
            if(!Move(playerid, from, to))
            {
                Debug.Log("Failed to move player...");
            }

        }

        /// <summary>
        /// Moves player
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool Move(int playerid, int from, int to)
        {
            // TODO add more of the move logic...

            if(!rooms[to].AddPlayer(playerid))
            {
                Debug.Log("Failed to add player to room!");
                return false;
            }

            if(!rooms[from].RemovePlayer(playerid))
            {
                Debug.Log("Failed to remove player from room!");
                return false;
            }

            return true;
        }

        #endregion Methods
    }
}
