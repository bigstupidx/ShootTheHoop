using UnityEngine;
using System.Collections;

public class CameraAngle : MonoBehaviour {

    private Vector3[] camAnglesPositions;
    private Quaternion[] camAnglesRotations;
    private Transform camTransform;

    void Start()
    {
        camTransform = GetComponent<Transform>();
        InitializeCameraAngles();
    }

    void InitializeCameraAngles()
    {
        camAnglesPositions = new Vector3[11];
        camAnglesRotations = new Quaternion[11];
        camAnglesPositions[0] = new Vector3(-9.12f, 4.17f, -6.26f);
        camAnglesRotations[0] = Quaternion.Euler(0, 90, 0);
        camAnglesPositions[1] = new Vector3(-8.434f, 4.17f, -2.955f);
        camAnglesRotations[1] = Quaternion.Euler(0, 107.11f, 0);
        camAnglesPositions[2] = new Vector3(-6.955f, 4.17f, -0.521f);
        camAnglesRotations[2] = Quaternion.Euler(0, 126.043f, 0);
        camAnglesPositions[3] = new Vector3(-4.807f, 4.17f, 1.138f);
        camAnglesRotations[3] = Quaternion.Euler(0, 137.096f, 0);
        camAnglesPositions[4] = new Vector3(-2.334f, 4.17f, 2.529f);
        camAnglesRotations[4] = Quaternion.Euler(0, 158.3f, 0);
        camAnglesPositions[5] = new Vector3(-8.582f, 4.17f, -9.473f);
        camAnglesRotations[5] = Quaternion.Euler(0, 72.24001f, 0);
        camAnglesPositions[6] = new Vector3(1.177f, 4.17f, 3.785f);
        camAnglesRotations[6] = Quaternion.Euler(0, 176.601f, 0);
        camAnglesPositions[7] = new Vector3(-7.09f, 4.17f, -11.69f);
        camAnglesRotations[7] = Quaternion.Euler(0, 57.959f, 0);
        camAnglesPositions[8] = new Vector3(-4.94f, 4.17f, -13.91f);
        camAnglesRotations[8] = Quaternion.Euler(0, 39.908f, 0);
        camAnglesPositions[9] = new Vector3(-2.2f, 4.17f, -15.19f);
        camAnglesRotations[9] = Quaternion.Euler(0, 22.622f, 0);
        camAnglesPositions[10] = new Vector3(1.9f, 4.17f, -15.68f);
        camAnglesRotations[10] = Quaternion.Euler(0, 0.205f, 0);
    }

    public void ChangeAngle(int index)
    {
        camTransform.position = camAnglesPositions[index];
        camTransform.rotation = camAnglesRotations[index];
    }
}
