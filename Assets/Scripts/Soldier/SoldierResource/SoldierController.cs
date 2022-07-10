using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class SoldierController : MonoBehaviour
{
	public GameObject m_projectile;
	public float m_rangeShoot = 25f; //enemy's shooting range
	public float m_rangeVision = 60f; //vision range
	public Transform m_gunBone;
	public GameObject m_gun;

	SceneController m_control; 
	SoldierNavigator m_navigator;
	SoldierAudioManager m_audio;
	Actions m_actions;
	Transform m_playerTrans;
	Animator animator;
	int m_health = 3; //enemy can take three hits
	float m_visionAngle = 50f; //enemy can see 50 (value of this variable) degrees to the right and to the left
	bool m_found = false; //this bool will indicate if the player was already spotted
	bool m_dead = false;


	void Start()
	{ 
		//try to obtain SceneController reference
		try
		{
			m_control = GameObject.FindObjectOfType<SceneController>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("SoldierController can't find SceneController! " + e.ToString ());
		}

		animator = GetComponent<Animator> ();
		//GetActions script reference
		m_actions = GetComponent<Actions>();
		//get navigator script
		m_navigator = GetComponent<SoldierNavigator>();
		//get audio source
		m_audio = GetComponent<SoldierAudioManager>();
		//setup weapon to use
		GameObject newRightGun = (GameObject) Instantiate(m_gun);
		newRightGun.transform.parent = m_gunBone;
		newRightGun.transform.localPosition = new Vector3 (0, 0.008f, 0.029f);
		newRightGun.transform.localRotation = Quaternion.Euler(-1.956f, -93.97f, 3.72f);

		//start patroling
		StartPatrol();
	}
		
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Bullet") && !m_dead) //"bullet" is player projectile tag & if dead don't accept collisions
		{
				TakeDamage ();
				
		}
		else if (other.CompareTag("Explosion") && !m_dead)
		{
			DeadFromExplosion(other.gameObject.transform.position);
		}
			
	}

	public void TakeDamage()
	{
		//reduce health
		m_health -= 1;
		//check helath
		if (m_health <= 0)
		{ 
			Dead();
			return;
		}
		//check if player was already spotted
		if (!m_found)
			m_found = true;
		//play damage animation
		m_actions.Damage ();
		//hurt sound
		m_audio.Hurt();

	}

	void Dead()
	{	
		//set control bool
		m_dead = true;
		//sop patrol
		m_navigator.PatrolStop();
		//stop all coroutines
		StopAllCoroutines();
		//play death animation
		m_actions.Death ();
		//Destroy (this.gameObject);
		m_audio.Death();
		//set collider to match 
		Invoke("ChangeHeight", 0.5f);
		//report death of the soldier
		m_control.EnemyDown();

	}

	void DeadFromExplosion(Vector3 explosionPosition)
	{
		//set control bool
		m_dead = true;
		//sop patrol
		m_navigator.PatrolStop();
		//stop all coroutines
		StopAllCoroutines();

		m_navigator.DisableAgent();
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.mass = 80f;
		rb.useGravity = true;
		GetComponent<Animator>().enabled = false;
		rb.AddExplosionForce(1000f, explosionPosition, 2.75f);

		//Destroy (this.gameObject);
		m_audio.Death();

		//set collider to match 
		Invoke("ChangeHeight", 0.5f);
		//report death of the soldier
		m_control.EnemyDown();
	}

	void ChangeHeight()
	{
		//make collider smaller to fit dead enemy
		CapsuleCollider col = GetComponent<CapsuleCollider> ();
		col.height = 0.25f;
		col.radius = 0.25f;
		col.center = new Vector3 (0f, 0f, 0f);
	}
		

	void CreateProjectile()
	{
		//calculate position
		Vector3 pos = m_gunBone.position + transform.forward;
	
		Instantiate (m_projectile, pos, transform.rotation);
	}

	//======================= AI ++++++++++++++++++++++++++++
	void StartPatrol()
	{
		//set navigator
		m_navigator.PatrolStart ();
		//start walking animation
		m_actions.Walk();

		StartCoroutine ("Patrol");

	}
		
	IEnumerator Patrol() 
	{
		m_found = false;

		while (!m_found) 
		{
			//scan for player 10 times per sec
			m_found = ScanForPlayer ();
			yield return new WaitForSeconds (0.1f); 
		}
		//player found
		StartCoroutine ("Shoot");
	}

	IEnumerator Shoot()
	{
		m_navigator.PatrolStop (); //stop navigator movement
		m_actions.Aiming();
		//shoot sound ("fell my wrath" (german))
		m_audio.Shout();
		//shooting timer
		//creating shooting pattern where soldier shoots few times (round) & having pauses between rounds
		int counter = 0; //this counter defines frames between shots
		int shots = 0; //this counter is for pause after every round
		int maxFrames = 10; //the next loop updates 10 times per second, so shoot every 10th frame (1s)
		int maxShots = 3;

		//if player is in shooting range and is visible to soldier
		while (IsInRange (m_rangeShoot))// this gives an endless loop ? ==>  && IsVisible()) 
		{
			

			//turn to face player
			transform.LookAt (m_playerTrans);
			//timer
			counter++;
			if (counter > maxFrames) 
			{
				//reset counter
				counter = 0;
				//shoot
				//shot animation
				m_actions.Attack (); 
				//create a projectile
				CreateProjectile ();
				//add to shots
				shots += 1;
				if (shots >= 3) 
				{
					//reset counter
					shots = 0;
					m_actions.Stay ();
					//take a pause (simulating reload)
					yield return new WaitForSeconds (1f);
					//debug
					bool check = IsVisible();
					//Debug.Log ("Checkc: " + check.ToString ()); 
					if (check) 
					{
						m_actions.Aiming ();
					} 
					else
						break; //proceed to GoInRange if not visible anymore

				}
			}

			//wait .1 sec
			yield return new WaitForSeconds (0.1f);
		}

		//player out of range
		StartCoroutine("GoInRange");

	}

	IEnumerator GoInRange()
	{
		m_navigator.AgentStop (false); //turn on agent, but dont start patrol
		m_navigator.SetSpeed(3.5f);
		m_actions.Run(); //should be RUN
		//chasing audio
		m_audio.Shout();

		while (!IsShootable())
		{
			m_navigator.Follow (m_playerTrans.position); //follow player
			yield return new WaitForSeconds(0.1f);
		}

		m_navigator.SetSpeed(1f); //just to set nav aget to default value
		StartCoroutine ("Shoot");
	}


	bool ScanForPlayer()
	{
		//ANGULAR DETECTION
		m_playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		//get direction vector
		Vector3 dir = m_playerTrans.position - transform.position;
		//get the angle between the soldier's forward vector and direction towards player
		float angle = Vector3.Angle (transform.forward, dir);


		//enemy has a view frustum with angle 100 degrees (50 to the left, 50 to the right)
		if (Mathf.Abs (angle) <= m_visionAngle) 
		{
			//player is in view frustum, now check if he is in vision range
			if (IsInRange (m_rangeVision)) 
			{
				//finally check if there are no obstacles blocking vision
				return IsVisible();
					
			}

		}

		return false;
	}
		
	bool IsInRange(float range)
	{
		//checks if player is in shooting range

		if(Vector3.Distance (transform.position, m_playerTrans.position) <= range)
			return true; 
		else
			return false;
	}

	bool IsVisible()
	{
		//returns true if there are no obstacles blocking vision
		RaycastHit hit;
		//add 1 unit up to current position (this is done so the linecast doesn't hit ground) 
		//"IgnoreRaycast" layer is not used; need detection with terrain objects ( trees, cliffs )
		Vector3 pos = transform.position + Vector3.up;
		if (Physics.Linecast (pos , m_playerTrans.position, out hit)) 
		{
			//Debug.Log (hit.point.ToString ());
			//check if we hit player
			if (hit.collider.CompareTag ("Player")) {

				return true;
			} 
			else 
			{
				return false;
			}

		}
		//shouldn't happen
		Debug.Log("Player collider NOT hit!");
		return false;
	}

	bool IsShootable()
	{
		//returns true if player is in range and is visible
		if (IsInRange (m_rangeShoot)) 
		{
			if (IsVisible ()) {
				return true;
			} else
				return false;
		} 
		else
			return false;
			
	}
}
