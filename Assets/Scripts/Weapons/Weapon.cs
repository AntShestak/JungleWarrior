using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// Weapon base class
    /// </summary>
    /// 

    public int Ammo { get; set; }


    [SerializeField] private GameObject m_projectile;
    [SerializeField] private Transform m_bulletSpawn; //projectile spawn point
    [SerializeField] private Transform m_gunTransform;

    [SerializeField] private int m_maxAmmo = 100;
    [SerializeField] private int m_shotCost = 10; //1 shot costs 10% of full charge (by default)

    [SerializeField] private float m_reloadSeconds = 1f; //time between shots

    //not every weapon recharges (but they do for now)
    [SerializeField] private int m_blasterRechargeRate = 1; //per second

    private CanvasManager m_canvas;
    private GunAudio m_audio;
    private Animator m_anim;

    private bool m_canShoot = true;


    private void Awake()
    {
        m_canvas = FindObjectOfType<CanvasManager>();

        m_audio = GetComponent<GunAudio>();

        m_anim = GetComponent<Animator>();
    }
    private void Start()
    {
        Ammo = m_maxAmmo;

        StartCoroutine("RegenerateAmmoCoroutine");

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


    //simple coroutine that shoots a bullet and controls fire rate
    protected virtual IEnumerator ShootCoroutine()
    {

        Instantiate(m_projectile, m_bulletSpawn.position, m_gunTransform.rotation);
        DeductAmmo(m_shotCost);
        m_anim.SetTrigger("Shoot");
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

    public virtual void Equip()
    {
        this.gameObject.SetActive(true);
        m_anim.SetTrigger("Equip");
    }

    public virtual void Unequip()
    {
        m_anim.SetTrigger("Unequip");

    }
}
