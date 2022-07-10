using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRiffle : Weapon
{
    [SerializeField] GameObject m_sparks;
    [SerializeField] GameObject m_blood;
    

    private LightBlink m_light;
    private MeshRenderer m_rend;
    private bool m_isAimed;

    // Start is called before the first frame update
    protected override void WeaponSetupOnStart()
    {
        m_weaponName = "Range";
        m_type = WeaponType.SniperRiffle;
        m_isAimed = false;
        m_light = GetComponentInChildren<LightBlink>();
        m_rend = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta != Vector2.zero && m_isAimed)
        {
            AimDisplayManager.Instance.SetZoom(Input.mouseScrollDelta.y);
        }
    }

    public override void Aim()
    {
        if (m_isAimed)
            DeactivateAim();
        else
            ActivateAim();
    }

    private void ActivateAim() {
        m_anim.SetBool("RangeAim",true);
        AimDisplayManager.Instance.ActivateSniperAim(0.25f,Ammo);
        
        Invoke("DisableMeshRenderer", 0.25f);
        
        m_isAimed = true;
     }

    private void DeactivateAim()
    {
        AimDisplayManager.Instance.ActivateBasicAim();
        m_anim.SetBool("RangeAim", false);
        m_rend.enabled = true;
        m_isAimed = false;
        
        
    }

    public void DisableMeshRenderer() {
        m_rend.enabled = false;
    }

    public override void Unequip()
    {
        base.Unequip();
        //disable aim if necessary
        if (m_isAimed)
            DeactivateAim();
    }

    public override void DeductAmmo(int toDeduct)
    {
        base.DeductAmmo(toDeduct);

        if (m_isAimed)
            AimDisplayManager.Instance.SetAimedDisplayAmmo(Ammo);
    }

    protected override IEnumerator ShootCoroutine()
   {
        
        m_anim.SetTrigger(m_weaponName + "Shot");
        m_audio.PlayShootAudio(); //play sound
        m_light.Blink();

        LayerMask mask = ~( 1 << LayerMask.NameToLayer("Player") );
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, Camera.main.transform.forward), out hit, 100.0f,mask))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Instantiate(m_blood, hit.point,Quaternion.identity,hit.transform);
                hit.collider.GetComponent<SoldierController>().TakeDamage();
            }  
            else
                Instantiate(m_sparks, hit.point, Quaternion.identity);
        }
       

        yield return new WaitForSeconds(m_reloadSeconds);
        m_canShoot = true;
    }

}
