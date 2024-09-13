using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour 
{
	public static ThirdPersonCamera instance;

	public float followSpeed = 3;
	public float cameraSpeed = 2;

	public Transform target;

	float turnSmoothing = 0.1f;
	public float minAngle = -35;
	public float maxAngle = 35;

	private float lookAngle;
	private float tiltAngle;

    void Awake()
    {
        instance = this;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
	
	private void FixedUpdate()
	{
		float inputX = Input.GetAxis("Mouse X");
		float inputY = Input.GetAxis("Mouse Y");

		FollowTarget(); 
		HandleRotations(inputX, inputY, cameraSpeed);
	}

	void FollowTarget()
	{
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
    }

    private float smoothX = 0f;
    private float smoothY = 0f;
    private float smoothXvelocity = 0f;
    private float smoothYvelocity = 0f;

    void HandleRotations(float inputX, float inputY, float targetSpeed)
	{
        smoothX = Mathf.SmoothDamp(smoothX, inputX, ref smoothXvelocity, turnSmoothing);
        smoothY = Mathf.SmoothDamp(smoothY, inputY, ref smoothYvelocity, turnSmoothing);

        tiltAngle -= smoothY * targetSpeed;
		tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        Camera.main.transform.parent.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

		lookAngle += smoothX * targetSpeed;
		transform.rotation = Quaternion.Euler(0, lookAngle, 0);
	}
}
