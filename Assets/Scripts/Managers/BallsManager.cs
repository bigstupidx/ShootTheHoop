using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BallsManager : MonoBehaviour {

    public static int balls;

    private Text ballsText;
    
	// Use this for initialization
	void Start () {
        balls = 5;
        ballsText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        ballsText.text = "Balls: " + balls;
	}
}
