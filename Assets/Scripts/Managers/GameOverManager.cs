using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    public static bool gameOver;
    public static int finalScore;

    void Awake()
    {
        DontDestroyOnLoad(this);
        gameOver = false;
        finalScore = 0;
    }

    void LateUpdate()
    {
        if(gameOver)
        {
            finalScore = ScoreManager.score;
            SceneManager.LoadScene("GameMenu");
        }
    }
}
