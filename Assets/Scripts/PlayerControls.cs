using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class PlayerController : MonoBehaviour 
{
    public static PlayerController instance;

    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private int currentHealth = 10;

    public float speed = 10.0f;
	public float airVelocity = 8f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;
	public float maxFallSpeed = 20.0f;
	public float rotateSpeed = 25f;
	private Vector3 moveDir;
	public GameObject playerCamera;
	private Rigidbody rb;

	private float distToGround;

	private bool canMove = true;
	private bool isStuned = false;
	private bool wasStuned = false;
	private float pushForce;
	private Vector3 pushDir;

	private void Awake()
	{
		instance = this;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		currentHealth = maxHealth;

		UIController.instance.ChangeHealthBar(currentHealth, maxHealth);
		distToGround = GetComponent<Collider>().bounds.extents.y;

		GetComponent<Renderer>().material.color = Color.yellow;
	}
	
	bool IsGrounded ()
	{
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = currentHealth < 0 ? 0 : currentHealth;

        UIController.instance.ChangeHealthBar(currentHealth, maxHealth);

        if (currentHealth == 0)
            UIController.instance.PlayerLost();
    }

    void FixedUpdate ()
	{
		if (canMove)
		{
			if (moveDir.x != 0 || moveDir.z != 0)
			{
				Vector3 targetDir = moveDir;
				targetDir.y = 0;

				if (targetDir == Vector3.zero)
					targetDir = transform.forward;
				Quaternion tr = Quaternion.LookRotation(targetDir); 
				Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed);
				transform.rotation = targetRotation;
			}

			if (IsGrounded())
			{
				Vector3 targetVelocity = moveDir;
				targetVelocity *= speed;

				Vector3 velocity = rb.velocity;
				if (targetVelocity.magnitude < velocity.magnitude)
				{
					targetVelocity = velocity;
					rb.velocity /= 1.1f;
				}

				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0;
                if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
					rb.AddForce(velocityChange, ForceMode.VelocityChange);

                if (IsGrounded() && Input.GetButton("Jump"))
				{
					rb.velocity = new Vector3(velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), velocity.z);
				}
			}
			else
			{
                Vector3 targetVelocity = new Vector3(moveDir.x * airVelocity, rb.velocity.y, moveDir.z * airVelocity);
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                rb.AddForce(velocityChange, ForceMode.Acceleration);
                if (velocity.y < -maxFallSpeed)
                    rb.velocity = new Vector3(velocity.x, -maxFallSpeed, velocity.z);
            }
		}
		else
		{
			rb.velocity = pushDir * pushForce;
		}

		rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0), ForceMode.Acceleration);
	}

	private void Update()
	{
        if (transform.position.y < -5)
            TakeDamage(99999);

        float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		moveDir = (inputX * playerCamera.transform.right + inputY * playerCamera.transform.forward).normalized;
	}

	public void HitPlayer(Vector3 velocityF, float time)
	{
		rb.velocity = velocityF;

		pushForce = velocityF.magnitude;
		pushDir = Vector3.Normalize(velocityF);
		StartCoroutine(Decrease(velocityF.magnitude, time));
	}

	private IEnumerator Decrease(float value, float duration)
	{
		if (isStuned)
			wasStuned = true;
		isStuned = true;
		canMove = false;

		float delta = 0;
		delta = value / duration;

		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			yield return null;
            pushForce = pushForce - Time.deltaTime * delta;
            pushForce = pushForce < 0 ? 0 : pushForce;
            rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0)); //Add gravity
		}

		if (wasStuned)
		{
			wasStuned = false;
		}
		else
		{
			isStuned = false;
			canMove = true;
		}
	}
}
