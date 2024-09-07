using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Цель, за которой следует камера (персонаж)
    public Transform player;
    public float distance = 5.0f; // Расстояние от камеры до персонажа
    public float height = 2.0f; // Высота, на которой находится камера
    public float smoothSpeed = 0.125f; // Скорость сглаживания камеры при следовании
    public Vector3 offset; // Смещение для настройки позиции камеры

    public float rotationSpeed = 100f; // Скорость вращения камеры вокруг персонажа

    private void Start()
    {
        offset = new Vector3(0, height, -distance); // Устанавливаем начальное смещение
    }

    void LateUpdate()
    {
       // target.position = player.position;
        // Вращение камеры вокруг персонажа по оси Y с помощью мыши
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        target.Rotate(0, horizontal, 0);

        // Смещение камеры относительно позиции персонажа
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // Плавное движение камеры
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Всегда смотрим на персонажа
        transform.LookAt(target.position + Vector3.up * height / 2);
    }
}
