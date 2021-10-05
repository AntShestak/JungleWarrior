using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour, IWeapon
{
    public int Ammo { get; set; }

    
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _bulletSpawn; //projectile spawn point
    [SerializeField] private Transform _gunTransform;

    [SerializeField] private int _maxAmmo = 100;
    [SerializeField] private int _shotCost = 1; //1 shot costs 10% of full charge (by default)

    [SerializeField] private float _reloadSeconds = 1f; //time between shots
    
    [SerializeField] private int _blasterRechargeRate = 1; //per second
     
    

  
    private CanvasManager _canvas;
    private GunAudio _audio;
    private Animator _anim;
    private bool _canShoot = true;


    private void Awake()
    {
        _canvas = FindObjectOfType<CanvasManager>();

        _audio = GetComponent<GunAudio>();

        _anim = GetComponent<Animator>();
    }
    private void Start()
    {
        Ammo = _maxAmmo;

        StartCoroutine("RegenerateAmmoCoroutine");

    }

    public void AddAmmo(int toAdd)
    {
        Ammo += toAdd;
       
        Ammo = Mathf.Clamp(Ammo, 0, _maxAmmo);
        
        _canvas.UpdateAmmoDisplay(Ammo,_maxAmmo);
    }

    public void DeductAmmo(int toDeduct)
    {
        Ammo -= toDeduct;

        Ammo = Mathf.Clamp(Ammo, 0, _maxAmmo);

        _canvas.UpdateAmmoDisplay(Ammo,_maxAmmo);
    }

    public void Shoot()
    {
        if (_canShoot)
        {
            if (Ammo >= _shotCost)
            { 
                _canShoot = false;
                StartCoroutine("ShootCoroutine");
            }   
            else
                _audio.PlayShootFail();
        }

    }  
     

    //simple coroutine that shoots a bullet and controls fire rate
    IEnumerator ShootCoroutine()
    {
        
        Instantiate(_projectile, _bulletSpawn.position, _gunTransform.rotation);
        DeductAmmo(_shotCost);
        _anim.SetTrigger("Shoot");
        _audio.PlayShootAudio(); //play sound
        yield return new WaitForSeconds(_reloadSeconds);
        _canShoot = true;
    }

    IEnumerator RegenerateAmmoCoroutine()
    {
        while (true)
        {
            //wait for 1 second
            yield return new WaitForSeconds(1f);
            //SORT OUT AMMO FIRST
            //add to ammo
            AddAmmo( _blasterRechargeRate);
           
           
           
        }

    }

    
}

           