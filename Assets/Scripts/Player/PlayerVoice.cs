using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoice : MonoBehaviour {

	public AudioSource m_audio;
	public AudioClip m_hurt;
	public AudioClip m_death;


	public void Hurt()
	{
		if (!m_audio.isPlaying) 
		{
			m_audio.clip = m_hurt;
			m_audio.Play ();
		}
	
	}

	public void Death()
	{
		m_audio.clip = m_death;
		m_audio.Play ();
	}

}
