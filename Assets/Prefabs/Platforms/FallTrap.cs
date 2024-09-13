using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    [SerializeField]
    private float timer = 2f;

    private Vector3 position;

    private bool loading = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" || loading)
            return;

        StartCoroutine(LoadTrap());
    }
    private IEnumerator LoadTrap()
    {
        loading = true;
        position = transform.position;

        for (float i = 0; i < timer; i += Time.deltaTime)
        {
            transform.position = position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            yield return null;
        }

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().size = Vector3.one * 0.9f;
    }
}
