using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void LoadScene(int index)
	{
		SceneManager.LoadScene (index);
		//unset gameManager script
		//try to get GameManager reference
		try
		{
			//reset game manager
			GameObject.FindObjectOfType<GameManager>().IsSet(false);
		}
		catch (System.Exception e) 
		{
			Debug.Log ("SceneLoader can't find GametManager component!  " + e.ToString ());
		}
	}

	//returns current scene index
	public int GetCurrentScene()
	{
		Scene scene = SceneManager.GetActiveScene ();
		return scene.buildIndex;
	}
}
