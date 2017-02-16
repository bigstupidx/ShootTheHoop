using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreCollision : MonoBehaviour {

    private bool ballHasHitScoreCircle;
    public ComboManager comboManagerScript;
    public FlamesManager flamesManagerScript;
    private Manipulation manipulationScript;

    void Start()
    {
        manipulationScript = GetComponent<Manipulation>();
        ballHasHitScoreCircle = false;
    }
                                
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("ScoreCircle") && !ballHasHitScoreCircle)
        {
            ScoreManager.successfulShotsInARow++;
            ScoreManager.score += ScoreManager.successfulShotsInARow * ScoreManager.points;
            ballHasHitScoreCircle = true;
            if (SceneManager.GetActiveScene().name == "NormalMode") {
                BallsManager.balls++;
            }
            else
            {
                TimerManager.timeLeft += 5f;
            }
            comboManagerScript.showText();
            flamesManagerScript.EnableFlames();
        }
        else if(other.collider.CompareTag("Environment") && !ballHasHitScoreCircle)
        {
            ScoreManager.successfulShotsInARow = 0;
            flamesManagerScript.DisableFlames();
            if((SceneManager.GetActiveScene().name == "NormalMode" && BallsManager.balls == 0 && !manipulationScript.ballHasBeenThrown) ||
                (SceneManager.GetActiveScene().name == "TimeMode" && (int)TimerManager.timeLeft == 0))
            {
                GameOverManager.gameOver = true;
            }
        }
    }
}
