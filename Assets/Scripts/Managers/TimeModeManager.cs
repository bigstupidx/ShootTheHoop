using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeModeManager : MonoBehaviour {

    public static float timeLeft;
    public static int airborneBalls;

    private Text timerText;
    private int timerTextFontSize;
    private RectTransform timerRectTransform;

	// Use this for initialization
	void Start () {
        airborneBalls = 0;
#if UNITY_STANDALONE
        timerTextFontSize = 30;
#elif UNITY_ANDROID
        timerTextFontSize = 70;
#endif
        timerText = GetComponent<Text>();
        timerRectTransform = GetComponent<RectTransform>();
        timerText.fontSize = timerTextFontSize;
        timeLeft = 90;
        timerText.text = "Timer: " + (int)timeLeft;
#if UNITY_ANDROID
        timerRectTransform.sizeDelta = new Vector2(350, 90);
        timerRectTransform.anchoredPosition = new Vector2(-timerRectTransform.rect.width / 2, -(timerRectTransform.rect.height / 2));
#endif
    }

    // Update is called once per frame
    void Update () {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Timer: " + (int)timeLeft;
        }
	}
}
