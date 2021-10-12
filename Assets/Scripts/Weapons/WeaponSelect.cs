using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public enum Weapon { Pistol, Blaster }

    //[SerializeField] GameObject m_weapon1;
    //[SerializeField] GameObject m_weapon2;

    [SerializeField] GameObject[] m_weapons;

    PlayerScript m_player;

    private Weapon m_currentWeapon = Weapon.Pistol;

    private void Awake()
    {
        m_player = GetComponentInParent<PlayerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Weaponse array length {m_weapons.Length}", this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Weapon1"))
            SelectWeapon(Weapon.Pistol);
        else if (Input.GetButtonDown("Weapon2"))
            SelectWeapon(Weapon.Blaster);
    }

    private void SelectWeapon(Weapon weapon)
    {
        if (weapon == m_currentWeapon) return;

        switch (weapon)
        {
            case Weapon.Blaster:
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
        DeactivateAllWeapons();
        m_weapons[index].SetActive(true);
        m_player.SetWeapon(m_weapons[index].GetComponent<IWeapon>());
    }

    private void DeactivateAllWeapons()
    {
        for(int i = 0; i < m_weapons.Length; i++)
            m_weapons[i].SetActive(false);
    }
}
