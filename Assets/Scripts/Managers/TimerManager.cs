using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    private Text timerText;
    public static float timeLeft;

	// Use this for initialization
	void Start () {
        timerText = GetComponent<Text>();
        timeLeft = 90;
        timerText.text = "Timer: " + (int)timeLeft;
	}
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        timerText.text = "Timer: " + (int)timeLeft;
	}
}
