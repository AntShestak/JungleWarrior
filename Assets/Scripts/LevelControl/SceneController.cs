using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	//this script controls the scene/level and it's environment

	public int m_maxTime = 300; //5 minutes by default
	public int m_enemies = 0; //starting number of enemy soldiers

	CanvasManager m_canvas;
	AudioSource m_audio;
	int m_time;
	int m_score = 0;

	// Use this for initialization
	void Start () 
	{
		//try to obtain CanavsManager reference
		try
		{
			m_canvas = GameObject.FindObjectOfType<CanvasManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("SceneController can't find canvas manager! " + e.ToString ());
		}

		//get audio souce component
		m_audio = GetComponent<AudioSource>();
		//set number of enemies
		m_canvas.SetEnemies(m_enemies);
		//set time
		m_time = m_maxTime;
		m_canvas.SetTime (m_time);
		//start timer
		StartCoroutine("Timer");
	}
	
	public void EnemyDown()
	{
		//reduce enemy count
		m_enemies -= 1;
		//set number of enemies
		m_canvas.SetEnemies(m_enemies);
		//adjust score
		m_score += 100; //for each enemy 100 points (equivalent 10sec)
	}

	public int GetScore()
	{
		m_score += m_time * 10; //for each second left you get 10 points
		return m_score;
	}

	public void AddWinPoints()
	{
		m_score += 2000; //add points for completing level
	}

	IEnumerator Timer()
	{
		bool loop = true;
		while (loop) 
		{
			//wait 1 sec, reduce time
			yield return new WaitForSeconds (1f);
			m_time -= 1;
			m_canvas.SetTime (m_time);
			//check time
			if (m_time <= 0) {
				//stop looping 
				loop = false;
				try {
					//try to call GameManager function
					GameObject.FindObjectOfType<GameManager> ().GameOver ();
				} catch (System.Exception e) {
					Debug.Log ("SceneController can't find gme manager! " + e.ToString ());
				}
			} else if (m_time <= 15)
				m_audio.Play (); //when it less than 15 seconds left play audio every second
		}



	}

}
