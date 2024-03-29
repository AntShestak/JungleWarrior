using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson; //for MouseLook

public class InputManager : MonoBehaviour {

	public MouseLook m_mouseScript;

	GameManager m_manager;
	Animator m_anim;
	PlayerScript m_player;

	bool m_inputAllowed = true;


	// Use this for initialization
	void Start () 
	{
		//get gamemanager reference
		try
		{
			m_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("Input manager can't access GameManager component through 'GameContoller' tag!  " + e.ToString ()); 
		}
		m_anim = gameObject.GetComponentInChildren<Animator> ();
		//get PlayerScript reference
		m_player = this.gameObject.GetComponent<PlayerScript>();
		//allow inputs
		AllowInput(true);
		//unlock cursor
		LockMouseCursor(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_inputAllowed) 
		{
			//pausing game
			if(Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown (KeyCode.Escape))
			{
				m_manager.PauseGame ();
				//unlock mouse
				LockMouseCursor(false);
			}

			if (Input.GetMouseButtonDown(0))
				m_player.ShootCall ();
		}
			
	}

	//=============PUBLIC FUNCTIONS===========
	public void AllowInput(bool value)
	{
		m_inputAllowed = value;
	}

	public void LockMouseCursor(bool value)
	{
		m_mouseScript.SetCursorLock (value);
	}
}
