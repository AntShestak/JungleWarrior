using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	[SerializeField] IWeapon _weapon;

	
	[SerializeField] private float _damageCost = 0.3f; //cost to health  (default = 30%)
	[SerializeField] private float _healthRechargeRate = 0.01f;
	float m_health = 1f; //current health

	GameManager m_manager;
	CanvasManager m_canvas;
	PlayerVoice m_voice;

	// Use this for initialization
	void Start () 
	{

		_weapon = GetComponentInChildren<IWeapon>();
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

		//start regeneration coroutine which slowly regenerates health all game long
		StartCoroutine("RegenerateHealthCoroutine");
		
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

	public void ShootCall()
	{
		_weapon.Shoot();
	}

	public void AddAmmo()
	{
		_weapon.AddAmmo(100);
	}

	void TakeDamage()
	{
		//reduce health
		m_health -= _damageCost;
		//check if it's below 0
		if (m_health <= 0) 
		{
			//play audio 
			m_voice.Death();
			//further controlled by game manager script
			m_manager.GameOver();
		
		}
		else
			m_canvas.SetHealth(m_health, _damageCost); //adjust GUI
	}

	

	

	//sets full health
	public void AddHealth()
	{
		m_health = 1f;
		//adjust on canvas
		m_canvas.SetHealth(m_health,_damageCost);
	}

	IEnumerator RegenerateHealthCoroutine()
	{
		while (true)
		{
			//wait for 1 second
			yield return new WaitForSeconds(1f);
		
			//NOW HEALTH
			//add to health
			if (m_health <= _damageCost) //if less than one shot left regenerate hp 4x faster (adrenaline)
				m_health += _healthRechargeRate * 4;
			else
				m_health += _healthRechargeRate;
			//clamp ammo to 1 max
			m_health = Mathf.Clamp01(m_health);
			//adjust on canvas
			m_canvas.SetHealth(m_health, _damageCost);
		}

	}

	public void SetWeapon(IWeapon weapon)
	{
		_weapon = weapon;
	}

}
