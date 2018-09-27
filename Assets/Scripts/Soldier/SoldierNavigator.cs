using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierNavigator : MonoBehaviour {

	public float m_speed = 1f; //make it constant later
	public Transform[] m_transforms;

	NavMeshAgent m_agent;
	Actions m_actions;
	//patrol points
	Vector3[] m_points;
	int m_nextPoint = 0;
	bool m_isPatroling = true;

	// Use this for initialization
	void Awake () 
	{
		//get nav mesh agent reference
		m_agent = GetComponent<NavMeshAgent>();
		//get actions script reference
		m_actions = GetComponent<Actions>();

		//set points
		int size = m_transforms.Length;
		m_points = new Vector3[size];

		for (int i = 0; i < size; i++) 
		{
			m_points [i] = m_transforms [i].position;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_isPatroling) 
		{
			//check if agent reached destination
			if (m_agent.remainingDistance < 0.5f)
				NextPatrolPoint ();
		} 

	}

	void NextPatrolPoint()
	{
		m_nextPoint += 1;
		//check if we are not out of bounds
		if (m_nextPoint > 2)
			m_nextPoint = 0;

		//set destination
		m_agent.destination = m_points[m_nextPoint];

	}


	public void PatrolStop()
	{
		m_isPatroling = false;
		//stop agent 
		AgentStop(true);

	}

	public void PatrolStart()
	{
		//set first patrol point
		m_agent.destination = m_points[m_nextPoint];

		m_isPatroling = true;
		//stop agent 
		AgentStop(false);
	}

	public void AgentStop(bool value)
	{
		m_agent.isStopped = value;
	}

	public void Follow (Vector3 pos)
	{
		if (pos.y > 1) 
		{
			//this sets agent's destination 5 units towards the PLAYER
			pos.y = 0f;
			Vector3 des = Vector3.MoveTowards (transform.position, pos, 5f);
			m_agent.destination = des;
		} 
		else
			m_agent.destination = pos;

	}

	public void SetSpeed(float x)
	{
		m_agent.speed = m_speed * x ;
	}
		
}
