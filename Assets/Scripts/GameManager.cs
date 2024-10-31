using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EventHandler eventHandler;
    Player player1;
    Player player2;

    // Start is called before the first frame update
    void Start()
    {
        eventHandler = new EventHandler();
        player1 = new Player();
        player2 = new Player();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
