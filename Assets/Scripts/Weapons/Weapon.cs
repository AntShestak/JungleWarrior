using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// Weapon base class
    /// </summary>
    /// 

    public enum WeaponType { Primary = 0, Secondary };

    public int Ammo { get; set; }


    [SerializeField] private GameObject m_projectile;
    [SerializeField] private Transform m_bulletSpawn; //projectile spawn point
    [SerializeField] private Transform m_gunTransform;

    [SerializeField] private int m_maxAmmo = 100;
    [SerializeField] private int m_shotCost = 10; //1 shot costs 10% of full charge (by default)

    [SerializeField] private float m_reloadSeconds = 1f; //time between shots

    //not every weapon recharges (but they do for now)
    [SerializeField] private int m_blasterRechargeRate = 1; //per second

    protected string m_weaponName;
    protected WeaponType m_type;
    private CanvasManager m_canvas;
    private GunAudio m_audio;
    private Animator m_anim;

    private bool m_canShoot = true;


    private void Awake()
    {
        m_canvas = FindObjectOfType<CanvasManager>();

        m_audio = GetComponent<GunAudio>();

        m_anim = GetComponentInParent<Animator>();
    }
    private void Start()
    {
        Ammo = m_maxAmmo;

        StartCoroutine("RegenerateAmmoCoroutine");

        SetName();

    }

    public virtual void AddAmmo(int toAdd)
    {
        Ammo += toAdd;

        Ammo = Mathf.Clamp(Ammo, 0, m_maxAmmo);

        m_canvas.UpdateAmmoDisplay(Ammo, m_maxAmmo);
    }

    public virtual void DeductAmmo(int toDeduct)
    {
        Ammo -= toDeduct;

        Ammo = Mathf.Clamp(Ammo, 0, m_maxAmmo);

        m_canvas.UpdateAmmoDisplay(Ammo, m_maxAmmo);
    }

    public virtual void Shoot()
    {
        if (m_canShoot)
        {
            if (Ammo >= m_shotCost)
            {
                m_canShoot = false;
                IEnumerator shootCoroutine = ShootCoroutine();
                StartCoroutine(shootCoroutine);
            }
            else
                m_audio.PlayShootFail();
        }

    }


    protected virtual void SetName()
    {
        m_weaponName = "Pistol";
    }

    public virtual void Equip()
    {
        //this.gameObject.SetActive(true);
        //IEnumerator equipCoroutine = EquipCoroutine();
        //StartCoroutine(equipCoroutine);
    }

    public virtual void Unequip()
    {
        m_anim.SetTrigger(m_weaponName + "Unequip");
        m_audio.PlayEquip();

    }

    //simple coroutine that shoots a bullet and controls fire rate
    protected virtual IEnumerator ShootCoroutine()
    {

        Instantiate(m_projectile, m_bulletSpawn.position, m_gunTransform.rotation);
        DeductAmmo(m_shotCost);
        m_anim.SetTrigger(m_weaponName + "Shot");
        m_audio.PlayShootAudio(); //play sound
        yield return new WaitForSeconds(m_reloadSeconds);
        m_canShoot = true;
    }

    protected virtual IEnumerator RegenerateAmmoCoroutine()
    {
        while (true)
        {
            //wait for 1 second
            yield return new WaitForSeconds(1f);
            //SORT OUT AMMO FIRST
            //add to ammo
            AddAmmo(m_blasterRechargeRate);



        }

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
