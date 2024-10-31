using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EventHandler eventHandler;
    Player player1;
    Player player2;
    int playerTurn; // 1 or 2 (player 1 or player 2)
    int turnCount;
    int matchCount;

    void Start()
    {
        eventHandler = new EventHandler();
        player1 = new Player();
        player2 = new Player();
        playerTurn = 0;
    }

    public void SelectWhoFirst(int player)
    {
        playerTurn = player;
    }

    public void NewTurn()
    {
        
    }

    public void NewMatch()
    {

    }

    public void WhoWin()
    {

    }
}
