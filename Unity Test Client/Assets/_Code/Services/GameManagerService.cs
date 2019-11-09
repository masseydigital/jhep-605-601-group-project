using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerService : MonoBehaviour
{
    public List<Player> players;
    public Player playerTurn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
