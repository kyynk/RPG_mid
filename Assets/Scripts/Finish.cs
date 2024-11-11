using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public Button menuButton;
    public TMP_Text resultText;
    public GameObject debugInfo;
    public TMP_Text debugText;

    private string whoWinFilePath;

    // Start is called before the first frame update
    void Start()
    {
        whoWinFilePath = Path.Combine(Application.streamingAssetsPath, "who_win.txt");
        Debug.Log("whoWinFilePath: " + whoWinFilePath);
        menuButton.onClick.AddListener(BackToMenu);
        debugInfo.SetActive(false);
        
        resultText.text = GetWhoWin() + "!!!";
        debugText.text = "State: Finished\n" +
                         "Winner:" + GetWhoWin();
    }

    // Update is called once per frame
    void Update()
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
            }
        }
        else if (Input.GetKeyDown("m"))
        {
            BackToMenu();
        }
    }

    private string GetWhoWin()
    {
        if (!File.Exists(whoWinFilePath))
        {
            return "File Not Found";
        }
        return File.ReadAllText(whoWinFilePath);
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("WelcomeScene");
    }
}
