using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	//public LayerMask m_mask;
	public GameObject m_projectile;
	public Transform m_bulletSpawn; //projectile spawn point
	public Transform m_gunTransform;
	public float m_reloadTime = 1f; //time between shots
	public float m_shotCost = 0.1f; //1 shot costs 10% of full charge (by default)
	public float m_gunRechargeRate = 0.01f; //recharges 1% every second 
	public float m_damageCost= 0.3f; //cost to health  (default = 30%) 
	public float m_healthRechargeRate = 0.01f; 

	float m_ammo = 1f; //current ammo
	float m_health = 1f; //current health
	bool m_canShoot = true;

	GameManager m_manager;
	CanvasManager m_canvas;
	Animator m_anim;
	Camera m_cam;
	GunAudio m_audio;
	PlayerVoice m_voice;

	// Use this for initialization
	void Start () 
	{
		m_cam = GetComponentInChildren<Camera> ();

		m_anim = GetComponentInChildren<Animator> ();

		m_audio = GetComponent<GunAudio> ();

		m_voice = GetComponent<PlayerVoice> ();

		try
		{
			m_canvas = GameObject.FindObjectOfType<CanvasManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("PlayerScript can't find CanvasManager component! " + e.ToString ());
		}
		//try to get GameManager reference
		try
		{
			m_manager = GameObject.FindObjectOfType<GameManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("PlayerScript can't find GameManager component! " + e.ToString ());
		}

		//start regeneration coroutine which slowly recharges health and ammo all game long
		StartCoroutine("Regenerate");
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Enemy")) 
		{
			TakeDamage ();
			//play audio
			m_voice.Hurt ();
		}
			
			
	}

	void TakeDamage()
	{
		//reduce health
		m_health -= m_damageCost;
		//check if it's below 0
		if (m_health <= 0) 
		{
			//play audio 
			m_voice.Death();
			//further controlled by game manager script
			m_manager.GameOver();
		
		}
		else
			m_canvas.SetHealth(m_health,m_damageCost); //adjust GUI
	}
		
	//controls the shot (called from input manager)
	public void ShootCall()
	{
		//check if reloaded
		if (m_canShoot) 
		{
			//check if enough ammo
			if(m_ammo >= m_shotCost)
			{
				m_canShoot = false;
				StartCoroutine ("Shoot");
				//initialise projectile4
				Instantiate(m_projectile,m_bulletSpawn.position,m_gunTransform.rotation);
			}
			else //not enough ammo
				m_audio.PlayShootFail();
		}


	}

	//simple coroutine that shoots a bullet and controls fire rate
	IEnumerator Shoot()
	{
		//deduct ammo
		m_ammo -= m_shotCost;
		m_canvas.SetAmmo (m_ammo);
		m_anim.SetTrigger ("Shoot");
		m_audio.PlayShootAudio (); //play sound
		yield return new WaitForSeconds (m_reloadTime);
		m_canShoot = true;
	}

	IEnumerator Regenerate()
	{
		while (true) 
		{
			//wait for 1 second
			yield return new WaitForSeconds (1f);
			//SORT OUT AMMO FIRST
			//add to ammo
			m_ammo += m_gunRechargeRate;
			//clamp ammo to 1 max
			m_ammo = Mathf.Clamp01(m_ammo);
			//adjust on canvas
			m_canvas.SetAmmo(m_ammo);
			//NOW HEALTH
			//add to health
			if(m_health <= m_damageCost) //if less than one shot left regenerate hp 4x faster (adrenaline)
				m_health += m_healthRechargeRate * 4;
			else
				m_health += m_healthRechargeRate;
			//clamp ammo to 1 max
			m_health = Mathf.Clamp01(m_health);
			//adjust on canvas
			m_canvas.SetHealth(m_health,m_damageCost);
		}

	}

	//sets full health
	public void AddHealth()
	{
		m_health = 1f;
		//adjust on canvas
		m_canvas.SetHealth(m_health,m_damageCost);
	}

	//sets full ammo
	public void AddAmmo()
	{
		m_ammo = 1f;
		//adjust on canvas
		m_canvas.SetAmmo(m_ammo);
	}
}
