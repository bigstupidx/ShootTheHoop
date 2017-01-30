using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public static int score;
    public static int successfulShotsInARow;
    public static int points;

    private Text scoreText;

	// Use this for initialization
	void Start () {
        score = 0;
        successfulShotsInARow = 0;
        points = 10;
        scoreText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score: " + score;
	}
}
