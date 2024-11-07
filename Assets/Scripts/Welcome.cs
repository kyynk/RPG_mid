using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPGBattle
{
    public class Welcome : MonoBehaviour
    {
        public BattleSystem battleSystem;
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

        private string filePath;

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
            hint.SetActive(false);
            whoFirst.SetActive(false);
        }

        public void ShowWhoFirst()
        {
            Debug.Log("Show Who First");
            hint.SetActive(false);
            whoFirst.SetActive(true);
        }

        public void PlayerAFirst()
        {
            selectIconA.SetActive(true);
            selectIconB.SetActive(false);
            WriteSelectionToFile("Player A");
        }

        public void PlayerBFirst()
        {
            selectIconA.SetActive(false);
            selectIconB.SetActive(true);
            WriteSelectionToFile("Player B");
        }
        private void WriteSelectionToFile(string selection)
        {
            try
            {
                File.WriteAllText(filePath, selection); // Write selection to file
                Debug.Log($"Selection '{selection}' written to {filePath}");
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to write to file: {ex.Message}");
            }
        }

        public void RandomFirst()
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                PlayerAFirst();
            }
            else
            {
                PlayerBFirst();
            }

        }

        public void ShowHint()
        {
            Debug.Log("Show Hint");
            hint.SetActive(true);
            whoFirst.SetActive(false);
        }

        public void StartGame()
        {
            Debug.Log("Game Started");
            SceneManager.LoadScene("BattleScene");
        }

        public void ExitGame()
        {
            Debug.Log("Game Exited");
            Application.Quit();
        }
    }
}
