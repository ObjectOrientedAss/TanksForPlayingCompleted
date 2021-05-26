using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private InputHandler inputHandler;
    private Transform target;
    public Camera Camera; //set in inspector
    public Vector3 CameraForward { get { return (new Vector3(Camera.transform.position.x, 0, Camera.transform.position.z) - new Vector3(target.position.x, 0, target.position.z)).normalized; } }

    public float OrbitalRotationSpeed;
    public float FollowMovementSpeed;

    public void Init(Transform target, InputHandler inputHandler)
    {
        this.target = target;
        this.inputHandler = inputHandler;

        enabled = true;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, FollowMovementSpeed * Time.deltaTime);

        if (inputHandler.cameraHorizontal != 0)
        {
            if (inputHandler.currentControlScheme == ControlScheme.KeyboardAndMouse)
            {
                if (inputHandler.cameraLockButtonPressed)
                {
                    transform.rotation *= Quaternion.Euler(Vector3.up * inputHandler.cameraHorizontal * OrbitalRotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                transform.rotation *= Quaternion.Euler(Vector3.up * inputHandler.cameraHorizontal * OrbitalRotationSpeed * Time.deltaTime);
            }
        }
    }
}
