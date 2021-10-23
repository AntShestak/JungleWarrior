using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour {

	[SerializeField] AudioSource m_gunAudio; //don't confuse with Player AudioSource component, gun AudioSource is attached to gun
	[SerializeField] AudioClip m_shootAudio;
	[SerializeField] AudioClip m_shootFailed;
	[SerializeField] AudioClip m_equip;


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

	public void PlayEquip()
	{
		m_gunAudio.clip = m_equip;
		m_gunAudio.Play();
	}
}
