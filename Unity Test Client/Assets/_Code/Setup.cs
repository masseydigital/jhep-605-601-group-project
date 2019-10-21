using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Setup : NetworkBehaviour
{
    public string ipAddress = "127.0.0.1";
    public int port = 4444;
    short messageID = 1000;
    public bool isAtStartup = true;

    public List<NetworkClient> clients = new List<NetworkClient>();

    // Start is called before the first frame update
    void Start()
    {
        SetupServer();

        AddClient();

        AddClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupServer()
    {
        NetworkServer.Listen(port);
        isAtStartup = false;
    }

    public void AddClient()
    {
        NetworkClient newClient = new NetworkClient();
        newClient.RegisterHandler(MsgType.Connect, OnConnected);
        newClient.Connect(ipAddress, port);
        clients.Add(newClient);
        isAtStartup = false;
    }

    public void SetupLocalClient()
    {
        NetworkClient newClient = ClientScene.ConnectLocalServer();
        newClient.RegisterHandler(MsgType.Connect, OnConnected);
        isAtStartup = false;
    }
   
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to Server");
    }
}
