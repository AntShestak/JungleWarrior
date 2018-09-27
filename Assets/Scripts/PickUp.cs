using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	public Canvas m_canvas; //personal canvas
	public int m_type = 0; //set type for pickup ( 1 -health, 2 - ammo)
	public Color m_active;
	public Color m_inactive;
	public float m_rechargeTime; //time this object takes to recharge

	PlayerScript m_player;
	AudioSource m_audio;
	ParticleSystem m_ps;
	bool m_isActive = true;

	// Use this for initialization
	void Start () 
	{
		//get player script access
		try
		{
			m_player = GameObject.FindObjectOfType<PlayerScript>();
		}
		catch (System.Exception e) 
		{
			Debug.Log (e.ToString ());
		}
		//get particle system reference
		m_ps = GetComponentInChildren<ParticleSystem>();
		//get audio
		m_audio = GetComponent<AudioSource>();
		//start rotation coroutine
		StartCoroutine("Rotation");
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		//only accept collision if active
		if (m_isActive) 
		{
			//check if it's player
			if (other.CompareTag ("Player")) 
			{
				//set inactive
				m_isActive = false;
				//play audio
				m_audio.Play();
				//check type
				switch (m_type) 
				{
				case 1: //health type
					m_player.AddHealth ();
					break;
				case 2: //ammo type
					m_player.AddAmmo ();
					break;
				default:
					Debug.Log ("Pick up not set!");
					break;
				}
				//start coroutine to recharge
				StartCoroutine("Recharge");
			}


		}

	}


	IEnumerator Recharge()
	{
		//stop particle system
		m_ps.Stop();
		//set canvas inactive
		m_canvas.enabled = false;
		StopCoroutine ("Rotation");
		//change color
		GetComponent<Renderer>().material.SetColor("_EmissionColor",m_inactive);
		//wait recharge time
		yield return new WaitForSeconds(m_rechargeTime);
		//start particle system
		m_ps.Play();
		//enable canvas
		m_canvas.enabled = true;
		StartCoroutine ("Rotation");
		//change color
		GetComponent<Renderer>().material.SetColor("_EmissionColor",m_active);
		//set active
		m_isActive = true;
	}

	IEnumerator Rotation()
	{
		while (true) 
		{
			m_canvas.transform.LookAt (Camera.main.transform);
			yield return new WaitForSeconds (0.1f);
		}
	}
}
