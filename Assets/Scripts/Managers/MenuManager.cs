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
    public GameObject ballPanel;
    public Text finalScoreText;

#if UNITY_ANDROID
    private FBManager fbManagerScript;
    private int ballPanelWidth = 1400;
    private int ballPanelHeight = 1100;
#elif UNITY_STANDALONE
    private float mainMenuPanelAnchorMinY = 0.1640664f;
    private float mainMenuPanelAnchorMaxY = 0.8494107f;
    private float controlsMenuPanelAnchorMinY = 0.1640664f;
    private float controlsMenuPanelAnchorMinX = 0.1462079f;
    private float controlsMenuPanelAnchorMaxY = 0.7685246f;
    private float controlsMenuPanelAnchorMaxX = 0.8598111f;
    private float gameModesMenuPanelAnchorMinY = 0.1640664f;
    private float gameModesMenuPanelAnchorMaxY = 0.8494107f;
    private float gameOverMenuPanelAnchorMinY = 0.3075388f;
    private float gameOverMenuPanelAnchorMaxY = 0.8516831f;
    private int ballPanelWidth = 1200;
    private int ballPanelHeight = 900;
#endif

#if UNITY_STANDALONE

    void Awake()
    {
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(1366, 768);
        GameObject.Find("FBManager").gameObject.SetActive(false);
        ballPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(ballPanelWidth, ballPanelHeight);
        if (GameOverManager.gameOver)
        {
            ShowGameOverMenu(true);
            GameOverManager.gameOver = false;
        }
        else
        {
            ShowMainMenu(true);
        }
    }
#elif UNITY_ANDROID
    void Awake()
    {
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080, 1920);
        fbManagerScript = GameObject.Find("FBManager").GetComponent<FBManager>();
        ballPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(ballPanelWidth, ballPanelHeight);
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
            panelRectTransform.anchorMin = new Vector2(controlsMenuPanelAnchorMinX, controlsMenuPanelAnchorMinY);
            panelRectTransform.anchorMax = new Vector2(controlsMenuPanelAnchorMaxX, controlsMenuPanelAnchorMaxY);
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
            logoutOrExitButton.onClick.AddListener(delegate { fbManagerScript.DealWithFBPanels(false); });
#endif
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
