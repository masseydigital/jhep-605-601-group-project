using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcast : MonoBehaviour
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
        Debug.Log("Inside broadcast... Test\n");
    }
                        
    void print(string action, string description)
    {
        Debug.Log("Broadcast: " + action + "action with " + description + " description.\n");
    }
}
