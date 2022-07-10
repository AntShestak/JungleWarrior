using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomManager : MonoBehaviour
{
    public static CameraZoomManager Instance { get; private set; }

    Camera m_mainCam;
    IEnumerator zoomCor;
    float m_initialFoV;
    float m_minZoomFoV = 12.0f;
    float m_maxZoomFoV = 6.0f;
    float m_pistolZoomFoV = 40.0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        m_mainCam = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_initialFoV = m_mainCam.fieldOfView;
    }

    public void SetSniperZoom(float perc)
    {
        m_mainCam.fieldOfView = Mathf.Lerp(m_minZoomFoV, m_maxZoomFoV, perc);
    }

   

    public void SetPistolZoom()
    {
        if (zoomCor != null)
            StopCoroutine(zoomCor);
        zoomCor = LerpFoVCor(m_pistolZoomFoV);
        StartCoroutine(zoomCor);
    }

    

    public void SetDefaultZoom()
    {
        if (zoomCor != null)
            StopCoroutine(zoomCor);
        zoomCor = LerpFoVCor(m_initialFoV);
        StartCoroutine(zoomCor);
    }

    IEnumerator LerpFoVCor(float targetFoV)
    {
        float i = 0;
        float speed = 5.5f;
        float startFoV = m_mainCam.fieldOfView;
        while (i < 1)
        {
            i += speed * Time.deltaTime;
            i = Mathf.Clamp01(i);
            m_mainCam.fieldOfView = Mathf.Lerp(startFoV, targetFoV, i);
            yield return null;
        }
    }
}

  



