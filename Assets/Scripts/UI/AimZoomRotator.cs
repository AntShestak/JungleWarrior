using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AimZoomRotator : MonoBehaviour
{
    [SerializeField] private float m_minRotationZ;
    [SerializeField] private float m_maxRotationZ;
    [SerializeField] private bool m_rotateClockwise;

    private RectTransform m_trans;

    private void Awake()
    {
        m_trans = GetComponent<RectTransform>();
        
    }

    private void Start()
    {
        if (!m_rotateClockwise)
            m_maxRotationZ *= -1;
    }

    public void Rotate(float rate)
    {

        float rotZ = Mathf.Lerp(m_minRotationZ, m_maxRotationZ, rate);
        m_trans.rotation = Quaternion.Euler(0, 0, rotZ);
        
    }

}
