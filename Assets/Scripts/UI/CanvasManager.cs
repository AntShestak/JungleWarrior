using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// OLD CODE | REFACTORING REQUIRED
/// </summary>

public class CanvasManager : MonoBehaviour {



	//public GameObject m_panelGUI;
	public GameObject m_panelPause;
	public GameObject m_panelGameOver;
	public GameObject m_panelGameWon;
	//public Slider m_ammoSlider;
	//public Slider m_healthSlider;
	public Text m_enemies;
	public Text m_time;
	//Game Won and Game Lost are different panels
	//Text objects Game Won panel
	public Text m_scoreWon; 
	public Text m_highscoreWon; 
	//Text objects Game Lost panel
	public Text m_scoreLost;
	public Text m_highscoreLost;

	public Image m_bloodFader; //blood image, when low health
	public GameObject m_computer;	//objects that holds text how to operate computer

	GameManager m_manager;


	// Use this for initialization
	void Start () 
	{
		//set pause panel inactive
		m_panelPause.SetActive(false);
		//set GUI panel active
		//m_panelGUI.SetActive(true);
		//try to find GameManager script
		try
		{
			m_manager = GameObject.FindObjectOfType<GameManager>();
		}
		catch (System.Exception e) 
		{
			Debug.Log ("CanvasManager can't find GameMnager component!  " + e.ToString ());
		}
	}
	
	public void OnPause()
	{
		//set GUI panel inactive
		//m_panelGUI.SetActive(false);
		//set pause panel inactive
		m_panelPause.SetActive(true);
	}

	public void OnResume()
	{
		//set pause panel inactive
		m_panelPause.SetActive(false);
		//set GUI panel active
		//m_panelGUI.SetActive(true);
		//call gameManager
		m_manager.ResumeGame();
	}

	public void UpdateAmmoDisplay(int value, int max)
	{
		//NOTE: cast is redundant, but dividing integers doesn't work out
		float sliderValue = (float)value / (float)max;
		//Debug.Log($"Updating ammo display to: {sliderValue}");
		//m_ammoSlider.value = sliderValue;
	}
		
	public void SetHealth(float value, float damage)
	{
		//adjust slider value
		//m_healthSlider.value = value;
		//damage 
		//update bllod image if health is low
		if (value <= damage) 
		{ //if is not enough health to withstand
			//calculate alpha value based on current health value, and what percentage it is out of one shot's damage
			float alpha = (1f - value / damage) * 0.75f;
			//create color
			Color color = new Color (1f, 1f, 1f, alpha);
			//update color
			m_bloodFader.color = color;
		}
	}

	public void GameOver(int current, int high)
	{
		//set GUI panel inactive
		//m_panelGUI.SetActive(false);
		//activate Game over panel
		m_panelGameOver.SetActive(true);
		m_scoreLost.text = current.ToString ();
		m_highscoreLost.text = high.ToString ();
	}

	public void GameWon(int current, int high)
	{
		//set GUI panel inactive
		//m_panelGUI.SetActive(false);
		//activate Game Won panel
		m_panelGameWon.SetActive(true);

		m_scoreWon.text = current.ToString ();
		m_highscoreWon.text = high.ToString ();

	}

	public void SetEnemies(int number)
	{
		m_enemies.text = number.ToString ();
	}

	public void SetTime(int seconds)
	{
		int mins = seconds / 60;
		int sec = seconds - mins * 60;
		m_time.text = mins.ToString() + " : " + sec.ToString();
		//if time is 15 start animation
		if (seconds == 15)
			GetComponent<Animator> ().SetTrigger ("Time");
	}
		

	public void SetComputer(bool value)
	{
		//turn on or off computer operation text
		m_computer.SetActive(value); 
	}
		
}
