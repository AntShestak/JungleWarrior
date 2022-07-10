using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entry to weapon selection system.
/// 
/// Comunicates with:
///     Player Controller 
///     GUI System
///     Input System
/// </summary>

public class WeaponSelect : MonoBehaviour
{

    [SerializeField] GameObject[] m_weapons;
    [SerializeField] Weapon.WeaponType m_defaultWeapon;
    [SerializeField] WeaponGuiManager m_guiMan;

    private Animator m_anim;
    private PlayerScript m_player;

    private Weapon m_currentWeapon;
    //private int m_primaryWeaponIndex = 0; //indicates which of the is equiped for primary slot
    //private int m_secondaryWeaponIndex = 2; //indicates which of the is equiped for secondary slot
    //private int? m_currentWeaponIndex = null; //default weapon index is

    //0 for gun, and 1 for launcher
    private int[] m_weaponSlots = new int[] { 0, 2 };
    private int m_currentSlotUsed = 0; //by default first slot 

    private void Awake()
    {
        m_player = GetComponentInParent<PlayerScript>();
        m_anim = GetComponent<Animator>();

    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log($"Weaponse array length {m_weapons.Length}", this);
        //SelectWeapon(m_weaponSlotAssignedIndexes[m_currentSlotIndexUsed]);
        ActivateWeapon(m_weaponSlots[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Weapon1"))
            UseWeaponSlot(0);
        else if (Input.GetButtonDown("Weapon2"))
            UseWeaponSlot(1);
        
    }

    private void UseWeaponSlot(int slotIndex)
    {

        //select weapon only responds to changing currently used slot
        if (m_currentSlotUsed == slotIndex) return;
        
        m_currentSlotUsed = slotIndex;
        m_currentWeapon.Unequip();
        ActivateWeapon(m_weaponSlots[slotIndex]);

    }

    public Dictionary<string,int> SwapCurrentWeapon(PickableWeapon pickedWeapon)
    {
        //setup return values for object to be dropped
        var ret = new Dictionary<string, int>();
        ret["index"] = m_weaponSlots[m_currentSlotUsed]; //which weapon to drop
        ret["ammo"] = m_currentWeapon.Ammo; //retrieve ammo stats

        //sitch weapons
        switch (pickedWeapon.WeaponType)
        {
            case Weapon.WeaponType.GrenadeLauncher:
                m_weaponSlots[m_currentSlotUsed] = 1;
                break;
            case Weapon.WeaponType.SniperRiffle:
                m_weaponSlots[m_currentSlotUsed] = 2;
                break;
            default:
                //get pistol by default
                m_weaponSlots[m_currentSlotUsed] = 0;
                break;
        }

        m_currentWeapon.Unequip();
        //Debug.Log($"Picking up weapon with ammo {weapon.Ammo}");
        ActivateWeapon(m_weaponSlots[m_currentSlotUsed], pickedWeapon.Ammo);

        return ret;


    }

    private void ActivateWeapon(int index, int? ammo=null)
    {


        AnimateWeaponActivation(index);
        IEnumerator cor = ActivateWeaponOnPlayer(index, ammo);
        StartCoroutine(cor);

    }

    private IEnumerator ActivateWeaponOnPlayer(int index, int? ammo=null)
    {
        Weapon newWeapon = m_weapons[index].GetComponent<Weapon>();
        

        m_currentWeapon = newWeapon;
        //still to Implement Bool system for GrenadeLauncher
        if (index != 1)
            newWeapon.Equip();
        m_player.SetWeapon(m_currentWeapon);

        yield return new WaitForSeconds(0.25f);
        //m_currentWeaponIndex = index;
        if (ammo != null)
            newWeapon.Ammo = (int)ammo;
    }

    private void AnimateWeaponActivation(int index)
    {
        Debug.Log($"Activating weapon at index {index}", this);
        //DeactivateAllWeapons();
        m_guiMan.SetWeaponPanelByIndex(index);

        //IWeapon weaponInterface = m_weapons[index].GetComponent<IWeapon>();
        //m_weapons[index].SetActive(true);
        m_anim.SetInteger("WeaponType", index);
    }

    private void DeactivateAllWeapons()
    {
        for (int i = 0; i < m_weapons.Length; i++)
            m_weapons[i].SetActive(false);
    }

    //public void SetSecondaryWeapon(PickableWeapon weapon)
    //{
    //    m_secondaryWeaponIndex = weapon.SecondaryIndex;
    //}

    

    
}
