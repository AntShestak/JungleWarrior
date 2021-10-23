using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    protected override void SetName()
    {
        m_weaponName = "Pistol";
        m_type = WeaponType.Primary;
    }
}
