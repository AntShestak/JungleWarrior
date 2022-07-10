using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NEW GUI SYSTEM
/// </summary>
public class GUISystem : MonoBehaviour
{
    public static GUISystem Instance { get; private set; }

    ////NOTE: this both can be united under the same name
    //[SerializeField] Canvas m_topGUI;
    //[SerializeField] Canvas m_weaponGUI;

    private WeaponGuiManager m_weaponGUI;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        m_weaponGUI = GetComponent<WeaponGuiManager>();
    }

    private void Start()
    {
        
    }

    #region Weapon GUI


    public void WeaponPanelSetByIndex(int index)
    {
        m_weaponGUI.SetWeaponPanelByIndex(index);
    }

    public void UpdateAmmoDisplay(int ammo, int m_maxAmmo)
    {
        
        m_weaponGUI.UpdateAmmo(ammo);
            
    }

    public void TransitionToSniperDisplay()
    {
        //IEnumerator cor
    }
    #endregion


}
