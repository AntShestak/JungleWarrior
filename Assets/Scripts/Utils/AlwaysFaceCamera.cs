using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{

    private Camera cam;
    private WaitForSeconds waitSec = new WaitForSeconds(0.1f);

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        StartCoroutine("FaceCamCor");
    }

    IEnumerator FaceCamCor() {
        while (true)
        {
            transform.LookAt(cam.transform);
            yield return waitSec;
        }
        
    }
}
