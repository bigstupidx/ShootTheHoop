using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NormalModeManager : MonoBehaviour {

    public static int balls;
    public static int airborneBalls;

    private Text ballsText;
    private int ballsTextFontSize;
    private RectTransform ballsRectTransform;
    
	// Use this for initialization
	void Start () {
        balls = 5;
        airborneBalls = 0;
#if UNITY_STANDALONE
        ballsTextFontSize = 30;
#elif UNITY_ANDROID
        ballsTextFontSize = 70;
#endif
        ballsText = GetComponent<Text>();
        ballsRectTransform = GetComponent<RectTransform>();
        ballsText.fontSize = ballsTextFontSize;
#if UNITY_ANDROID
        ballsRectTransform.sizeDelta = new Vector2(320, 90);
        ballsRectTransform.anchoredPosition = new Vector2(-ballsRectTransform.rect.width / 2, -(ballsRectTransform.rect.height / 2));
#endif
    }

    // Update is called once per frame
    void Update () {
        ballsText.text = "Balls: " + balls;
	}
}
