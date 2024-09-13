using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField]
    private int damage = 4;
    [SerializeField]
    private float period = 1f;
    private bool reset = false;

    private void Start()
    {
        GetComponent<Renderer>().material.color = Color.red;

        StartCoroutine(LoadTrap());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" || reset)
            return;

        collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        reset = true;
    }

    private IEnumerator LoadTrap()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 topPos = Vector3.up;

        yield return new WaitForSeconds(period);
        reset = false;

        for(float i = 0; i < 0.1f; i += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, topPos, i / 0.1f);
            yield return null;
        }
        transform.localPosition = topPos;

        yield return new WaitForSeconds(0.75f);

        for (float i = 0; i < 5f; i += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, i / 5f);
            yield return null;
        }

        transform.localPosition = startPos;

        StartCoroutine(LoadTrap());
    }
}
