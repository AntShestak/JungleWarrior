using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjectScanner : MonoBehaviour
{
    /// <summary>
    /// Scan for objects on a pickable Layer
    /// </summary>
    
    public static PickableObjectScanner Instance { get; private set; }
    public GameObject CurrentPickableObject { get; private set; }

    private IEnumerator m_scanCoroutine;
    private float m_scanDistance = 2.5f;
    private int m_scannableObjectsCount = 0;

    private void Awake() {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        CurrentPickableObject = null;
    }

    public void StartScan() {
        if (m_scannableObjectsCount == 0)
        {
            //otherwise coroutine is already running
            m_scanCoroutine = ScanCoroutine();
            StartCoroutine(m_scanCoroutine);
        }
        m_scannableObjectsCount++;
        
    }

    public void StopScan()
    {
        m_scannableObjectsCount--;
        if (m_scannableObjectsCount == 0)
        {
            StopCoroutine(m_scanCoroutine);
            AimDisplayManager.Instance.DeactivatePickableInfo();
        }
        else if (m_scannableObjectsCount < 0)
        {
            Debug.Log($"Error. Scannable object count is below zero: {m_scannableObjectsCount}", this);
        }
        
    }

    IEnumerator ScanCoroutine()
    {

        Debug.Log("Scan for weapon initialised");

        WaitForSeconds waitSec = new WaitForSeconds(0.1f);

        RaycastHit hit;
        LayerMask mask = 1 << 13; //layer 13 --> "Pickable"

        while (true)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, m_scanDistance, mask, QueryTriggerInteraction.Ignore))
            {
                CurrentPickableObject = hit.collider.gameObject;;
                string info = "Unknown object";
                if (CurrentPickableObject.GetComponent<IPickable>().Type == IPickable.PickableType.Weapon)
                {
                    info = "Swap secondary weapon";
                }
               
                
                AimDisplayManager.Instance.ActivatePickableInfo(info);
            }
            else
            {
                CurrentPickableObject = null;
                AimDisplayManager.Instance.DeactivatePickableInfo();
            }
            yield return waitSec;
        }
    }

    
}
