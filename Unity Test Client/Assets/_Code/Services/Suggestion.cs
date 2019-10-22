using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suggestion : MonoBehaviour
{

    string character = "Miss Scarlet";
    string location = "Kitchen";
    string weapon = "Knife";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool makeSuggestion(string character, string weapon, string location)
    {
        string msg = "A suggestion has been made for " + character + " in the " + location + " with the " + weapon + ".";
        Debug.Log(msg);
        Broadcast.Instance.EnqueueMsg("MSG_FROM_SUGGESTION: " + msg);
        return true;
    }

    public void test()
    {
        //Debug.Log("A suggestion has been made... Test\n");
        makeSuggestion(character, weapon, location);
    }
}
