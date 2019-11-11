using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The data associated with each room
[System.Serializable]
public struct RoomData
{
    public int roomNum;
    public string roomName;
    public List<int> occupants;     // list of occupants... for hallways this is 1
    public int maxOccupancy;
}

// The attached instance of a Room
public class Room : MonoBehaviour
{
    public RoomData roomData;
}
