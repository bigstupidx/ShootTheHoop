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

#if UNITY_ANDROID
    public FBManager fbManagerScript;
    private float mainMenuPanelAnchorMinY = 0.3510594f;
    private float mainMenuPanelAnchorMaxY = 0.6684822f;
    private float controlsMenuPanelAnchorMinY = 0.3510594f;
    private float controlsMenuPanelAnchorMaxY = 0.6292154f;
    private float gameModesMenuPanelAnchorMinY = 0.3420139f;
    private float gameModesMenuPanelAnchorMaxY = 0.6758933f;
    private float gameOverMenuPanelAnchorMinY = 0.3669046f;
    private float gameOverMenuPanelAnchorMaxY = 0.6684822f;
#elif UNITY_STANDALONE
    private float mainMenuPanelAnchorMinY = 0.1640664f;
    private float mainMenuPanelAnchorMaxY = 0.8850016f;
    private float controlsMenuPanelAnchorMinY = 0f;
    private float controlsMenuPanelAnchorMaxY = 0.8323418f;
    private float gameModesMenuPanelAnchorMinY = 0.1640664f;
    private float gameModesMenuPanelAnchorMaxY = 0.8850016f;
    private float gameOverMenuPanelAnchorMinY = 0.1640664f;
    private float gameOverMenuPanelAnchorMaxY = 0.9193238f;
#endif

#if UNITY_STANDALONE

    void Awake()
    {
        GameObject.Find("FBManager").gameObject.SetActive(false);
    }

    void Start()
    {
        if(GameOverManager.gameOver)
        {
            ShowGameOverMenu(true);
            GameOverManager.gameOver = false;
        }
        else
        {
            ShowMainMenu(true);
        }
    }

#endif

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
#if UNITY_STANDALONE
        if (show)
        {
            RectTransform panelRectTransform = gameModesPanel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(panelRectTransform.anchorMin.x, gameModesMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, gameModesMenuPanelAnchorMaxY);
        }
#elif UNITY_ANDROID
        if (show)
        {
            RectTransform panelRectTransform = gameModesPanel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(panelRectTransform.anchorMin.x, gameModesMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, gameModesMenuPanelAnchorMaxY);
        }
#endif
    }

    public void ShowControlsMenu(bool show)
    {
        controlsPanel.SetActive(show);
        mainMenuPanel.SetActive(!show);
        if(show)
        {
#if UNITY_STANDALONE
            RectTransform panelRectTransform = controlsPanel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(panelRectTransform.anchorMin.x, controlsMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, controlsMenuPanelAnchorMaxY);
#elif UNITY_ANDROID
            RectTransform panelRectTransform = controlsPanel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(panelRectTransform.anchorMin.x, controlsMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, controlsMenuPanelAnchorMaxY);
#endif
        }
    }

    public void ShowLeaderboardMenu(bool show)
    {
        leaderboardPanel.SetActive(show);
        mainMenuPanel.SetActive(!show);
    }

    public void ShowGameOverMenu(bool show)
    {
        gameOverPanel.SetActive(show);
        ShowMainMenu(!show);
        if (show)
        {
            finalScoreText.text = "Your Final Score: " + GameOverManager.finalScore;
#if UNITY_ANDROID
            if(!FBManager.newHighscore)
            {
                gameOverPanel.transform.Find("SetHighscoreButton").gameObject.SetActive(false);
            }
            RectTransform panelRectTransform = gameOverPanel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(panelRectTransform.anchorMin.x, gameOverMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, gameOverMenuPanelAnchorMaxY);
#elif UNITY_STANDALONE
            RectTransform panelRectTransform = gameOverPanel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(panelRectTransform.anchorMin.x, gameOverMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, gameOverMenuPanelAnchorMaxY);

            gameOverPanel.transform.FindChild("ChallengeButton").gameObject.SetActive(false);
            gameOverPanel.transform.FindChild("SetHighscoreButton").gameObject.SetActive(false);
#endif
        }
        else
        {
            Destroy(GameObject.Find("GameOverManager"));
        }
    }

    public void ShowMainMenu(bool show)
    {
        mainMenuPanel.SetActive(show);
        if(show)
        {
            Button logoutOrExitButton = mainMenuPanel.transform.FindChild("MainButtonsPanel").FindChild("LogoutButton").GetComponent<Button>();
#if UNITY_STANDALONE
            RectTransform panelRectTransform = mainMenuPanel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(panelRectTransform.anchorMin.x, mainMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, mainMenuPanelAnchorMaxY);

            mainMenuPanel.transform.FindChild("MainButtonsPanel").FindChild("LeaderboardButton").gameObject.SetActive(false);
            mainMenuPanel.transform.FindChild("SideButtonsPanel").gameObject.SetActive(false);
            mainMenuPanel.transform.FindChild("MainButtonsPanel").FindChild("LogoutButton").FindChild("Text").GetComponent<Text>().text = "Exit";
            logoutOrExitButton.onClick.AddListener(delegate {  CloseGame(); });
#elif UNITY_ANDROID
            RectTransform panelRectTransform = mainMenuPanel.GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(panelRectTransform.anchorMin.x, mainMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, mainMenuPanelAnchorMaxY);
            logoutOrExitButton.onClick.AddListener(delegate { fbManagerScript.DealWithFBPanels(false); });
#endif
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
