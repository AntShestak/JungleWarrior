using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
  

    [SerializeField] GameObject[] m_weapons;
    [SerializeField] Weapon.WeaponType m_defaultWeapon;

    private Animator m_anim;
    private PlayerScript m_player;
    private Weapon m_currentWeapon;

    private void Awake()
    {
        m_player = GetComponentInParent<PlayerScript>();
        m_anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Weaponse array length {m_weapons.Length}", this);
        SelectWeapon(m_defaultWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Weapon1"))
            SelectWeapon(Weapon.WeaponType.Primary);
        else if (Input.GetButtonDown("Weapon2"))
            SelectWeapon(Weapon.WeaponType.Secondary);
    }

    private void SelectWeapon(Weapon.WeaponType weapon)
    {
        if (m_currentWeapon != null)
        {
            if (weapon == m_currentWeapon.GetType()) return;
            m_currentWeapon.Unequip();
        }
            
        

        switch (weapon)
        {   
            
            case Weapon.WeaponType.Secondary:
                ActivateWeapon(1);
                break;
                
            default:
                ActivateWeapon(0);
                break;
        }
        
    }

    private void ActivateWeapon(int index)
    {
        Debug.Log($"Activating weapon at index {index}", this);
        //DeactivateAllWeapons();

        //IWeapon weaponInterface = m_weapons[index].GetComponent<IWeapon>();
        //m_weapons[index].SetActive(true);
        m_anim.SetInteger("WeaponType", index);
        Weapon newWeapon = m_weapons[index].GetComponent<Weapon>();
        
        m_currentWeapon = newWeapon;
        m_player.SetWeapon(m_currentWeapon);
    }

    private void DeactivateAllWeapons()
    {
        for(int i = 0; i < m_weapons.Length; i++)
            m_weapons[i].SetActive(false);
    }

    private void WeaponSwap()
    {
        
    }

    //IEnumerator
}
