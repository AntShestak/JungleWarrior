using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour {

	public AudioSource m_gunAudio; //don't confuse with Player AudioSource component, gun AudioSource is attached to gun
	public AudioClip m_shootAudio;
	public AudioClip m_shootFailed;

	public void PlayShootAudio()
	{
		m_gunAudio.clip = m_shootAudio;
		//now play it
		m_gunAudio.Play();
	}

	public void PlayShootFail()
	{
		m_gunAudio.clip = m_shootFailed;
		//now play it
		m_gunAudio.Play();
	}
}
