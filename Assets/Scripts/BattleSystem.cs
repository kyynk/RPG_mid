using TMPro;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGBattle
{
    public class BattleSystem : MonoBehaviour
    {
        // UI references
        public GameObject debugInfo;
        public GameObject playerATurn;
        public GameObject playerBTurn;
        public GameObject turnInfo;
        public GameObject matchInfoPanel; // MatchInfo panel
        public TMP_Text debugText;
        public TMP_Text matchResultText; // Match result text
        public TMP_Text pointResultText; // Point result text
        public ButtonAction buttonAction;

        private EventHandler eventHandler;
        private CoroutineRunner coroutineRunner;
        private Player[] players;
        private int playerTurn; // 0 or 1 (player 1 or player 2)
        private int turnCount;
        private int[] playerPoint;
        private int firstPlayer;
        private string whoFirstFilePath;
        private string whoWinFilePath;

        private void Start()
        {
            GameObject runnerObject = new GameObject("CoroutineRunner");
            coroutineRunner = runnerObject.AddComponent<CoroutineRunner>();
            eventHandler = new EventHandler(new List<string> { "Giant", "Paladin" });
            players = new Player[2];
            players[0] = new Player(new Character("Giant"), "L_HP", "L_Shield", coroutineRunner);
            players[1] = new Player(new Character("Paladin"), "R_HP", "R_Shield", coroutineRunner);
            playerPoint = new int[2] { 0, 0 };
            whoFirstFilePath = Path.Combine(Application.streamingAssetsPath, "who_first.txt");
            whoWinFilePath = Path.Combine(Application.streamingAssetsPath, "who_win.txt");
            debugInfo.SetActive(false);
            InitSomeSettings();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (debugInfo.activeSelf)
                {
                    debugInfo.SetActive(false);
                }
                else
                {
                    debugInfo.SetActive(true);
                    UpdateDebugInfo();
                }
            }
        }

        private void InitSomeSettings()
        {
            turnCount = 1;
            SetFirstPlayer();
            matchInfoPanel.SetActive(false);
            turnInfo.SetActive(true);
            foreach (Player player in players)
            {
                player.ResetStatus();
            }
            UpdateTurnText();
            UpdateDebugInfo();
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
            if (playerTurn == 0)
            {
                playerATurn.SetActive(true);
                playerBTurn.SetActive(false);
            }
            else
            {
                playerATurn.SetActive(false);
                playerBTurn.SetActive(true);
            }
        }

        private void UpdateTurnText()
        {
            TMP_Text turnInfoText = turnInfo.GetComponent<TMP_Text>();
            if (turnInfoText != null)
            {
                turnInfoText.text = "Round " + turnCount;
            }
            else
            {
                Debug.LogError("Text component is missing on TurnText GameObject!");
            }
        }

        private void UpdateDebugInfo()
        {
            debugText.text = "State: " + (playerTurn == 0 ? "Player A" : "Player B") + "\n" +
                             "Player A: \nHP=" + players[0].PlayerCharacter.HP +
                             ", ATK=" + players[0].PlayerCharacter.ATK +
                             ", DEFEND=" + (players[0].PlayerCharacter.IsDefend ? "true" : "false") + "\n" +
                             "Player B: \nHP=" + players[1].PlayerCharacter.HP +
                             ", ATK=" + players[1].PlayerCharacter.ATK +
                             ", DEFEND=" + (players[1].PlayerCharacter.IsDefend ? "true" : "false");
        }

        public IEnumerator NextTurn(string action)
        {
            // every turn need two players to attack each other, so we need to get the current player and the opponent
            Player currentPlayer = players[playerTurn];
            Player opponent = players[(playerTurn + 1) % 2];
            buttonAction.DisableFighterActionButtons();
            yield return HandlePlayerAction(action, currentPlayer, opponent);
            if (firstPlayer != playerTurn)
            {
                turnCount++;
            }
            UpdateDebugInfo(); // update debug info for player property
            if (IsNewMatch())
            {
                NewMatch();
            }
            else
            {
                yield return HandleTurnTransition();
            }
        }

        private IEnumerator HandlePlayerAction(string action, Player currentPlayer, Player opponent)
        {
            if (action == "attack")
            {
                yield return eventHandler.OnPlayerAttack(currentPlayer, opponent);
            }
            else if (action == "defend")
            {
                yield return eventHandler.OnPlayerDefend(currentPlayer);
            }
            else
            {
                Debug.LogError("Invalid action!");
            }
        }

        private bool IsNewMatch()
        {
            // if 10 turn or one player hp <= 0, then the match is over
            return turnCount > 10 || players[0].IsCharacterDead() || players[1].IsCharacterDead();
        }

        private void NewMatch()
        {
            UpdateMatchResult();
            matchInfoPanel.SetActive(true);
            turnInfo.SetActive(false);
            buttonAction.DisableFighterActionButtons();
            buttonAction.EnableContinueButton();
        }

        private void UpdateMatchResult()
        {
            // has results: p1 win, p2 win, draw
            if (players[1].IsCharacterDead())
            {
                matchResultText.text = "Player A Win";
                playerPoint[0]++;
            }
            else if (players[0].IsCharacterDead())
            {
                matchResultText.text = "Player B Win";
                playerPoint[1]++;
            }
            else
            {
                matchResultText.text = "Draw";
            }
            pointResultText.text = playerPoint[0] + " - " + playerPoint[1];
        }

        private IEnumerator HandleTurnTransition()
        {
            UpdateTurnText();
            playerTurn = (playerTurn + 1) % 2;
            UpdateTurnImg();
            UpdateDebugInfo(); // update debug info for state
            yield return TriggerRandomEvent();
            if (IsNewMatch()) // since maybe the random event cause the match over
            {
                NewMatch();
            }
            else
            {
                buttonAction.EnableFighterActionButtons();
            }
        }

        private IEnumerator TriggerRandomEvent()
        {
            Player currentPlayer = players[playerTurn];
            yield return eventHandler.OnPlayerHeal(currentPlayer);
            UpdateDebugInfo(); // update debug info for player property (hp)
            yield return eventHandler.OnPlayerTakeEventDamage(currentPlayer);
            UpdateDebugInfo(); // update debug info for player property (hp)
        }

        public void ContinueToNextMatch()
        {
            buttonAction.EnableFighterActionButtons(); // Re-enable buttons
            buttonAction.DisableContinueButton(); // Hide Continue button
            if (IsGameOver())
            {
                WriteWinnerToFile();
                GoToFinishedScene();
            }
            else
            {
                InitSomeSettings();
            }
        }

        private bool IsGameOver()
        {
            return Math.Abs(playerPoint[0] - playerPoint[1]) == 2;
        }

        private void WriteWinnerToFile()
        {
            try
            {
                string winner = playerPoint[0] > playerPoint[1] ? "Player A" : "Player B";
                File.WriteAllText(whoWinFilePath, winner);
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to write to file: {ex.Message}");
            }
        }

        private void GoToFinishedScene()
        {
            SceneManager.LoadScene("FinishedScene");
        }
    }
}
