using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;         
    public float distance = 5f;     
    public float heightOffset = 1f;  
    public float rotationSpeed = 100f;

    public float minYAngle = -40f;     
    public float maxYAngle = 60f;     

    private float currentYRotation = 0f;    
    private float currentXRotation = 0f;    

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        if(PlayerController.instance.enabled == false)
        {
            horizontalInput = 0f;
            verticalInput = 0f;
        }

        currentXRotation += horizontalInput * rotationSpeed * Time.deltaTime;
        currentYRotation -= verticalInput * rotationSpeed * Time.deltaTime;

        currentYRotation = Mathf.Clamp(currentYRotation, minYAngle, maxYAngle);

        Quaternion cameraRotation = Quaternion.Euler(currentYRotation, currentXRotation, 0);

        Vector3 cameraPosition = player.position - cameraRotation * Vector3.forward * distance + Vector3.up * heightOffset;

        transform.position = cameraPosition;

        transform.LookAt(player.position + Vector3.up * heightOffset);
    }
}
