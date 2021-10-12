using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] private AudioClip m_exposionClip;
    [SerializeField] private AudioClip m_fireClip;
    private AudioSource m_audio;

    private void Awake()
    {
        m_audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine("ExplosionBurnCoroutine");
        //Destroy(this.gameObject, 10.0f);
    }

    IEnumerator ExplosionBurnCoroutine()
    {
        //NOTE: transition from explosion to burn should be much smoother

        m_audio.clip = m_exposionClip;
        m_audio.Play();
        yield return new WaitForSeconds(2.0f);
        m_audio.clip = m_fireClip;
        m_audio.loop = true;
        m_audio.Play();
        Destroy(this.gameObject, 14.0f);
    }
}
