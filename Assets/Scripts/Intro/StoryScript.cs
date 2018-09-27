using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryScript : MonoBehaviour {

	//index for next scene, default 0 (menu) to determine error
	public int m_next = 0;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.anyKeyDown)
			SceneManager.LoadScene(m_next);
		
	}
}
