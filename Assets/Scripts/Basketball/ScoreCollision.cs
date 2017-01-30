using UnityEngine;
using System.Collections;

public class ScoreCollision : MonoBehaviour {

    private bool ballHasHitScoreCircle;
    public ComboManager comboManagerScript;

    void Start()
    {
        ballHasHitScoreCircle = false;
    }
                                
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("ScoreCircle") && !ballHasHitScoreCircle)
        {
            ScoreManager.successfulShotsInARow++;
            ScoreManager.score += ScoreManager.successfulShotsInARow * ScoreManager.points;
            ballHasHitScoreCircle = true;
            BallsManager.balls++;
            comboManagerScript.showText();
        }
        else if(other.collider.CompareTag("Environment") && !ballHasHitScoreCircle)
        {
            ScoreManager.successfulShotsInARow = 0;
        }
    }
}
