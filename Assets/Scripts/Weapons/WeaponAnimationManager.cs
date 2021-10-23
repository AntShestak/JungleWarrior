using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    private Animator m_anim;
    private PlayerScript m_player;

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
    }

    public void ChangeWeapon(int weaponId)
    {
        
    }
}
