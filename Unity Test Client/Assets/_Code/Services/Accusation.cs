using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accusation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test()
    {
        //Debug.Log("Inside accusation... Test\n");
        makeAccusation("player" + Random.Range(1, 10), "player" + Random.Range(11, 20), "room" + Random.Range(1, 5), "weapon" + Random.Range(55, 99));
    }

    public bool makeAccusation(string accuser, string character, string room, string weapon)
    {
        string msg = accuser + " accuses that " + character + " in room " + room + " has " + weapon;
        Debug.Log(msg);
        Broadcast.Instance.EnqueueMsg("MSG_FROM_ACCCUSATION: " + msg);
        //TODO update database
        return true;
    }
}
