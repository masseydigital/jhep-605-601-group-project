using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.Networking.Types;

using TMPro;
using System.Text;

public class Player : NetworkBehaviour
{
    Broadcast broadcast;
    [SyncVar(hook = "OnPlayerNameChanged")] public string playerName;
    public int winLose;
    private List<string> characters;
    private List<string> locations;
    private List<string> weapons;

    public TMP_InputField msg_input;
    public TMP_InputField playerName_input;
    public TextMeshProUGUI playerName_Tmpro;
    
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
        if (isClient)
        {
            Debug.Log($":: I AM CLIENT: {GetComponent<NetworkIdentity>().playerControllerId} ::");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // only allow clients to execute
        if (!isLocalPlayer)
            return;
    }
    
    // Changes the player name
    void OnPlayerNameChanged(string newName)
    {
        Debug.Log($"OnPlayerNameChanged:: Old Name: {playerName} New Name: {newName}");

        playerName = newName;

        gameObject.name = $"{newName}";
    }

    [Command] 
    void Cmd_ChangePlayerName(string n)
    {
        Debug.Log($"Cmd_ChangePlayerName {n}");

        playerName = n;

        playerName_Tmpro.text = playerName;
    }

    //Changes the player name when the update button is pressed
    public void ChangePlayerName()
    {
        Cmd_ChangePlayerName(playerName_input.text);
    }

    public void Send()
    {
        Message(msg_input.text);
    }

    public void Message(string msg)
    {
        string fullMessage = $":: {playerName}: {msg} ::";
        Debug.Log(fullMessage);

        NetMessage netmsg = new NetMessage();

        netmsg.msg = Encoding.ASCII.GetBytes(fullMessage);

        NetworkServer.SendToAll(1000, netmsg);
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

public class NetMessage : MessageBase
{
    public byte[] msg;
}

