using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapon
{


    protected override void WeaponSetupOnStart()
    {
        m_weaponName = "Secondary";
        m_type = WeaponType.GrenadeLauncher;
    }


}
