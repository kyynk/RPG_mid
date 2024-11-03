using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welcome : MonoBehaviour
{
    public void StartGame()
    {
        // will show text "Game Started"
        Debug.Log("Game Started");
    }

    public void Help()
    {
        // will show text "Help"
        Debug.Log("Help");
        // will show text like how to control or play the game
        Debug.Log("How to control or play the game");
    }

    public void ExitGame()
    {
        // will show text "Game Exited"
        Debug.Log("Game Exited");
    }
}
