using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunLine : MonoBehaviour
{
    [SerializeField]
    private bool startRun;

    public void OnTriggerEnter(Collider other)
    {
        if(other == null) return;

        if(other.tag == "Player")
        {
            if(startRun)
                UIController.instance.StartRun();
            else
                UIController.instance.EndRun();
        }
    }
}
