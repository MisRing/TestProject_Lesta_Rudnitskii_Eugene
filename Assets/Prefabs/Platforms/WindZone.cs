using UnityEngine;

public class WindZone : MonoBehaviour
{
    public float windStrength = 5.0f;
    private float timer = 0;

    private void Update()
    {
        if (timer <= 0)
        {
            transform.eulerAngles = new Vector3(0f, Random.Range(0, 4) * 90f, 0f);
            timer = 2;
        }

        timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
            rb.AddForce(transform.forward.normalized * windStrength, ForceMode.Force);

    }
}
