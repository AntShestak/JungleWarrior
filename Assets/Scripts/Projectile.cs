using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public GameObject m_explosion;
	public int m_type = 0; //1 for player, 2 for enemy
	public float m_speed; 
	public float m_range;

	Rigidbody m_rigid;
	Vector3 m_startPos;

	// Use this for initialization
	void Start() 
	{
		m_rigid = GetComponent<Rigidbody> ();
		//save start position
		m_startPos = transform.position;
		//if it's a player
		switch (m_type) 
		{
		case 1: //player
			m_rigid.AddForce (Camera.main.transform.forward * m_speed);
			break;
		case 2: //enemy
			//rotate projectile to face player so the transform.forward.points to the right direction 
			//(enemy weapon points slightly up, and on the distance projectile misses player)
			Transform trans = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
			transform.LookAt (trans);
			m_rigid.AddForce (transform.forward * m_speed);
			break;
		default:
			Debug.Log ("Projectile type not assigned!");
			break;
		}
			
	}

	void Update()
	{
		float x = Vector3.Distance (m_startPos, transform.position);
		if (x > m_range)
			DestroyProjectile ();
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log (other.tag.ToString ());
		DestroyProjectile ();
	}

	void DestroyProjectile()
	{
		//offset spawn position a little bit backwards, so the explosion doen't appear inside of object
		Vector3 pos;
		switch (m_type) 
		{
		case 1://player
			//calculate direction of movement
			Vector3 dir = transform.position - m_startPos;
			//normalize
			dir.Normalize ();
			dir *= 0.69f;
			pos = transform.position - dir;
			break;
		case 2:
			pos = transform.position - transform.forward * 0.69f; //69% of transform.forward
			break;
		default:
			pos = transform.position;
			break;
		}

		//instantiate explosion
		Instantiate (m_explosion, pos, transform.rotation);

		//destroy this projectile
		Destroy(this.gameObject);
	}
	

}
