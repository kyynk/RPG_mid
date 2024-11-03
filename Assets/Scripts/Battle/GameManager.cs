using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleState
{
    public class GameManager : MonoBehaviour
    {
        private EventHandler eventHandler;
        private Player[] players;
        private int playerTurn; // 1 or 2 (player 1 or player 2)
        private int turnCount;
        private int matchCount;

        void Start()
        {
            eventHandler = new EventHandler();
            players = new Player[2];
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
}
