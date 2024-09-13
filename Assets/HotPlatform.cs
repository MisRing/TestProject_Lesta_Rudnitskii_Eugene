using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPlatform : MonoBehaviour
{
    [SerializeField]
    private float damageDuration = 0.3f;
    [SerializeField]
    private float loadSpeed = 1.25f;
    [SerializeField]
    private int damage = 1;

    private float timer = 0f;
    private bool loading = false;
    private bool heated = false;

    private void Update()
    {
        timer -= Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" || loading)
            return;

        StartCoroutine(StartDamage());
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Player" || !heated)
            return;

        if(timer <= 0f)
        {
            timer = damageDuration;

            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }

    }

    private IEnumerator StartDamage()
    {
        loading = true;
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Color startColor = renderer.material.color;

        for(float i = 0; i < loadSpeed; i += Time.deltaTime)
        {
            renderer.material.color = Color.Lerp(startColor, Color.red, i);
            yield return null;
        }

        renderer.material.color = Color.red;
        heated = true;
    }
}
