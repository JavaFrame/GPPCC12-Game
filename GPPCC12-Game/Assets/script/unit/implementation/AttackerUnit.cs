using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AttackerUnit : Unit
{
	[SerializeField]
	private string attackAnimation = "attack";

	private Hurtable target;

	private Transform targetTransform;

	private NavMeshAgent agent;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float minAttackDistance;

	[SerializeField]
	private float maxAttackDistance;

	[SerializeField]
	private Weapon weapon;

	private State idle, walk, walkAttack, attack;

	public Hurtable AttackTarget
	{
		get { return target; }
		set
		{
			target = value;
			targetTransform = target.transform;
			CurrentState = walkAttack;
		}
	}

	public Transform WalkTarget
	{
		get { return targetTransform; }
		set
		{
			targetTransform = value;
			CurrentState = walk;
		}
	}

	public Hurtable Target
	{
		get { return target; }
	}

	public Transform TargetTransform
	{
		get { return targetTransform; }
	}

	public NavMeshAgent Agent
	{
		get { return agent; }
	}

	public string AttackAnimation
	{
		get { return attackAnimation; }
		set { attackAnimation = value; }
	}

	public float MinAttackDistance
	{
		get { return minAttackDistance; }
		set { minAttackDistance = value; }
	}

	public float MaxAttackDistance
	{
		get { return maxAttackDistance; }
		set { maxAttackDistance = value; }
	}

	void Start ()
	{
		base.Start();
		agent = GetComponent<NavMeshAgent>();

		idle = RegisterState("Idle");
		walk = RegisterState("Walk");
		walkAttack = RegisterState("Walk-Attack");
		attack = RegisterState("Attack");



		//walk update listener
		AddStateUpdateListener(walk, state =>
		{
			if (targetTransform.position != agent.destination)
				agent.destination = targetTransform.position;
		});
		//walk -> idle
		AddStateChangeCondition(walk, idle, state => agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0);

		//walkAttack update listener
		AddStateUpdateListener(walkAttack, state =>
		{
			if(targetTransform.position != agent.destination)
				agent.destination = targetTransform.position;
		});
		//walkAttack -> attack
		AddStateChangeCondition(walkAttack, attack, state => agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0);

		//attack -> walkAttack
		AddStateChangeCondition(attack, walkAttack, state =>
			Vector3.Distance(transform.position, targetTransform.position) > maxAttackDistance &&
			Vector3.Distance(transform.position, targetTransform.position) < minAttackDistance);
		//attack enter: set attack animtion on true
		AddStateEnterListener(attack, (state, newState) => animator.SetBool(attackAnimation, true));
		AddStateEnterListener(attack, (state, newState) => StartCoroutine(Shoot()));
		//attack leave: set attack animtion on false
		AddStateLeaveListener(attack, (state, newState) => animator.SetBool(attackAnimation, false));

		AddStateUpdateListener(attack, state =>
		{
			if (target.life <= 0)
			{
				CurrentState = idle;
			}
		});
	}

	void Update()
	{
		base.Update();
		Vector3 velocity = agent.velocity;
		animator.SetFloat("x", velocity.x);
		animator.SetFloat("y", velocity.y);
	}

	IEnumerator Shoot()
	{
		if(weapon == null)
			throw new Exception("The weaon of the Attack Unit \"" + gameObject.name + "\" isn't set");

		while (CurrentState == attack)
		{
			weapon.Use();
			yield return new WaitForSeconds(weapon.shootTime);
		}
	}
}
