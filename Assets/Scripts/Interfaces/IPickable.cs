using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable 
{
    public enum PickableType { Weapon = 0}

    public PickableType Type { get; set; }
}
