using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private int currentHealth = 10;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform cameraTransform;

    private Rigidbody rb;
    [SerializeField]
    private bool isGrounded;

    void Start()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        UIController.instance.ChangeHealthBar(currentHealth, maxHealth);
    }

    void Update()
    {
        MovePlayer();
        Jump();

        if (transform.position.y < -5)
            TakeDamage(99999);
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = (cameraTransform.forward * vertical + cameraTransform.right * horizontal).normalized;
        moveDirection.y = 0;

        Vector3 targetPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(targetPosition);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = currentHealth < 0 ? 0 : currentHealth;

        UIController.instance.ChangeHealthBar(currentHealth, maxHealth);

        if (currentHealth == 0)
            UIController.instance.PlayerLost();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
