using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapon
{
    

    protected override void SetName()
    {
        m_weaponName = "Secondary";
        m_type = WeaponType.Secondary;
    }


}
