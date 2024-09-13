using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : MonoBehaviour
{
    public float speed = 3f;

    private void Start()
    {
        speed *= Random.Range(0, 2) == 0 ? 1 : -1;
    }
    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime / 0.01f, Space.Self);
    }
}
