using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //---------EVENT FUNCTIONS---------

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }


    //---------GAME PROGRESS---------

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}