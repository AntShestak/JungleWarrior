using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] private AudioClip m_exposionClip;
    [SerializeField] private AudioClip m_fireClip;
    private AudioSource m_audio;
    private ParticleSystem m_ps;

    private void Awake()
    {
        m_audio = GetComponent<AudioSource>();
        m_ps = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine("ExplosionControlCoroutine");
        //Destroy(this.gameObject, 10.0f);
    }

    IEnumerator ExplosionControlCoroutine()
    {
        //NOTE: transition from explosion to burn should be much smoother
        IEnumerator explosion = ExplodeCoroutine();
        IEnumerator burn = BurnCoroutine();

        yield return explosion;
        yield return burn;
       
        Destroy(this.gameObject,0.5f);
    }

    IEnumerator ExplodeCoroutine()
    {
        m_audio.clip = m_exposionClip;
        m_audio.volume = 1;
        m_audio.maxDistance = 1000;
        m_audio.Play();

        float timer = 0;
        float maxTime = 2;

        while (timer <= maxTime)
        {
            timer += Time.deltaTime;
            m_audio.volume = Mathf.Clamp01(1 - timer / maxTime);
            yield return null;
        }
        
    }

    IEnumerator BurnCoroutine()
    {
        m_audio.clip = m_fireClip;
        m_audio.loop = true;
        m_audio.volume = 0;
        m_audio.maxDistance = 100;
        m_audio.Play();

        float timeLength = m_ps.main.duration;
        float interpolator = 0;

        //attempting to fade in volume on start burn and fadeout on end burn

        while (interpolator <= 1)
        {
            interpolator = m_ps.time / timeLength;
            if (interpolator >= 0.5f)
                m_audio.volume = Mathf.Clamp01(1 - interpolator);
            else
                m_audio.volume = interpolator;

            yield return null;
        }
    }
}
