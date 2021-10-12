using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private GameObject m_explosion;
    [SerializeField] private float m_force = 100f;
    [SerializeField] private float m_secToExplode = 4.0f;

    private Rigidbody m_rb;
    private AudioSource m_audio;
    

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_audio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GrenadeCoroutine");
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_audio.Play();
    }

    private IEnumerator GrenadeCoroutine()
    {
        m_rb.AddForce(transform.up * m_force, ForceMode.Impulse);
        yield return new WaitForSeconds(m_secToExplode);
        Instantiate(m_explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    
}
