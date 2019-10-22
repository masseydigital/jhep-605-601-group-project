using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcast : MonoBehaviour
{
    private static Queue<string> msgQueue = new Queue<string>();
    private static Broadcast instance;

    public static Broadcast Instance
    {
        get { return instance ?? (instance = new GameObject("Broadcast").AddComponent<Broadcast>()); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Broadcast.msgQueue.Count > 0)
        {
            DequeueMsg();
        }
    }

    public void DequeueMsg()
    {
        string msg = Broadcast.msgQueue.Dequeue();
        Debug.Log("BROADCAST: Received msg: " + msg);
    }

    public void EnqueueMsg(string msg)
    {
        Broadcast.msgQueue.Enqueue(msg);
    }

    public void test()
    {
        Debug.Log("Inside broadcast... Test\n");
    }
                        
    void print(string action, string description)
    {
        Debug.Log("Broadcast: " + action + "action with " + description + " description.\n");
    }
}
