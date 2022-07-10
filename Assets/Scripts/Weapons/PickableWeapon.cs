using UnityEngine;

public class PickableWeapon : MonoBehaviour, IPickable
{
    public IPickable.PickableType Type { get; set; } = IPickable.PickableType.Weapon;

    [SerializeField] private Weapon.WeaponType m_weaponType;
    public Weapon.WeaponType WeaponType { 
        get { return m_weaponType; } 
        set { m_weaponType = value; } 
    }

    
    [SerializeField] private int m_ammo;

    public int Ammo {
        get { return m_ammo; }
        set { m_ammo = value; }
    }




    
}
