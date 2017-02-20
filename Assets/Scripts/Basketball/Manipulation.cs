using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Manipulation : MonoBehaviour
{
    private Rigidbody ballRigidbody;
    private Transform ballTransform;
    private float ballVelocity;
    private Vector3 ballPos;
    private bool ballHasCollidedOnce = false;
    private bool ballHasBeenThrown = false;
    private Vector3[] ballPositions;
    private Quaternion[] ballRotations;
    private CameraAngle camAngleScript;

    private float xVelMultiplier = 2.5f;
    private float yVelMultiplier = 5.3f;

#if UNITY_STANDALONE
    private Vector3 firstMousePos;
    private Vector3 secondMousePos;
#endif
#if UNITY_ANDROID
    private SphereCollider ballColl;
#endif

    public float ballAcceleration;

    void Start()
    {
        #if UNITY_ANDROID
        Input.multiTouchEnabled = false;
        ballColl = GetComponent<SphereCollider>();
        #endif
        camAngleScript = Camera.main.GetComponent<CameraAngle>();
        ballRigidbody = GetComponent<Rigidbody>();
        ballTransform = GetComponent<Transform>();
        InitializeBallPositionsOnField();
        ballVelocity = 0;
    }

    void InitializeBallPositionsOnField()
    {
        ballPositions = new Vector3[11];
        ballRotations = new Quaternion[11];
        ballPositions[0] = new Vector3(-8.33f, 3.66f, -6.25f);
        ballRotations[0] = Quaternion.Euler(0, 0, 0);
        ballPositions[1] = new Vector3(-7.68f, 3.66f, -3.22f);
        ballRotations[1] = Quaternion.Euler(0, 16.717f, 0);
        ballPositions[2] = new Vector3(-6.3f, 3.66f, -1.001f);
        ballRotations[2] = Quaternion.Euler(0, 32.085f, 0);
        ballPositions[3] = new Vector3(-4.28f, 3.66f, 0.54f);
        ballRotations[3] = Quaternion.Euler(0, 46.491f, 0);
        ballPositions[4] = new Vector3(-2.06f, 3.66f, 1.92f);
        ballRotations[4] = Quaternion.Euler(0, 65.314f, 0);
        ballPositions[5] = new Vector3(-7.84f, 3.66f, -9.23f);
        ballRotations[5] = Quaternion.Euler(0, -15.787f, 0);
        ballPositions[6] = new Vector3(1.209f, 3.66f, 3.046f);
        ballRotations[6] = Quaternion.Euler(0, 86.691f, 0);
        ballPositions[7] = new Vector3(-6.43f, 3.66f, -11.27f);
        ballRotations[7] = Quaternion.Euler(0, -30.804f, 0);
        ballPositions[8] = new Vector3(-4.45f, 3.66f, -13.31f);
        ballRotations[8] = Quaternion.Euler(0, -48.309f, 0);
        ballPositions[9] = new Vector3(-1.89f, 3.66f, -14.48f);
        ballRotations[9] = Quaternion.Euler(0, -66.189f, -0.6950001f);
        ballPositions[10] = new Vector3(1.865f, 3.66f, -14.88f);
        ballRotations[10] = Quaternion.Euler(0, -91.96101f, 0);
    }

    void Update()
    {
        if (ballHasBeenThrown)
        {
            ballTransform.RotateAround(ballTransform.position, ballTransform.forward, Time.deltaTime * 180f);
        }
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch firstTouch = Input.GetTouch(0);
            if (BallIsTouched(firstTouch))
            {
                if (firstTouch.phase == TouchPhase.Began)
                {
                    if (ballRigidbody.isKinematic)
                    {
                        ballRigidbody.isKinematic = false;
                    }
                }
                if (firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Stationary)
                {
                    MakeBallFollowFinger(firstTouch);
                    ChangeBallSpeed(firstTouch);
                }
                if (firstTouch.phase == TouchPhase.Ended)
                {
                    ballRigidbody.AddRelativeForce(new Vector3(ballVelocity * xVelMultiplier, ballVelocity * yVelMultiplier, 0));
                    ballHasBeenThrown = true;
                    ballVelocity = 0;
                    if(SceneManager.GetActiveScene().name == "NormalMode")
                    {
                        if(NormalModeManager.balls > 0)
                        {
                            NormalModeManager.balls--;
                        }
                        NormalModeManager.airborneBalls++;
                    }
                    else
                    {
                        TimeModeManager.airborneBalls++;
                    }
                }
            }
        }
#endif
    }

