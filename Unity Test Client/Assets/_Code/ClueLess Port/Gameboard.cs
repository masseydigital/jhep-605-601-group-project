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
        public GameManager gameManager;

        public override void OnStartServer()
        {
            GenerateTest();
        }

        public void Start()
        {
            rooms.Callback = OnRoomsChanged;
        }

        #region Methods

        // callback syncs this across the network
        public void OnRoomsChanged(SyncListStruct<Room>.Operation op, int itemIndex)
        {
            Debug.Log("OnRoomChanged: " + op);
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
            rooms.Add(new Room(0, "Study", 6, new int[3] { 1, 5, 20}));
            rooms.Add(new Room(1, "Hallway", 1, new int[2] { 0, 2}));
            rooms.Add(new Room(2, "Library", 6, new int[3] {1, 3, 6 }));
            rooms.Add(new Room(3, "Hallway", 1, new int[2] { 2, 4 }));
            rooms.Add(new Room(4, "Conservatory", 6, new int[3] { 3, 7, 16 }));
            rooms.Add(new Room(5, "Hallway", 1, new int[2] { 0, 8}));
            rooms.Add(new Room(6, "Hallway", 1, new int[2] { 2, 10 }));
            rooms.Add(new Room(7, "Hallway", 1, new int[2] { 4, 12 }));
            rooms.Add(new Room(8, "Hall", 6, new int[3] { 5, 9, 13}));
            rooms.Add(new Room(9, "Hallway", 1, new int[2] { 8, 10 }));
            rooms.Add(new Room(10, "Billiard Room", 6, new int[4] { 6, 9, 11, 14 }));
            rooms.Add(new Room(11, "Hallway", 1, new int[2] { 10, 12 }));
            rooms.Add(new Room(12, "Ball Room", 6, new int[3] { 7,11, 15 }));
            rooms.Add(new Room(13, "Hallway", 1, new int[2] { 8, 16 }));
            rooms.Add(new Room(14, "Hallway", 1, new int[2] { 10, 18 }));
            rooms.Add(new Room(15, "Hallway", 1, new int[2] { 12, 20 }));
            rooms.Add(new Room(16, "Lounge", 6, new int[3] { 13, 17, 4 }));
            rooms.Add(new Room(17, "Hallway", 1, new int[2] { 16, 18 }));
            rooms.Add(new Room(18, "Dining Room", 6, new int[3] { 14, 17, 19 }));
            rooms.Add(new Room(19, "Hallway", 1, new int[2] { 18, 20 }));
            rooms.Add(new Room(20, "Kitchen", 6, new int[3] { 15, 19, 0 }));
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
            // check if the move is through an available secret passage
            Debug.Log("Entered move player ... moving from " + from + " to " + to);
            List<int> secretPassages = GetSecretPassages(from);
            foreach (int room in secretPassages)
            {
                if (room == to)
                {
                    Debug.Log("Using secret passage to move from " + from + " to " + to);
                    if (!Move(playerid, from, to))
                    {
                        Debug.Log("Failed to move player...");
                    }
                    return;
                }
            }

            // check if the move is through doorway into a hallway
            // hallways has a maxOccupancy of 1...
            if(rooms[to].maxOccupancy == 1 && rooms[to].occupants[0] == -1)
            {
                Debug.Log("Moving from room " + from + " to hallway " + to);
                if (!Move(playerid, from, to))
                {
                    Debug.Log("Failed to move player...");
                }
                return;
            }

            // if already in a hallway, the move must be to a room...
            if(rooms[from].maxOccupancy == 1  && rooms[to].maxOccupancy > 1)
            {
                Debug.Log("Moving from hallway " + from + " to " + to);
                if (!Move(playerid, from, to))
                {
                    Debug.Log("Failed to move player...");
                }
                return;
            }

            Debug.Log("Failed to move from " + from + " to " + to);

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
            bool success = true;

            if(!gameManager.canMove)
            {
                Debug.Log("Already Moved");
                return false;
            }

            if(!rooms[to].AddPlayer(playerid))
            {
                Debug.Log("Failed to add player to room!");
                success = true;

                if(isLocalPlayer)
                {
                    gameboardUi.networkPlayer.currentRoom = to;
                }
            }

            if(!rooms[from].RemovePlayer(playerid))
            {
                Debug.Log("Failed to remove player from room!");
                success = false;
            }

            if (isLocalPlayer)
            {
                Cmd_MovePlayer(gameboardUi.networkPlayer.playerInfo.id, from, to);
            }

            gameboardUi.UpdateRoomUis(rooms);
            gameboardUi.UpdateRoomImages();
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TryMove(int to)
        {
            bool success = false;

            // If it is not our turn then bounce out 
            if(gameManager.playerTurn != gameboardUi.networkPlayer.playerInfo.id)
            {
                Debug.Log("It isn't your turn!");
                return false;
            }

            // Check if the move is valid
            if (!gameboardUi.roomUis[gameboardUi.networkPlayer.currentRoom].roomData.ChecKValidRoom(to))
            {
                Debug.Log("This is not a valid room!");
                return false;
            }

            // We should also be in the space
            if(!Move(gameboardUi.networkPlayer.playerInfo.id, gameboardUi.networkPlayer.currentRoom, to))
            {
                Debug.Log("Something went wrong with my move!");
            }

            return success;
        }
        #endregion Methods
    }
}
