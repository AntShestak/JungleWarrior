using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public int Ammo { get; set; }
    public void Shoot();

    public void AddAmmo(int toAdd);
    

    public void DeductAmmo(int toDeduct);


}
