using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public static int score;
    public static int successfulShotsInARow;
    public static int points;

    private Text scoreText;
    private RectTransform scoreRectTransform;
    private bool modifyScoreTextWidth;
    private int scoreRectXOffset;
    private int scoreTextFontSize;

    // Use this for initialization
    void Start () {
        score = 0;
        successfulShotsInARow = 0;
        points = 10;
        modifyScoreTextWidth = true;
        scoreRectXOffset = 15;
#if UNITY_STANDALONE
        scoreTextFontSize = 30;
#elif UNITY_ANDROID
        scoreTextFontSize = 70;
#endif
        scoreText = GetComponent<Text>();
        scoreRectTransform = GetComponent<RectTransform>();
        scoreText.fontSize = scoreTextFontSize;
#if UNITY_ANDROID  
        scoreRectTransform.sizeDelta = new Vector2(350, 90);
        scoreRectTransform.anchoredPosition = new Vector2(scoreRectTransform.rect.width / 2, -(scoreRectTransform.rect.height / 2));
#endif
    }

    // Update is called once per frame
    void Update () {
        if(score >= 100 && modifyScoreTextWidth)
        {
            scoreRectTransform.position = new Vector3(scoreRectTransform.position.x + scoreRectXOffset, scoreRectTransform.position.y);
            scoreRectTransform.sizeDelta = new Vector2(scoreRectTransform.position.x * 2, scoreRectTransform.rect.height);
            modifyScoreTextWidth = false;
        }
        scoreText.text = "Score: " + score;
	}
}
