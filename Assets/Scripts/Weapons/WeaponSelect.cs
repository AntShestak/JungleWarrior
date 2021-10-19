using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public enum WeaponType { Pistol, GrenadeLauncher }

    //[SerializeField] GameObject m_weapon1;
    //[SerializeField] GameObject m_weapon2;

    [SerializeField] GameObject[] m_weapons;

    PlayerScript m_player;

    private WeaponType m_currentWeapon = WeaponType.Pistol;

    private void Awake()
    {
        m_player = GetComponentInParent<PlayerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Weaponse array length {m_weapons.Length}", this);
        ActivateWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Weapon1"))
            SelectWeapon(WeaponType.Pistol);
        else if (Input.GetButtonDown("Weapon2"))
            SelectWeapon(WeaponType.GrenadeLauncher);
    }

    private void SelectWeapon(WeaponType weapon)
    {
        if (weapon == m_currentWeapon) return;

        switch (weapon)
        {
            case WeaponType.GrenadeLauncher:
                m_currentWeapon = weapon;
                ActivateWeapon(1);
                break;
                
            default:
                m_currentWeapon = weapon;
                ActivateWeapon(0);
                break;
        }
        
    }

    private void ActivateWeapon(int index)
    {
        Debug.Log($"Activating weapon at index {index}", this);
        DeactivateAllWeapons();

        //IWeapon weaponInterface = m_weapons[index].GetComponent<IWeapon>();
        m_weapons[index].SetActive(true);
        m_player.SetWeapon(m_weapons[index].GetComponent<Weapon>());
    }

    private void DeactivateAllWeapons()
    {
        for(int i = 0; i < m_weapons.Length; i++)
            m_weapons[i].SetActive(false);
    }
}
