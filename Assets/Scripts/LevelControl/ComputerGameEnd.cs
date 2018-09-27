using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerGameEnd : MonoBehaviour {

	GameManager m_manager;
	CanvasManager m_canvas;
	bool m_allowed = false;

	// Use this for initialization
	void Start () 
	{
		//try to obtain canvas reference
		try
		{
			m_canvas = GameObject.FindObjectOfType<CanvasManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log (e.ToString ());
		}
		//try to get GameManager reference
		try
		{
			m_manager = GameObject.FindObjectOfType<GameManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("Computer script can't find GameManager component! " + e.ToString ());
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_allowed) 
		{
			//expect inputs
			if (Input.GetKeyDown (KeyCode.Return)) 
			{
				//Add points
				try
				{
					//no more allowed to use computer
					m_allowed = false;
					//turn off "press ENTER" text
					m_canvas.SetComputer(false);
					//add points
					GameObject.FindObjectOfType<SceneController> ().AddWinPoints();
				}
				catch (System.Exception e) 
				{
					Debug.Log ("ComputerGameEnd script can't find scene controller script to add points for win! " + e.ToString ());
				}
				//tell gameManager
				m_manager.GameWon ();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		m_allowed = true;
		//turn on text
		m_canvas.SetComputer (true);
	}

	void OnTriggerExit(Collider other)
	{
		m_allowed = false;
		//turn off text
		m_canvas.SetComputer (false);
	}
}
