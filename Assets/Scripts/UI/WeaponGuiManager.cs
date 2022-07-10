using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGuiManager : MonoBehaviour
{
    [SerializeField] Animator m_anim;
    [SerializeField] GameObject[] m_weaponAmmoPanels;

    private AmmoDisplay m_display;

    public void SetWeaponPanelByIndex(int index)
    {
        //Debug.Log("Weapon panel set", this);

        IEnumerator cor = SwapPanelCor(index);
        StartCoroutine(cor);
        

    }

    IEnumerator SwapPanelCor(int index)
    {
        m_anim.SetBool("ShowGUI",false);
        yield return new WaitForSeconds(1.25f);
        DeactivateAll();
        m_weaponAmmoPanels[index].SetActive(true);
        m_display = m_weaponAmmoPanels[index].GetComponent<AmmoDisplay>();
        m_anim.SetBool("ShowGUI",true);

    }

    private void DeactivateAll()
    {
        foreach (var panel in m_weaponAmmoPanels)
        {
            panel.SetActive(false);
        }
    }

    public void UpdateAmmo(int ammo)
    {
        
        if (m_display )
        {
            m_display.SetAmmo(ammo);
        }
    
    }

}
