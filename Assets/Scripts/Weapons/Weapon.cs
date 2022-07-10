using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// Weapon base class
    /// </summary>
    /// 

    public enum WeaponType { Pistol, GrenadeLauncher, SniperRiffle };

    private int m_ammo;
    public int Ammo { 
        get {
            return m_ammo;
        }
        set {
            m_ammo = value;
            if (GUISystem.Instance != null)
                GUISystem.Instance.UpdateAmmoDisplay(Ammo, m_maxAmmo);
        } 
    }
    public WeaponType Type { 
        get { return m_type; } 
    }


    [SerializeField] private GameObject m_projectile;
    [SerializeField] private Transform m_projectileSpawn; //projectile spawn point
    [SerializeField] private Transform m_gunTransform;

    [SerializeField] private int m_maxAmmo = 0;
    [SerializeField] private int m_shotCost = 0; 

    [SerializeField] protected float m_reloadSeconds = 0f; //time between shots

    protected string m_weaponName;
    protected WeaponType m_type;
    protected Animator m_anim;
    //private CanvasManager m_canvas;
    protected GunAudio m_audio;
    

    protected bool m_canShoot = true;


    private void Awake()
    {
        //m_canvas = FindObjectOfType<CanvasManager>();

        m_audio = GetComponent<GunAudio>();

        m_anim = GetComponentInParent<Animator>();
    }
    private void Start()
    {
        Ammo = m_maxAmmo;

        RegenerateAmmo();

        WeaponSetupOnStart();

    }

    public virtual void AddAmmo(int toAdd)
    {

        Ammo += toAdd;

        Ammo = Mathf.Clamp(Ammo, 0, m_maxAmmo);

        GUISystem.Instance.UpdateAmmoDisplay(Ammo, m_maxAmmo);

    }

    public virtual void Aim()
    {
        
    }


    public virtual void DeductAmmo(int toDeduct)
    {
        //Debug.Log($"Deducting ammo {toDeduct}");

        Ammo -= toDeduct;

        Ammo = Mathf.Clamp(Ammo, 0, m_maxAmmo);

        GUISystem.Instance.UpdateAmmoDisplay(Ammo, m_maxAmmo);
    }

    public virtual void Shoot()
    {
        
        if (m_canShoot)
        {
            if (Ammo >= m_shotCost)
            {
                
                m_canShoot = false;
                DeductAmmo(m_shotCost);

                IEnumerator shootCoroutine = ShootCoroutine();
                StartCoroutine(shootCoroutine);
            }
            else
                m_audio.PlayShootFail();
        }

    }


    protected virtual void WeaponSetupOnStart()
    {
        m_weaponName = "Pistol";
    }

    protected virtual void RegenerateAmmo()
    {
        
    }

    public virtual void Equip()
    {
        if (m_weaponName == "Range" || m_weaponName == "Pistol")
            m_anim.SetBool(m_weaponName + "Equip", true);
    }

    public virtual void Unequip()
    {
        if (m_weaponName == "Range" || m_weaponName == "Pistol")
            m_anim.SetBool(m_weaponName + "Equip", false);
        else
            m_anim.SetTrigger(m_weaponName + "Unequip");
        m_audio.PlayEquip();

    }

    //simple coroutine that shoots a bullet and controls fire rate
    protected virtual IEnumerator ShootCoroutine()
    {
        Debug.Log("Base call");
        Instantiate(m_projectile, m_projectileSpawn.position, m_gunTransform.rotation);
        
        m_anim.SetTrigger(m_weaponName + "Shot");
        m_audio.PlayShootAudio(); //play sound
        yield return new WaitForSeconds(m_reloadSeconds);
        m_canShoot = true;
    }

    

    public WeaponType GetType()
    {
        return m_type;
    }

    //protected virtual IEnumerator EquipCoroutine()
    //{
    //    //m_anim.SetTrigger(m_WeaponName + "Equip");
    //    bool isAnimPlaying = true;
    //    while (isAnimPlaying)
    //    {
    //        yield return 0;
    //        AnimatorTransitionInfo info = m_anim.GetAnimatorTransitionInfo(0);
    //        Debug.Log(info.ToString());
    //        isAnimPlaying = false;
    //    }
    //}
}
