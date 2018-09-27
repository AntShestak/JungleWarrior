using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAudioManager : MonoBehaviour {

	public AudioClip m_clipHurt;
	public AudioClip m_clipDeath;
	public AudioClip[] m_clipsShouts;

	AudioSource m_audio;

	// Use this for initialization
	void Start () 
	{
		m_audio = GetComponent<AudioSource> ();
	}
	
	public void Hurt()
	{
		m_audio.clip = m_clipHurt;
		m_audio.Play ();
	}

	public void Shout()
	{
		if (!m_audio.isPlaying) 
		{
			//play random clip from an array of shouts
			int max = m_clipsShouts.Length;
			int rand = Random.Range (0, max);
			m_audio.clip = m_clipsShouts [rand];
			m_audio.Play ();
		}
	}
		
	public void Death()
	{
			m_audio.clip = m_clipDeath;
			m_audio.Play ();
	}
}
