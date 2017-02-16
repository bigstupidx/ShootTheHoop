using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject loggedInPanel;
    public GameObject loggedOutPanel;
    public GameObject controlsPanel;
    public GameObject gameModesPanel;
    public GameObject leaderboardPanel;
    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
    public Text finalScoreText;

    public void LoadMode(string mode)
    {
        if(mode.Equals("normal"))
        {
            SceneManager.LoadSceneAsync("NormalMode");
        }
        else
        {
            SceneManager.LoadSceneAsync("TimeMode");
        }
    }

    public void ShowLoggedInPanel(bool show)
    {
        loggedInPanel.SetActive(show);
    }

    public void ShowLoggedOutPanel(bool show)
    {
        loggedOutPanel.SetActive(show);
    }

    public void ShowGameModesMenu(bool show)
    {
        gameModesPanel.SetActive(show);
        mainMenuPanel.SetActive(!show);
    }

    public void ShowControlsMenu(bool show)
    {
        controlsPanel.SetActive(show);
        mainMenuPanel.SetActive(!show);
    }

    public void ShowLeaderboardMenu(bool show)
    {
        leaderboardPanel.SetActive(show);
        mainMenuPanel.SetActive(!show);
    }

    public void ShowGameOverMenu(bool show)
    {
        gameOverPanel.SetActive(show);
        mainMenuPanel.SetActive(!show);
        if (show)
        {
            finalScoreText.text = "Your Final Score: " + GameOverManager.finalScore;
            if(!FBManager.newHighscore)
            {
                gameOverPanel.transform.Find("SetHighscoreButton").gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(GameObject.Find("GameOverManager"));
        }
    }

    public void ShowMainMenu(bool show)
    {
        mainMenuPanel.SetActive(show);
        
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
