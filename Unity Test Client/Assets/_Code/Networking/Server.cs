using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.Networking.Types;

public class Server : NetworkManager
{
    public List<NetworkConnection> connectedPlayers = new List<NetworkConnection>();

    public List<TextMeshProUGUI> players_Text;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        // We only want the server to run this
        if (!isServer)
            return;
        */

    }

    // adds a player to the server
    public void AddPlayer()
    {
        for(int i=0; i<players_Text.Count; i++)
        {
            // If we have a connected player
            if(i < connectedPlayers.Count)
            {
                players_Text[i].gameObject.SetActive(true);
                players_Text[i].text = "Player: " +
                    connectedPlayers[i].connectionId;
            }
            else
            {
                players_Text[i].gameObject.SetActive(false);
            }
        }
    }

    //Broadcasts a server to all constituents
    public void BroadcastMessage()
    {

    }

    //When a client connects to our server
    public override void OnServerConnect(NetworkConnection conn)
    {
        connectedPlayers.Add(conn);
        AddPlayer();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        connectedPlayers.Remove(conn);
        AddPlayer();
    }
}
