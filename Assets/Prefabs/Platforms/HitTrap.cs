using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HitTrap : MonoBehaviour
{
    [SerializeField]
    private float loadTime = 1f;
    [SerializeField]
    private float reLoadTime = 5f;
    [SerializeField]
    private int damage = 2;
    [SerializeField]
    private Color loadingColor = Color.yellow;
    [SerializeField]
    private Color damageColor = Color.red;

    private bool loading = false;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Player" || loading)
            return;

        StartCoroutine(LoadTrap());
    }

    private IEnumerator LoadTrap()
    {
        loading = true;
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Color startColor = renderer.material.color;

        for(float i = 0; i < loadTime; i += Time.deltaTime)
        {
            renderer.material.color = Color.Lerp(startColor, loadingColor, i / loadTime);
            yield return null;
        }

        renderer.material.color = damageColor;

        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale * 1.1f, quaternion.identity);
        foreach(Collider hit in colliders)
        {
            if(hit.gameObject.tag == "Player")
                hit.GetComponent<PlayerController>().TakeDamage(damage);
        }

        for (float i = 0; i < reLoadTime; i += Time.deltaTime)
        {
            renderer.material.color = Color.Lerp(damageColor, startColor, i / reLoadTime);
            yield return null;
        }

        renderer.material.color = startColor;
        loading = false;
    }
}
