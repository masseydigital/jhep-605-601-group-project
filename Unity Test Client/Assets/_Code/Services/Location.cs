﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Location : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public List<Hallway> hallways = new List<Hallway>();
    public const int maxRow = GameDefines.MAX_GAMEBOARD_ROWS;
    public const int maxColumn = GameDefines.MAX_GAMEBOARD_COLS;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Room getRoom(int row, int column)
    {
        if (row < 0 || column < 0 || row >= maxRow || column >= maxColumn)
        {
            Debug.Log("getRoom: Invalid parameter(s)");
            return null;
        }

        foreach (Room room in rooms)
        {
            if (room.row == row && room.column == column)
            {
                return room;
            }
        }
        return null;
    }

    public Hallway getHallway(int row, int column)
    {
        if (row < 0 || column < 0 || row >= maxRow || column >= maxColumn)
        {
            Debug.Log("getHallway: Invalid parameter(s)");
            return null;
        }

        foreach (Hallway hallway in hallways)
        {
            if (hallway.row == row && hallway.column == column)
            {
                return hallway;
            }
        }

        return null;
    }

    public bool move(Player player, Room destinationRoom, Hallway destinationHallway)
    {
        string msg = "Tried to move to room" + destinationRoom.roomName + " through hallway" + destinationHallway.row + ":"+ destinationHallway.column;
        Debug.Log(msg);
        Broadcast.Instance.EnqueueMsg("MSG_FROM_LOCATION: "+msg);

        // Go through hallway to get to room

        return true;
    }

    // Returns True if successfully used secret passage; false otherwise
    public bool showSecretPassages(Player player)
    {
        Debug.Log("You tried to use a secret passage...\n");
        Broadcast.Instance.EnqueueMsg("MSG_FROM_LOCATION: " + "player tried to use a secret passage!\n");

        int startRow = 0, startColumn = 0;
        // available room that can be used for passage
        int secretRow = 0, secretColumn = 0;
        Room currentRoom = null;

        // find where the player is located for our starting point
        foreach (Hallway hallway in hallways)
        {
            if(hallway.player == player)
            {
                startRow = hallway.row;
                startColumn = hallway.column;
            }
        }

        foreach (Room room in rooms)
        {
            if(room.player == player)
            {
                startRow = room.row;
                startColumn = room.column;
            }
        }

        // check diagonals for an available empty room
        if (((currentRoom = getRoom(startRow + 1, startColumn - 1)) != null) && currentRoom.player == null)
        {
            Debug.Log("showSecret: found a secret passage to top-left room");
        }
        else if(((currentRoom = getRoom(startRow + 1, startColumn + 1)) != null) && currentRoom.player == null)
        {
            Debug.Log("showSecret: found a secret passage to top-right room");
        }
        else if (((currentRoom = getRoom(startRow - 1, startColumn - 1)) != null) && currentRoom.player == null)
        {
            Debug.Log("showSecret: found a secret passage to bottom-left room");
        }
        else if (((currentRoom = getRoom(startRow - 1, startColumn + 1)) != null) && currentRoom.player == null)
        {
            Debug.Log("showSecret: found a secret passage to bottom-right room");
        }
        else
        {
            return false;
        }

        secretRow = currentRoom.row;
        secretColumn = currentRoom.column;

        // Illuminate room

        return true;
    }

    public void test()
    {
        //Debug.Log("Inside location... Test ... calling move\n");
        //Player player = new Player();
        //player.name = "testPlayer" + Random.Range(0, 100).ToString();
        //move(Random.Range(0, maxRow), Random.Range(0, maxColumn));
    }

}
