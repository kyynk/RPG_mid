using System;
using System.IO;
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
        private string whoFirstFilePath;
        private string whoWinFilePath;

        // UI references
        public GameObject playerATurn;
        public GameObject playerBTurn;
        public GameObject turnInfoText;
        public GameObject matchInfoPanel; // MatchInfo panel
        public TMP_Text matchResultText; // Match result text
        public TMP_Text pointResultText; // Point result text
        public ButtonAction buttonAction;

        private void Start()
        {
            eventHandler = new EventHandler();
            players = new IPlayer[2];
            players[0] = new Player(new Character("Giant"), "L_HP", "L_Shield");
            players[1] = new Player(new Character("Paladin"), "R_HP", "R_Shield");
            playerPoint = new int[2] { 0, 0 };
            whoFirstFilePath = Path.Combine(Application.dataPath, "ConfigForGame", "who_first.txt");
            whoWinFilePath = Path.Combine(Application.dataPath, "ConfigForGame", "who_win.txt");
            InitSomeSettings();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("Tab key pressed!");
            }
        }

        private void InitSomeSettings()
        {
            turnCount = 1;
            SetFirstPlayer();
            matchInfoPanel.SetActive(false);
            turnInfoText.SetActive(true);
            foreach (Player player in players)
            {
                player.ResetStatus();
            }
            UpdateTurnText();
        }

        private void SetFirstPlayer()
        {
            string firstPlayerName = GetFirstPlayer();
            if (firstPlayerName == "Player A")
            {
                firstPlayer = 0;
                playerTurn = 0;
            }
            else
            {
                firstPlayer = 1;
                playerTurn = 1;
            }
            UpdateTurnImg();
        }

        private string GetFirstPlayer()
        {
            if (File.Exists(whoFirstFilePath))
            {
                return File.ReadAllText(whoFirstFilePath);
            }
            else
            {
                Debug.LogError("File not found!");
                return "Player A";
            }
        }

        private void UpdateTurnImg()
        {
            Debug.Log("Player " + playerTurn + " turn!");
            if (playerTurn == 0)
            {
                Debug.Log("Player A turn!");
                playerATurn.SetActive(true);
                playerBTurn.SetActive(false);
            }
            else
            {
                Debug.Log("Player B turn!");
                playerATurn.SetActive(false);
                playerBTurn.SetActive(true);
            }
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
            }
            if (IsNewMatch())
            {
                NewMatch();
            }
            else
            {
                UpdateTurnText();
            }

            playerTurn = (playerTurn + 1) % 2;
            UpdateTurnImg();
        }

        private bool IsNewMatch()
        {
            // if 10 turn or one player hp <= 0, then the match is over
            return turnCount > 10 || players[0].IsCharacterDead() || players[1].IsCharacterDead();
        }

        private void NewMatch()
        {
            WhoWin();
            matchInfoPanel.SetActive(true);
            turnInfoText.SetActive(false);
            buttonAction.DisableFighterActionButtons();
        }

        private void WhoWin()
        {
            // has results: p1 win, p2 win, draw
            if (players[1].IsCharacterDead())
            {
                Debug.Log("Player 0 win!");
                matchResultText.text = "Player A Win";
                playerPoint[0]++;
            }
            else if (players[0].IsCharacterDead())
            {
                Debug.Log("Player 1 win!");
                matchResultText.text = "Player B Win";
                playerPoint[1]++;
            }
            else
            {
                Debug.Log("Draw!");
                matchResultText.text = "Draw";
            }
            pointResultText.text = playerPoint[0] + " - " + playerPoint[1];
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

        private bool IsGameOver()
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

        public void ContinueToNextMatch()
        {
            buttonAction.EnableFighterActionButtons(); // Re-enable buttons
            InitSomeSettings();
        }
    }
}
