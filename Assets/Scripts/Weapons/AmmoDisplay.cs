using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    private enum DisplayType { Slider, Count };

    [SerializeField] private DisplayType m_type;
    [SerializeField] private GameObject m_ammoHolder;
    [SerializeField] private Text m_ammoText;

    private Slider m_slider;

    private Image[] m_ammunition;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        if (m_type == DisplayType.Slider)
        {
            m_slider = GetComponentInChildren<Slider>();
        }
        else 
        {
            m_ammunition = m_ammoHolder.GetComponentsInChildren<Image>();
        }
    }

    private void OnEnable()
    {
        if (m_type == DisplayType.Slider && m_slider == null)
        {
            m_slider = GetComponentInChildren<Slider>();
        }
        else if (m_type == DisplayType.Count && m_ammunition == null)
        {
            m_ammunition = m_ammoHolder.GetComponentsInChildren<Image>();
        }
    }

    public void SetAmmo(int ammo)
    {
        //Debug.Log($"Setting Ammo to {ammo} ammunition length {m_ammunition.Length}");
        if (m_type == DisplayType.Slider)
        {
            
            m_slider.value = ammo;
            m_ammoText.text = $"{ammo}%";
        }
        else 
        {
            if (m_ammunition == null)
                Debug.Log("Ammunition is null");
            if (ammo > m_ammunition.Length)
            {
                //Debug.Log($"Image ammo display warning. Requested set to {ammo}");
            }
            else
            {
                //Debug.Log($"Total images {m_ammunition.Length}");
                for (int i = 0; i < m_ammunition.Length; i++)
                {
                    if (i < ammo)
                        m_ammunition[i].enabled = true;
                    else
                        m_ammunition[i].enabled = false;
                }
                
                m_ammoText.text = $"{ammo}";

                
            }
        }
    }
}
