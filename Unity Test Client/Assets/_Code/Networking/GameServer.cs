using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GameServer : NetworkManager
{
    public List<NetworkPlayer> players = new List<NetworkPlayer>();
    public int connections = 0;

    public string playerName;
    public GameManagerService gameManager;

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
        gameManager = Instantiate(spawnPrefabs[0], transform.position, transform.rotation).GetComponent<GameManagerService>();

        NetworkServer.Spawn(gameManager.gameObject);
    }

    // When the server adds a player
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        players.Add(player.GetComponent<NetworkPlayer>());

        players[connections].id = connections;

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        gameManager.players.Add(player.GetComponent<NetworkPlayer>());
        gameManager.playerNames.Add(player.GetComponent<NetworkPlayer>().playerName);

        connections++;
    }
}
