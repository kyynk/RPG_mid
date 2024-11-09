using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPGBattle
{
    public class Welcome : MonoBehaviour
    {
        public GameObject hint;
        public GameObject whoFirst;
        public Button whoFirstButton;
        public Button hintButton;
        public Button startButton;
        public Button exitButton;
        public Button playerAButton;
        public Button playerBButton;
        public Button randomButton;
        public GameObject selectIconA;
        public GameObject selectIconB;
        public GameObject debugInfo;
        public TMP_Text debugText;

        private string filePath;
        private bool isPlayerSelectionlocked;

        void Start()
        {
            filePath = Path.Combine(Application.dataPath, "ConfigForGame", "who_first.txt"); // Set file path

            whoFirstButton.onClick.AddListener(ShowWhoFirst);
            hintButton.onClick.AddListener(ShowHint);
            startButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(ExitGame);

            playerAButton.onClick.AddListener(PlayerAFirst);
            playerBButton.onClick.AddListener(PlayerBFirst);
            randomButton.onClick.AddListener(RandomFirst);
            PlayerAFirst();
            isPlayerSelectionlocked = true;
            hint.SetActive(false);
            whoFirst.SetActive(false);
            debugInfo.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("w"))
            {
                ShowWhoFirst();
            }
            else if (Input.GetKeyDown("h"))
            {
                ShowHint();
            }
            else if (Input.GetKeyDown("s"))
            {
                StartGame();
            }
            else if (Input.GetKeyDown("e"))
            {
                ExitGame();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (debugInfo.activeSelf)
                {
                    debugInfo.SetActive(false);
                }
                else
                {
                    debugInfo.SetActive(true);
                }
            }
            else if (!isPlayerSelectionlocked)
            {
                if (Input.GetKeyDown("a"))
                {
                    PlayerAFirst();
                }
                else if (Input.GetKeyDown("b"))
                {
                    PlayerBFirst();
                }
                else if (Input.GetKeyDown("r"))
                {
                    RandomFirst();
                }
            }
            UpdateDebugText();
        }

        private void UpdateDebugText()
        {
            debugText.text = "State: Welcome\n" +
                             "Who First:\n" +
                             GetFirstPlayer();
        }

        private string GetFirstPlayer()
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                Debug.LogError("File not found!");
                return "Player A";
            }
        }

        private void ShowWhoFirst()
        {
            Debug.Log("Show Who First");
            hint.SetActive(false);
            whoFirst.SetActive(true);
            isPlayerSelectionlocked = false;
        }

        private void PlayerAFirst()
        {
            selectIconA.SetActive(true);
            selectIconB.SetActive(false);
            WriteSelectionToFile("Player A");
        }

        private void PlayerBFirst()
        {
            selectIconA.SetActive(false);
            selectIconB.SetActive(true);
            WriteSelectionToFile("Player B");
        }

        private void WriteSelectionToFile(string selection)
        {
            try
            {
                File.WriteAllText(filePath, selection);
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to write to file: {ex.Message}");
            }
        }

        private void RandomFirst()
        {
            if (Random.Range(0, 2) == 0)
            {
                PlayerAFirst();
            }
            else
            {
                PlayerBFirst();
            }

        }

        private void ShowHint()
        {
            hint.SetActive(true);
            whoFirst.SetActive(false);
            isPlayerSelectionlocked = true;
        }

        private void StartGame()
        {
            SceneManager.LoadScene("BattleScene");
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}
