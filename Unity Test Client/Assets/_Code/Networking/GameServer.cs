using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

using ClueLess;

public class GameServer : NetworkManager
{
    public List<NetPlayer> players = new List<NetPlayer>();
    public int connections = 0;

    public string playerName;
    public ClueLess.GameManager gameManager;

    public void ServerStart()
    {
        StartServer();
    }

    public void ClientStart()
    {
        NetworkClient client = StartClient();
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        // The is the official game manager
        //gameManager = Instantiate(spawnPrefabs[0], transform.position, transform.rotation).GetComponent<GameManagerService>();

        //NetworkServer.Spawn(gameManager.gameObject);
    }

    // When the server adds a player
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        players.Add(player.GetComponent<NetPlayer>());

        players[connections].playerInfo.id = connections;

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        connections++;
    }
}
