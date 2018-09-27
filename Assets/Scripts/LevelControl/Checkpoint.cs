using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public GameObject m_soldiers; //the zone of soldiers to activate
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			//checkpoint reached
			//enable next zone of soldiers
			m_soldiers.SetActive(true);
			
			//set object inactive, because it's used only once
			this.gameObject.SetActive(false);
		}
			
	}
}
