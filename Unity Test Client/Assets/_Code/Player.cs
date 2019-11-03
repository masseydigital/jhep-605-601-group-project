using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.Networking.Types;

public class Player : NetworkBehaviour
{
    Broadcast broadcast;
    public string playerName;
    public int winLose;
    private List<string> characters;
    private List<string> locations;
    private List<string> weapons;

    public Player()
    {
        playerName = "test name";
        characters = new List<string>();
        locations = new List<string>();
        weapons = new List<string>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            Debug.Log(":: I AM SERVER ::");
        }

        if (isClient)
        {
            Debug.Log($":: I AM CLIENT: {GetComponent<NetworkIdentity>().playerControllerId} ::");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test()
    {
        Debug.Log("Player service: test\n");

        Broadcast.Instance.EnqueueMsg("broadcast_win:really long string that is very long " + playerName);
        Broadcast.Instance.EnqueueMsg("broadcast_lose: " + playerName);

        Debug.Log("Player service: making suggestion");
        Suggestion suggestion = gameObject.AddComponent<Suggestion>();
        suggestion.makeSuggestion("Col. Mustard", "Dining Room", "Lead pipe");

        Debug.Log("Player service: making proof");
        Proof proof = gameObject.AddComponent<Proof>();
        proof.makeProof(playerName, "Lead pipe");

        Debug.Log("Player service: making accusation");
        Accusation acc = gameObject.AddComponent<Accusation>();
        acc.makeAccusation(playerName, "Col. Mustard", "Dining Room", "Lead pipe");
    }
}
