using System;
using TMPro;
using UnityEngine;

namespace RPGBattle
{
    public class BattleSystem : MonoBehaviour
    {
        private EventHandler eventHandler;
        private IPlayer[] players;
        private int playerTurn; // 0 or 1 (player 1 or player 2)
        private int turnCount;
        private int[] playerPoint;
        private int firstPlayer;

        void Start()
        {
            eventHandler = new EventHandler();
            players = new IPlayer[2];
            players[0] = new Player(new Character(), "L_HP");
            players[1] = new Player(new Character(), "R_HP");
            playerTurn = 0;
            turnCount = 1;
            playerPoint = new int[2] { 0, 0 };
            firstPlayer = 0;
            UpdateTurnText();
        }

        public void SelectWhoFirst(int player)
        {
            firstPlayer = player;
            playerTurn = player;
        }

        public void NextTurn(string action)
        {
            // every turn need two players to attack each other, so we need to get the current player and the opponent
            IPlayer currentPlayer = players[playerTurn];
            IPlayer opponent = players[(playerTurn + 1) % 2];

            if (action == "attack")
            {
                Debug.Log("Player " + playerTurn + " attack!");
                eventHandler.OnPlayerAttack(currentPlayer, opponent);
            }
            else if (action == "defence")
            {
                Debug.Log("Player " + playerTurn + " defence!");
                eventHandler.OnPlayerDefend(currentPlayer);
            }
            else
            {
                Debug.Log("Invalid action!");
            }

            Debug.Log("Player 0" + " HP: " + players[0].Character.HP);
            Debug.Log("Player 1" + " HP: " + players[1].Character.HP);

            if (firstPlayer != playerTurn)
            {
                turnCount++;
                UpdateTurnText();
            }
            if (IsNewMatch())
            {
                NewMatch();
            }

            playerTurn = (playerTurn + 1) % 2;
        }

        public bool IsNewMatch()
        {
            // if 10 turn or one player hp <= 0, then the match is over
            return turnCount > 10 || players[0].IsCharacterDead() || players[1].IsCharacterDead();
        }

        public void NewMatch()
        {
            WhoWin();
            turnCount = 0;
            foreach (Player player in players)
            {
                player.ResetStatus();
            }
        }

        public void WhoWin()
        {
            // has results: p1 win, p2 win, draw
            if (players[1].IsCharacterDead())
            {
                Debug.Log("Player 0 win!");
                playerPoint[0]++;
            }
            else if (players[0].IsCharacterDead())
            {
                Debug.Log("Player 1 win!");
                playerPoint[1]++;
            }
            else
            {
                Debug.Log("Draw!");
            }
            if (IsGameOver())
            {
                Debug.Log("Game Over!");
                if (playerPoint[0] > playerPoint[1])
                {
                    Debug.Log("Player 0 win the game!");
                }
                else
                {
                    Debug.Log("Player 1 win the game!");

                }
            }
        }

        public bool IsGameOver()
        {
            return Math.Abs(playerPoint[0] - playerPoint[1]) == 2;
        }

        private void UpdateTurnText()
        {
            GameObject turnInfoText = GameObject.FindGameObjectWithTag("TurnInfo");
            if (turnInfoText != null)
            {
                TMP_Text textComponent = turnInfoText.GetComponent<TMP_Text>();
                if (textComponent != null)
                {
                    textComponent.text = "Round " + turnCount;
                }
                else
                {
                    Debug.LogError("Text component is missing on TurnText GameObject!");
                }
            }
            else
            {
                Debug.LogError("TurnText UI element is not assigned in the Inspector!");
            }
        }
    }
}
