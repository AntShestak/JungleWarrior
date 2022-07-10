using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    private int m_regenRate = 2; //per sec
    private bool m_isAimed = false;

    protected override void WeaponSetupOnStart()
    {
        m_weaponName = "Pistol";
        m_type = WeaponType.Pistol;
    }

    protected override void RegenerateAmmo()
    {
        StartCoroutine("RegenerateAmmoCoroutine");
    }

    protected virtual IEnumerator RegenerateAmmoCoroutine()
    {
        WaitForSeconds waitTimeSec = new WaitForSeconds(1f);
        while (true)
        {
            //wait for 1 second
            yield return waitTimeSec;
            //SORT OUT AMMO FIRST
            //add to ammo
            AddAmmo(m_regenRate);



        }

    }
    public override void Unequip()
    {
        
        //disable aim if necessary
        if (m_isAimed)
            DeactivateAim();

        base.Unequip();
    }

    public override void Aim()
    {
        if (m_isAimed)
            DeactivateAim();
        else
            ActivateAim();
    }

    void ActivateAim()
    {

        CameraZoomManager.Instance.SetPistolZoom();
        m_anim.SetBool("PistolAim", true);
        m_isAimed = true;
    }

    void DeactivateAim()
    {
        CameraZoomManager.Instance.SetDefaultZoom();
        m_anim.SetBool("PistolAim", false);
        m_isAimed = false;
    }
}
