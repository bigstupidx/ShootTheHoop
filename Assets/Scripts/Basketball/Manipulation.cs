using UnityEngine;
using System.Collections;

public class Manipulation : MonoBehaviour
{
    private Rigidbody ballRigidbody;
    private Transform ballTransform;
    private float ballVelocity;
    private Vector3 ballPos;
    private Vector3 firstMousePos;
    private Vector3 secondMousePos;
    private bool ballHasBeenThrown = false;
    private bool ballHasCollidedOnce = false;
    private Vector3[] ballPositions;
    private Quaternion[] ballRotations;
    private CameraAngle camAngleScript;

    public float ballAcceleration;

    void Start()
    {
        camAngleScript = Camera.main.GetComponent<CameraAngle>();
        ballRigidbody = GetComponent<Rigidbody>();
        ballTransform = GetComponent<Transform>();
        InitializeBallPositionsOnField();
        ballVelocity = 100;
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

    void FixedUpdate()
    {
        if (ballHasBeenThrown)
        {
            ballTransform.RotateAround(ballTransform.position, ballTransform.forward, Time.deltaTime * 180f);
        }
    }

#if UNITY_STANDALONE

    void OnMouseDown()
    {
        if (ballRigidbody.isKinematic)
        {
            ballRigidbody.isKinematic = false;
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
        ballRigidbody.AddRelativeForce(new Vector3(ballVelocity * 2.5f, ballVelocity * 5.3f, 0));
        ballHasBeenThrown = true;
        ballVelocity = 100;
        BallsManager.balls--;
    }

    void MakeBallFollowMouse()
    {
        ballPos = Input.mousePosition;
        ballPos.z = 1;
        ballPos = Camera.main.ScreenToWorldPoint(ballPos);
        ballTransform.position = ballPos;
    }

#endif

    void ChangeBallSpeed()
    {
        secondMousePos = Input.mousePosition;
        secondMousePos.z = 1;
        secondMousePos = Camera.main.ScreenToWorldPoint(secondMousePos);
        if (firstMousePos.y < secondMousePos.y - 0.08f)
        {
            ballVelocity += ballAcceleration;
            if (ballVelocity > 107)
            {
                ballVelocity = 107;
            }
        }
        else if (firstMousePos == secondMousePos)
        {
            if (ballVelocity - ballAcceleration * 20 >= 100)
            {
                ballVelocity -= ballAcceleration * 20;
            }
            else if (ballVelocity != 100)
            {
                ballVelocity = 100;
            }
        }
        firstMousePos = secondMousePos;
        Debug.Log(ballVelocity);
    }

    void OnCollisionEnter()
    {
        if(!ballHasCollidedOnce) {
            ballHasBeenThrown = false;
            int randIndex = Random.Range(0, 11);
            GenerateNewBallWithPositionIndex(randIndex);
            camAngleScript.ChangeAngle(randIndex);
            DeleteThrownBall();
            ballHasCollidedOnce = true;
        } 
    }

    void GenerateNewBallWithPositionIndex(int index)
    {
        GameObject newBall;
        newBall = Instantiate(gameObject, ballPositions[index], ballRotations[index]) as GameObject;
        newBall.GetComponent<Rigidbody>().isKinematic = true;
    }

    void DeleteThrownBall()
    {
        Destroy(gameObject, 3f);
    }
}
