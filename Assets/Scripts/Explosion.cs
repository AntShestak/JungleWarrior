﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Destroy(this.gameObject, 0.75f); //destroy after .5 sec
	}

}
