using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<Transform> platforms;
    public List<GameObject> traps;
    void Start()
    {
        platforms.AddRange(gameObject.GetComponentsInChildren<Transform>());
        foreach (var platform in platforms)
        {
            if (platform == transform)
                continue;

            if (Random.Range(0, 100f) < 70)
            {
                GameObject trap = Instantiate(traps[Random.Range(0,traps.Count)]);
                trap.transform.parent = transform;
                trap.transform.position = platform.position;
                Destroy(platform.gameObject);
            }
        }    
    }
}
