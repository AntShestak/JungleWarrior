using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour {


	void OnTriggerEnter(Collider other)
	{
		//control layer only collides with player layer
		//so we know we hit layer
		//destroy the object as no longer needed
		Debug.Log("Hit");
		Destroy (this.gameObject);
	}
}
