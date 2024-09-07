using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // ����, �� ������� ������� ������ (��������)
    public Transform player;
    public float distance = 5.0f; // ���������� �� ������ �� ���������
    public float height = 2.0f; // ������, �� ������� ��������� ������
    public float smoothSpeed = 0.125f; // �������� ����������� ������ ��� ����������
    public Vector3 offset; // �������� ��� ��������� ������� ������

    public float rotationSpeed = 100f; // �������� �������� ������ ������ ���������

    private void Start()
    {
        offset = new Vector3(0, height, -distance); // ������������� ��������� ��������
    }

    void LateUpdate()
    {
       // target.position = player.position;
        // �������� ������ ������ ��������� �� ��� Y � ������� ����
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        target.Rotate(0, horizontal, 0);

        // �������� ������ ������������ ������� ���������
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // ������� �������� ������
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ������ ������� �� ���������
        transform.LookAt(target.position + Vector3.up * height / 2);
    }
}
