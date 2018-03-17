using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AttackerUnit : Unit
{
	private Vector3 target;
	private NavMeshAgent agent;

	void Start ()
	{
		agent = GetComponent<NavMeshAgent>();

		State idle = RegisterState("Idle");
		State walk = RegisterState("Walk");
		State walkAttack = RegisterState("Walk-Attack");
		State attack = RegisterState("Attack");

		AddStateEnterListener(walk, (oldState, newState) => agent.destination = target);
		//AddStateChangeCondition(walk, idle, state => );

		AddStateEnterListener(walkAttack, (oldState, newState) =>
		{

		});

		AddStateEnterListener(attack, (oldState, newState) =>
		{

		});
	}
	
}
