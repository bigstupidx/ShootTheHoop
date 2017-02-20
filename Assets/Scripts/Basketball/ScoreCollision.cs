using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreCollision : MonoBehaviour {

    private bool ballHasHitScoreCircle;
    private bool ballHasHitGround;
    public ComboManager comboManagerScript;
    public FlamesManager flamesManagerScript;
    private Manipulation manipulationScript;

    void Start()
    {
        manipulationScript = GetComponent<Manipulation>();
        ballHasHitScoreCircle = false;
        ballHasHitGround = false;
    }
                                
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("ScoreCircle") && !ballHasHitScoreCircle)
        {
            ScoreManager.successfulShotsInARow++;
            ScoreManager.score += ScoreManager.successfulShotsInARow * ScoreManager.points;
            ballHasHitScoreCircle = true;
            if (SceneManager.GetActiveScene().name == "NormalMode") {
                NormalModeManager.balls++;
                if(NormalModeManager.balls == 1)
                {
                    manipulationScript.ChangeShootingPosition();
                }
            }
            else
            {
                TimeModeManager.timeLeft += 5f;
            }
            comboManagerScript.showText();
            flamesManagerScript.EnableFlames();
        }
        else if(other.collider.CompareTag("Environment"))
        {
            if (!ballHasHitGround)
            {
                if (SceneManager.GetActiveScene().name == "NormalMode")
                {
                    if (NormalModeManager.airborneBalls > 0)
                    {
                        NormalModeManager.airborneBalls--;
                    }
                }
                else
                {
                    if (TimeModeManager.airborneBalls > 0)
                    {
                        TimeModeManager.airborneBalls--;
                    }
                }
                if(!ballHasHitScoreCircle)
                {
                    ScoreManager.successfulShotsInARow = 0;
                    flamesManagerScript.DisableFlames();
                    if ((SceneManager.GetActiveScene().name == "NormalMode" && NormalModeManager.balls == 0 && NormalModeManager.airborneBalls == 0) ||
                        (SceneManager.GetActiveScene().name == "TimeMode" && (int)TimeModeManager.timeLeft == 0) && TimeModeManager.airborneBalls == 0)
                    {
                        GameOverManager.gameOver = true;
                    }
                }
                ballHasHitGround = true;
            }
        }
    }
}