#if UNITY_ANDROID

    bool BallIsTouched(Touch t)
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(t.position);
        return ballColl.Raycast(ray, out hit, 1f);
    }

    void MakeBallFollowFinger(Touch t)
    {
        Vector3 newBallPos = new Vector3(t.position.x, t.position.y, 1);
        ballTransform.position = Camera.main.ScreenToWorldPoint(newBallPos);
    }

    void ChangeBallSpeed(Touch t)
    {
        if(t.phase == TouchPhase.Moved)
        {
            if(ballVelocity == 0)
            {
                ballVelocity = 100;
            }
            ballVelocity += ballAcceleration;
            if (ballVelocity > 106)
            {
                ballVelocity = 106;
            }
        }
        else
        {
            if (ballVelocity - ballAcceleration * 2 >= 100)
            {
                ballVelocity -= ballAcceleration * 2;
            }
            else
            {
                ballVelocity = 0;
            }
        }
    }
#endif

#if UNITY_STANDALONE

    void OnMouseDown()
    {
        if (ballRigidbody.isKinematic)
        {
            ballRigidbody.isKinematic = false;
            ballRigidbody.WakeUp();
        }
        firstMousePos = Input.mousePosition;
        firstMousePos.z = 1;
        firstMousePos = Camera.main.ScreenToWorldPoint(firstMousePos);
    }

    void OnMouseDrag()
    {
        MakeBallFollowMouse();
        ChangeBallSpeed();
    }

    void OnMouseUp()
    {
        // Signals the ball has been released applying the accumulated velocity to its rigidbody
        ballRigidbody.AddRelativeForce(new Vector3(ballVelocity * xVelMultiplier, ballVelocity * yVelMultiplier, 0));
        ballHasBeenThrown = true;
        ballVelocity = 0;
        if(SceneManager.GetActiveScene().name == "NormalMode") {
            NormalModeManager.balls--;
            NormalModeManager.airborneBalls++;
        }
        else {
            TimeModeManager.airborneBalls++;
        }
    }

    void MakeBallFollowMouse()
    {
        ballPos = Input.mousePosition;
        ballPos.z = 1;
        ballPos = Camera.main.ScreenToWorldPoint(ballPos);
        ballTransform.position = ballPos;
    }

    void ChangeBallSpeed()
    {
        secondMousePos = Input.mousePosition;
        secondMousePos.z = 1;
        secondMousePos = Camera.main.ScreenToWorldPoint(secondMousePos);
        if (firstMousePos.y < secondMousePos.y - 0.08f)
        {
            if(ballVelocity == 0)
            {
                ballVelocity = 100;
            }
            ballVelocity += ballAcceleration;
            if (ballVelocity > 106)
            {
                ballVelocity = 106;
            }
        }
        else if (firstMousePos == secondMousePos)
        {
            if (ballVelocity - ballAcceleration * 3 >= 100)
            {
                ballVelocity -= ballAcceleration * 3;
            }
            else
            {
                ballVelocity = 0;
            }
        }
        firstMousePos = secondMousePos;
    }

#endif

    void OnCollisionEnter(Collision collision)
    {
        if(!ballHasCollidedOnce) {
            ballHasBeenThrown = false;
            if((SceneManager.GetActiveScene().name == "NormalMode" && NormalModeManager.balls > 0) || 
                (SceneManager.GetActiveScene().name == "TimeMode" && TimeModeManager.timeLeft > 0.0f))
            {
                ChangeShootingPosition();
            }
            ballHasCollidedOnce = true;
        }
        if (collision.collider.CompareTag("Environment"))
        {
            DeleteThrownBall();
        }
    }

    public void ChangeShootingPosition()
    {
        int randIndex = Random.Range(0, 11);
        GenerateNewBallWithPositionIndex(randIndex);
        camAngleScript.ChangeAngle(randIndex);
    }

    void GenerateNewBallWithPositionIndex(int index)
    {
        GameObject newBall;
        newBall = Instantiate(gameObject, ballPositions[index], ballRotations[index]) as GameObject;
        newBall.GetComponent<Rigidbody>().isKinematic = true;
    }

    void DeleteThrownBall()
    {
        Destroy(gameObject, 1f);
    }
}
