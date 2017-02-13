using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FBManager : MonoBehaviour {

    public Text username;
    public Image profilePic;
    public MenuManager menuManagerScript;

	void Awake()
    {
        if (FB.IsInitialized)
        {
            DealWithFBPanels(FB.IsLoggedIn);
            FB.ActivateApp();
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
        if(result.Error == null)
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
        if(res.Error == null)
        {
            
        }
        else
        {
            Debug.Log(res.Error);
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
            Debug.Log(GameOverManager.gameOver);
            if(GameOverManager.gameOver)
            {
                menuManagerScript.ShowGameOverMenu(true);
                GameOverManager.gameOver = false;
            }
            else
            {
                menuManagerScript.ShowMainMenu(true);
            }

            FB.API("/me?fields=name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&width=130&height=130", HttpMethod.GET, DisplayProfilePic);
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

    void DisplayUsername(IResult res)
    {
        if(res.Error == null)
        {
            username.text = res.ResultDictionary["name"].ToString();
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
            profilePic.sprite = Sprite.Create(res.Texture, new Rect(0, 0, 130, 130), new Vector2());
        }
        else
        {
            Debug.Log(res.Error);
        }
    }
}
