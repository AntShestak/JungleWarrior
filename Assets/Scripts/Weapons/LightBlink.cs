using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightBlink : MonoBehaviour
{
    [SerializeField] private float m_blinkLengthSeconds = 0.1f;

    Light m_light;
    // Start is called before the first frame update
    void Awake()
    {
        m_light = GetComponent<Light>();
    }

    private void Start()
    {
        m_light.enabled = false;
    }

    public void Blink()
    {
        StartCoroutine("BlinkCor");
    }

    private IEnumerator BlinkCor()
    {
        m_light.enabled = true;
        yield return new WaitForSeconds(m_blinkLengthSeconds);
        m_light.enabled = false;
    }
}
