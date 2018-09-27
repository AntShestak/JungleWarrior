using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	public AudioClip m_song1;
	public AudioClip m_song2;

	SceneLoader m_loader;
	AudioSource m_audio;
	int m_sceneIndex = 1; //default level is 1

	void Start()
	{
		//try to obtain reference to scene loader script
		try
		{
			m_loader = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneLoader>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("MenuManager can't find GameManager's component: SceneLoader  . " + e.ToString ()); 
		}
		//get audio source component
		m_audio = GetComponent<AudioSource>();
		StartCoroutine ("Audio");

	}

	IEnumerator Audio()
	{
		while (true) 
		{
			yield return new WaitForSeconds (5f);
			//check if the clip is still playing
			if (!m_audio.isPlaying) 
			{
				//audio stopped
				if(m_audio.clip.Equals(m_song1))
				{
					m_audio.clip = m_song2;
					m_audio.Play();
				}
				else
				{
					m_audio.clip = m_song1;
					m_audio.Play();
				}
			}
		}
	}

	public void StartGame()
	{
		m_loader.LoadScene (m_sceneIndex);
	}

	//set the scene to be loaded when game starts
	public void SetScene(int index)
	{
		m_sceneIndex = index;
	}

	public void ExitGame()
	{
		Application.Quit ();
	}
}
