using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;


[RequireComponent(typeof(Hurtable), typeof(Rigidbody))]
public class AttackerUnit : Unit
{
	[SerializeField]
	private string attackAnimation = "attacking";

	private Hurtable target;

	private Transform targetTransform;

	private Vector3 targetPos;

	private NavMeshAgent agent;

	private Rigidbody rigidbody;

	private float lastRemainingDistance;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float minAttackDistance;

	[SerializeField]
	private float maxAttackDistance;

	[SerializeField]
	private Weapon weapon;


	private State idle, preWalk, walk, preWalkAttack, walkAttack, attack;

	public Hurtable AttackTarget
	{
		get { return target; }
		set
		{
			rigidbody.velocity = Vector3.zero;
			target = value;
			targetTransform = target.transform;
			CurrentState = preWalkAttack;
			agent.destination = targetTransform.position;
			//agent.stoppingDistance = (MinAttackDistance + MaxAttackDistance) / 2;
		}
	}

	public Vector3 WalkTarget
	{
		get { return targetPos; }
		set
		{
			rigidbody.velocity = Vector3.zero;
			targetPos = value;
			CurrentState = preWalk;
			agent.destination = value;
			agent.stoppingDistance = 0.5f;
			lastRemainingDistance = Vector3.Distance(targetPos, transform.position);
			
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

		rigidbody = GetComponent<Rigidbody>();

		agent = GetComponent<NavMeshAgent>();
		idle = RegisterState("Idle");
		preWalk = RegisterState("Pre Walk");
		walk = RegisterState("Walk");
		preWalkAttack = RegisterState("Pre Walk-Attack");
		walkAttack = RegisterState("Walk-Attack");
		attack = RegisterState("Attack");

		InfoShower.Instance.ChangeSecundarySelectionEvent += (added, removed, selected) =>
		{
			if (InfoShower.Instance.SelectedObjects.Contains(gameObject))
			{
				foreach (var selGo in selected)
				{
					Player p = selGo.GetComponent<Player>();
					if (p != null)
					{
						AttackTarget = p.GetComponent<Hurtable>();
					}
				}
			}
		};

		InfoShower.Instance.ChangePositionEvent += pos =>
		{
			if (InfoShower.Instance.SelectedObjects.Contains(this.gameObject))
			{
				WalkTarget = pos;

			}
		};

		//pre-walk -> walk
		AddStateChangeCondition(preWalk, walk, state => true);

		//walk update listener
		AddStateUpdateListener(walk, state =>
		{
			if (targetPos!= agent.destination)
				agent.destination = targetPos;
			if (Math.Abs(lastRemainingDistance) <= Math.Abs(agent.remainingDistance) && agent.remainingDistance != Mathf.Infinity)
				agent.stoppingDistance = agent.remainingDistance + 0.1f;
			lastRemainingDistance = agent.remainingDistance;

		});
		//walk -> idle
		AddStateChangeCondition(walk, idle, state => agent.remainingDistance-agent.stoppingDistance <= 0);


		AddStateChangeCondition(preWalkAttack, walkAttack, state => true);
		//walkAttack update listener
		AddStateUpdateListener(walkAttack, state =>
		{
			if(targetTransform.position != agent.destination)
				agent.destination = targetTransform.position;
            NavMeshHit hit;
            if(!agent.Raycast(targetTransform.position, out hit)
                && Vector3.Distance(targetTransform.position, transform.position) < maxAttackDistance
                && Vector3.Distance(targetTransform.position, transform.position) > minAttackDistance)
            {
                agent.stoppingDistance = agent.remainingDistance;
            }

		});
		//walkAttack -> attack
		AddStateChangeCondition(walkAttack, attack, state => {
            NavMeshHit hit;
            bool result = !agent.Raycast(targetTransform.position, out hit);
			Debug.Log(hit.distance);
			return result;
		});

		//attack -> walkAttack
		AddStateChangeCondition(attack, walkAttack, state =>
			targetTransform == null || //prevents that the check below access a already died player
			Vector3.Distance(transform.position, targetTransform.position) > maxAttackDistance &&
			Vector3.Distance(transform.position, targetTransform.position) < minAttackDistance);
		//attack enter: set attack animtion on true
		AddStateEnterListener(attack, (state, newState) =>
		{
			agent.stoppingDistance = agent.remainingDistance;
            StartCoroutine(Shoot());
        });
		//attack leave: set attack animtion on false
		//AddStateLeaveListener(attack, (state, newState) => animator.SetBool(attackAnimation, false));

		AddStateUpdateListener(attack, state =>
		{
			if (targetTransform == null || target.life <= 0) 
			{
				CurrentState = idle;
			}
			if (targetTransform == null) return;
			transform.rotation = Quaternion.LookRotation(targetTransform.position - agent.transform.position);
		});
	}

	void Update()
	{
		base.Update();
		Vector3 velocity = agent.velocity;
		animator.SetFloat("x", velocity.y);
		animator.SetFloat("y", velocity.x);
	}

	IEnumerator Shoot()
	{
		if(weapon == null)
			throw new Exception("The weaon of the Attack Unit \"" + gameObject.name + "\" isn't set");

		while (CurrentState == attack)
		{
			if (weapon.Use())
			{
				animator.SetTrigger(attackAnimation);
			}
			yield return new WaitForSeconds(weapon.shootTime);
		}
	}
}
