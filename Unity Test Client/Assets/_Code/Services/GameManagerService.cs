using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerService : MonoBehaviour
{
    private List<Player> players = new List<Player>();
    private Player playerTurn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool addPlayer(Player player)
    {
        if (players.Count <= GameDefines.PARTY_SIZE_LIMIT)
        {
            // TODO: Alert others that a player has joined the game

            players.Add(player);

            return true;
        }
        else
        {
            // TODO: Alert player that party size limit has been reached
            return false;
        }
    }

    public bool removePlayer(Player player)
    {
        //if (players.Exists(player))
        if(player != null)
        {
            if (playerTurn == player)
            {
                // Go to next player turn
                incrementTurn();
            }

            // TODO: Alert other players that someone is leaving

            players.Remove(player);

            return true;
        }
        else
        {
            // Requested player not in game
            return false;
        }
    }

    public int getNumPlayers()
    {
        return players.Count;
    }

    public bool setPlayerTurn(Player player)
    {
        //if (players.Exists(player))
        if(player != null)
        {
            playerTurn = player;

            // TODO: Alert players of change in turn

            return true;
        }
        else
        {
            // The player is not in the game
            return false;
        }
    }

    public bool incrementTurn()
    {
        int playerTurnIndex = 0;
        int playerCount = getNumPlayers();;
        
        if (playerCount == 1)
        {
            // Only one player in game so make it their turn
            playerTurn = players[0];

            // TODO: Alert players of change in turn

            return true;
        }
        else if (playerCount <= 0)
        {
            return false;
        }
        else
        {
            //if (players.Exists(playerTurn))
            if(playerTurn != null)
            {
                playerTurnIndex = players.IndexOf(playerTurn);

                // Set player turn to next player in players list
                playerTurn = players[(playerTurnIndex + 1) % playerCount];

                // TODO: Alert players of change in turn

                return true;
            }
            else
            {

            }
        }

        return false;
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

    public Player getTurn()
    {
        return playerTurn;
    }

    public bool checkWinner(Player player)
    {
        if (player.winLoose == 1) return true;
        else return false;
    }

    public bool checkLoser(Player player)
    {
        if (player.winLoose != 1) return true;
        else return false;
    }
}
