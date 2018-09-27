using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {

	FirstPersonController m_playerController;
	CanvasManager m_canvas;
	InputManager m_inputs;

	int m_currentHighScore;
	bool m_isSet = false;

	void Awake()
	{

		//have to set resolution and force fullscreen mode here as player settings don't work
		Screen.SetResolution(1680,1050,true);

		//set GameManager object to not to be destroyed on loading new scenes (required in brief)
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		m_currentHighScore = PlayerPrefs.GetInt ("High");
	}

	//this method is called from scene loader when loading new scene so game manager knows if its ready to perform in this scene or requires a setup
	public void IsSet(bool value)
	{
		m_isSet = value;
		//if false - new scene will load, set time scale to normal
		Time.timeScale = 1;
	}


	public void PauseGame()
	{
		//first check if the manager is set
		if (!m_isSet)
			CheckSetup (); //call check setup if GameManager is not set

		//set game on pause
		Time.timeScale = 0;
		//set UI
		m_canvas.OnPause();
		//disable shooting inputs
		m_inputs.AllowInput(false);
		//stop FP controller inputs and unlock mouse
		m_playerController.enabled = false;

	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
		//UI get's set by pressing resumeGame button, and this function gets called

		//enable shooting inputs
		m_inputs.AllowInput(true);
		//start FP controller inputs and lock mouse
		m_playerController.enabled = true; 
		//mouse gets locked automatically
	}

	public void GameOver()
	{
		//first check if the manager is set
		if (!m_isSet)
			CheckSetup (); //call check setup if GameManager is not set

		StartCoroutine ("GameEnd");

	}

	IEnumerator GameEnd()
	{
		//disable shooting inputs
		m_inputs.AllowInput(false);
		//stop FP controller inputs and unlock mouse
		m_playerController.enabled = false;
		//wait a second
		yield return new WaitForSeconds(1f);
		//set game on pause
		Time.timeScale = 0;
		//compare scores
		int high = PlayerPrefs.GetInt ("High");
		int score = GameObject.FindObjectOfType<SceneController> ().GetScore ();
		if (high < score)
			PlayerPrefs.SetInt ("High", score);
		//set UI
		m_canvas.GameOver(score, high);
		//unlock cursor
		m_inputs.LockMouseCursor(false);

	}

	public void GameWon()
	{

		//first check if the manager is set
		if (!m_isSet)
			CheckSetup (); //call check setup if GameManager is not set
	
		//set game on pause
		Time.timeScale = 0;
		//compare scores
		int high = PlayerPrefs.GetInt ("High");
		int score = GameObject.FindObjectOfType<SceneController> ().GetScore ();
		if (high < score)
			PlayerPrefs.SetInt ("High", score);
		//set UI
		m_canvas.GameWon(score, high);
		//unlock cursor
		m_inputs.LockMouseCursor(false);
		//disable shooting inputs
		m_inputs.AllowInput(false);
		//stop FP controller inputs and unlock mouse
		m_playerController.enabled = false;


	}

	void CheckSetup()
	{
		//get current scene index
		int scene = GetComponent<SceneLoader> ().GetCurrentScene ();

		//depending on the scene do actions a follows
		switch (scene) 
		{
		case 2: //index 2 represents level1
			SetupLevel (); 
			break;

		default:
			break;
		}

		m_isSet = true; //now GameManager is ready
	}

	//this functions set's all level1 references needed for this script
	void SetupLevel()
	{
		//try to get InputManager reference
		try
		{
			m_inputs = GameObject.FindObjectOfType<InputManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("GameManager can't find InputMnager component!  " + e.ToString ());
		}
		//try to get canvas script
		try
		{
			m_canvas = GameObject.FindObjectOfType<CanvasManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("GameManager can't find CanvasManager component!  " + e.ToString ());
		}
		//try to get FirstPersonController reference
		try
		{
			m_playerController = GameObject.FindObjectOfType<FirstPersonController>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("GameManager can't find FirstPersonController script!  " + e.ToString ());
		}
	
	}
		
}
