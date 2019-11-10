using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Location : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public List<Hallway> hallways = new List<Hallway>();
    public const int maxRow = GameDefines.MAX_GAMEBOARD_ROWS;
    public const int maxColumn = GameDefines.MAX_GAMEBOARD_COLS;

    //TODO: FIX THIS
    int row;
    int column;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool move(Player player, Room destinationRoom, Hallway destinationHallway)
    {
        string msg = "Tried to move to row:" + row.ToString() + " column:" + column.ToString();
        Debug.Log(msg);
        // TODO: Logic that updates location

        // passing the message up to broadcast...
        //broadcast = GameObject.Find("Broadcast");
        Broadcast.Instance.EnqueueMsg("MSG_FROM_LOCATION: "+msg);
        return true;
    }

    // Returns True if successfully used secret passage; false otherwise
    public bool useSecretPassage(Player player, Room destinationRoom)
    {
        Debug.Log("You tried to use a secret passage...\n");
        //foreach (Position pos in positions)
        {
            //if(pos.player == player)
            {
                // TODO: check diagonals and see if there's an open position
            }
        }

        Broadcast.Instance.EnqueueMsg("MSG_FROM_LOCATION: " + "player tried to use a secret passage!\n");
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
