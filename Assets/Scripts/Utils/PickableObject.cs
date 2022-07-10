using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickableObjectScanner.Instance.StartScan();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //NOTE: this will result in bugs, where exit one objects boundaries will stop scan, while still inside other object boundaries
        if (other.CompareTag("Player"))
        {
            PickableObjectScanner.Instance.StopScan();
        }
    }
}
