using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimDisplayManager : MonoBehaviour
{
    public static AimDisplayManager Instance { get; private set; }

    [SerializeField] GameObject m_basicAim;
    [SerializeField] GameObject m_sniperAim;
    [SerializeField] Text m_infoField;
    [SerializeField] AmmoDisplay m_secondaryAmmoDisplay;

    private WeaponGuiManager m_weaponGuiMan;
    private CameraZoomManager m_camZoom;
    private AimZoomRotator[] m_uiRotators;
    

    private float m_maxZoomRate = 4.0f; //4x
    private float m_minZoomRate = 1.0f;
    private float m_currentZoom;
    private float m_zoomSpeed = 0.1f;

    
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        m_weaponGuiMan = GetComponent<WeaponGuiManager>();
        m_camZoom = Camera.main.GetComponent<CameraZoomManager>();
        m_uiRotators = GetComponentsInChildren<AimZoomRotator>(true);

        Debug.Log($"Rotators total {m_uiRotators.Length}",this);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_currentZoom = m_minZoomRate;
    }

    public void ActivateBasicAim()
    {
        m_sniperAim.SetActive(false);
        m_basicAim.SetActive(true);
        m_camZoom.SetDefaultZoom();
    }

    public void ActivateSniperAim(float delaySeconds, int ammo)
    {
        if (m_basicAim.activeSelf)
        {
            IEnumerator activateSniper = ActivateSniperAimCor(delaySeconds, ammo);
            StartCoroutine(activateSniper);
        }
    }
        

    IEnumerator ActivateSniperAimCor(float delay, int ammo)
    {
        //GUISystem.Instance.TransitionToSniperDisplay();
        yield return new WaitForSeconds(delay); 
        m_basicAim.SetActive(false);
        m_sniperAim.SetActive(true);
        SetAimedDisplayAmmo(ammo);
        m_currentZoom = m_minZoomRate;
        SetZoom(0);

    }

    public void ActivatePickableInfo(string info)
    {
        if (m_basicAim.activeSelf)
        {
            m_basicAim.SetActive(false);

            m_infoField.text = info;
            m_infoField.gameObject.SetActive(true);
        }
        
    }

    public void DeactivatePickableInfo()
    {
        if (m_infoField.gameObject.activeSelf)
        {
            m_basicAim.SetActive(true);

            m_infoField.gameObject.SetActive(false);
        }
        
    }

    public void SetZoom(float value)
    {
        m_currentZoom = Mathf.Clamp(m_currentZoom + value * m_zoomSpeed, m_minZoomRate, m_maxZoomRate);
        //Debug.Log($"ADM | Current zoom: {m_currentZoom}");
        float rate = (m_currentZoom - m_minZoomRate ) / (m_maxZoomRate - m_minZoomRate);
        //Debug.Log($"ADM | Current zoom RATE is: {rate}");
        
        m_camZoom.SetSniperZoom(rate);
        foreach (AimZoomRotator rot in m_uiRotators)
        {
            rot.Rotate(rate);
        }
    }

    public void SetAimedDisplayAmmo(int ammo)
    {
        m_secondaryAmmoDisplay.SetAmmo(ammo);
    }
}
