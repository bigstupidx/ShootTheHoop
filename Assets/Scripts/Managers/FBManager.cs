using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FBManager : MonoBehaviour {

    public Text userName;
    public Image userProfilePic;
    public Text userHighscore;
    public MenuManager menuManagerScript;
    public GameObject leaderboard;
    public GameObject templatePlayerPanel;

    public static bool newHighscore = false;

	void Awake()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            DealWithFBPanels(FB.IsLoggedIn);
        }
        else
        {
            FB.Init(OnInitComplete, OnHideUnity);
        }
    }

    void OnInitComplete()
    {
        if(FB.IsInitialized)
        {
            FB.ActivateApp();
            DealWithFBPanels(FB.IsLoggedIn);
        }
        else
        {
            Debug.Log("Failed to initialize Facebook SDK");
        }
        
    }

    void OnHideUnity(bool isGameShown)
    {
        if(isGameShown)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void FacebookLoginWithReadPermissions()
    {
        List<string> permissions = new List<string>() {"public_profile", "user_friends"};
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result)
    {
        if(result.Error == null && !result.Cancelled)
        {
            if(FB.IsLoggedIn)
            {
                Debug.Log("Successfully logged in!");
            }
            else
            {
                Debug.Log("Failed to log in!");
            }

            DealWithFBPanels(FB.IsLoggedIn);
        }
        else if(result.Cancelled)
        {
            Debug.Log("Cancelled Login");
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    public void GetPlayersScore()
    {
        FB.API("/app/scores?fields=score,user.limit(30)", HttpMethod.GET, PopulateLeaderboard);
    }

    void PopulateLeaderboard(IResult res)
    {
        if (res.Error == null)
        {
            foreach(Transform entryPanel in leaderboard.transform)
            {
                Destroy(entryPanel.gameObject);
            }

            foreach(object obj in (List<object>)res.ResultDictionary["data"])
            {
                Dictionary<string, object> pEntry = (Dictionary<string, object>)obj;
                Dictionary<string, object> pInfo = (Dictionary<string, object>)pEntry["user"];

                GameObject playerPanel = Instantiate(templatePlayerPanel) as GameObject;
                playerPanel.transform.SetParent(leaderboard.transform, false);
                playerPanel.SetActive(true);

                Image pImage = playerPanel.transform.Find("Avatar").GetComponent<Image>();
                Text pName = playerPanel.transform.Find("FBName").GetComponent<Text>();
                Text pScore = playerPanel.transform.Find("Score").GetComponent<Text>();

                pName.text = pInfo["name"].ToString();
                pScore.text = pEntry["score"].ToString();

                FB.API(pInfo["id"] + "/picture?type=square&width=90&height=90", HttpMethod.GET, delegate(IGraphResult picRes) {
                    if(picRes.Texture != null)
                    {
                        pImage.sprite = Sprite.Create(picRes.Texture, new Rect(0, 0, 90, 90), new Vector2());
                    }
                    else
                    {
                        Debug.Log(picRes.Error);
                    }
                });
            }
        }
        else
        {
            Debug.Log(res.Error);
        }
    }

    public void SetNewHighscore()
    {
        if(AccessToken.CurrentAccessToken.Permissions.ToCommaSeparateList().Contains("publish_actions"))
        {
            Dictionary<string, string> scoreDic = new Dictionary<string, string>();
            scoreDic["score"] = GameOverManager.finalScore.ToString(); 
            FB.API("/me/scores", HttpMethod.POST, delegate(IGraphResult res) {
                if(res.Error == null)
                {
                    Debug.Log("Score submitted successfully!");
                    UpdatePlayerHighscore();
                }
                else
                {
                    Debug.Log(res.Error);
                }
            }, scoreDic);
        }
        else
        {
            FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, delegate(ILoginResult loginRes)
            {
                if(loginRes.Error == null && !loginRes.Cancelled)
                {
                    Dictionary<string, string> scoreDic = new Dictionary<string, string>();
                    scoreDic["score"] = GameOverManager.finalScore.ToString();
                    FB.API("/me/scores", HttpMethod.POST, delegate (IGraphResult scoreRes) {
                        if (scoreRes.Error == null)
                        {
                            Debug.Log("Score submitted successfully!");
                            UpdatePlayerHighscore();
                        }
                        else
                        {
                            Debug.Log(scoreRes.Error);
                        }
                    }, scoreDic);
                }
                else if(loginRes.Cancelled)
                {
                    Debug.Log("Permission cancelled");
                }
                else
                {
                    Debug.Log(loginRes.Error);
                }
            });
        }
    }

    public void ChallengeFriends()
    {
        FB.AppRequest("Hey, I just made " + GameOverManager.finalScore + " points. Can you beat that?",
                        null,
                        new List<object>() { "app_users" },
                        null, null, null, null, delegate(IAppRequestResult res) {
                            Debug.Log(res.RawResult);
                        });
    }

    public void RecommendToFriends()
    {
        FB.AppRequest("This game is awesome! Want to try it out?",
                        null,
                        new List<object>() { "app_non_users" },
                        null, null, null, null, delegate (IAppRequestResult res)
                        {
                            Debug.Log(res.RawResult);
                        });
    }

    public void DealWithFBPanels(bool isLoggedIn)
    {
        if(isLoggedIn)
        {
            menuManagerScript.ShowLoggedOutPanel(false);
            menuManagerScript.ShowLoggedInPanel(true);
            if (GameOverManager.gameOver)
            {
                CheckForNewHighscore();
                GameOverManager.gameOver = false;
            }
            else
            {
                menuManagerScript.ShowMainMenu(true);
            }

            FB.API("/me?fields=name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&width=130&height=130", HttpMethod.GET, DisplayProfilePic);
            UpdatePlayerHighscore();
        }
        else
        {
            menuManagerScript.ShowLoggedInPanel(false);
            menuManagerScript.ShowLoggedOutPanel(true);
            menuManagerScript.ShowMainMenu(false);
            if(FB.IsLoggedIn)
            {
                FB.LogOut();
            }
        }
    }

    void CheckForNewHighscore()
    {
        FB.API("/me/scores?fields=score", HttpMethod.GET, delegate(IGraphResult res) {
            List<object> playerScoresList = (List<object>)res.ResultDictionary["data"];
            if((playerScoresList.Count == 0 && GameOverManager.finalScore > 0) 
                || (playerScoresList.Count != 0 && int.Parse(((Dictionary<string, object>)playerScoresList[0])["score"].ToString()) < GameOverManager.finalScore))
            {
                newHighscore = true;
            }
            else
            {
                newHighscore = false;
            }
            menuManagerScript.ShowGameOverMenu(true);
        });
    }

    void UpdatePlayerHighscore()
    {
        FB.API("/me/scores?fields=score", HttpMethod.GET, DisplayHighscore);
    }

    void DisplayUsername(IResult res)
    {
        if(res.Error == null)
        {
            userName.text = res.ResultDictionary["name"].ToString();
        }
        else
        {
            Debug.Log(res.Error);
        }
    }

    void DisplayProfilePic(IGraphResult res)
    {
        if(res.Error == null && res.Texture != null)
        {
            userProfilePic.sprite = Sprite.Create(res.Texture, new Rect(0, 0, 130, 130), new Vector2());
        }
        else
        {
            Debug.Log(res.Error);
        }
    }

    void DisplayHighscore(IResult res)
    {
        if (res.Error == null)
        {
            List<object> playerScoresList = (List<object>)res.ResultDictionary["data"];
            if (playerScoresList.Count > 0)
            {
                userHighscore.text = ((Dictionary<string, object>)playerScoresList[0])["score"].ToString();
            }
            else
            {
                userHighscore.text = "0";
            }
        }
        else
        {
            Debug.Log(res.Error);
        }
    }
}
