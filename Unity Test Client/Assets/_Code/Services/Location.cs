using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Location : MonoBehaviour
{
    public List<Position> positions = new List<Position>();
    public const int maxRow = 3;
    public const int maxColumn = 3;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool move(int row, int column)
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
    public bool useSecretPassage(Player player)
    {
        Debug.Log("You tried to use a secret passage...\n");
        foreach (Position pos in positions)
        {
            if(pos.player == player)
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
        move(Random.Range(0, maxRow), Random.Range(0, maxColumn));
    }


    public class Position
    {
        public int row;
        public int column;
        public string locationName;
        public Player player;

        /*public int getRow()
        {
            return row;
        }

        public int getColumn()
        {
            return column;
        }

        public string getLocationName()
        {
            return locationName;
        }

        private Player getPlayer()
        {
            return player;
        }

        private void setRow(int newRow)
        {
            row = newRow;
        }

        private void setColumn(int newColumn)
        {
            column = newColumn;
        }

        private void setLocationName(string newLocationName)
        {
            locationName = newLocationName;
        }

        private void setPlayer(Player newPlayer)
        {
            player = newPlayer;
        }*/
    }

}
