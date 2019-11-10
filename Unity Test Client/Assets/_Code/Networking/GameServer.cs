using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GameServer : NetworkManager
{
    public List<NetworkClient> connectedPlayers = new List<NetworkClient>();
    public List<GameObject> players = new List<GameObject>();
    public int connections = 0;

    public string playerName;
    public GameManager gameManager;

    public void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void ServerStart()
    {
        StartServer();
    }

    public void StartGame()
    {

    }

    public void ClientStart()
    {
        NetworkClient client = StartClient();

        connectedPlayers.Add(client);
    }

    // When the server adds a player
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        players.Add(player);

        players[connections].GetComponent<NetworkPlayer>().id = connections;

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        connections++;
    }
}
