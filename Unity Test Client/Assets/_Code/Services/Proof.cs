using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proof : MonoBehaviour
{
    public string item;
    public string player;

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
        //Debug.Log("Inside proof... Test\n");
        makeProof("player" + Random.Range(0, 100), "randWep" + Random.Range(0, 10));
    }

    public bool makeProof(string player, string item)
    {
        string msg = player + " has " + item;
        Debug.Log(msg);
        Broadcast.Instance.EnqueueMsg("MSG_FROM_PROOF: " + msg);
        //TODO update database
        return true;
    }
}
