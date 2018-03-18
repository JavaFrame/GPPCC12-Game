using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	/// <summary>
	/// The initial state of the state machine
	/// </summary>
	[SerializeField] private State _initState = new State("start");

	/// <summary>
	/// The delegate for when state changes happens
	/// </summary>
	/// <param name="oldState">The old state</param>
	/// <param name="newState">the new state</param>
	public delegate void StateChange(State oldState, State newState);

	public delegate void StateUpdate(State state);

	public delegate bool StateChangeCondition(State state);

	/// <summary>
	/// All registered States are in this state
	/// </summary>
	[SerializeField] private Dictionary<String, State> _registeredStates = new Dictionary<String, State>();

	/// <summary>
	/// In this dictionary are all registered states saved with their state change delegateres in a list
	/// </summary>
	[SerializeField]
	private Dictionary<State, List<StateChange>> _stateEnterListeners = new Dictionary<State, List<StateChange>>();

	/// <summary>
	/// In this dictionary are all registered states saved with their state update delegateres in a list
	/// </summary>
	private Dictionary<State, List<StateUpdate>> _stateUpdateListeners = new Dictionary<State, List<StateUpdate>>();

	[SerializeField]
	private Dictionary<State, List<StateChange>> _stateLeaveListeners = new Dictionary<State, List<StateChange>>();

	/// <summary>
	/// the current state
	/// </summary>
	[SerializeField]
	private State _currentState;

	/// <summary>
	/// The initial state of the state machine
	/// </summary>
	public State InitState
	{
		get { return _initState; }
	}

	/// <summary>
	/// All registered states are in this list
	/// </summary>
	public Dictionary<String, State> RegisteredStates
	{
		get { return _registeredStates; }
	}


	/// <summary>
	/// In this dictionary are all registered states saved with their state change delegateres in a list
	/// The listeners are invoked when the state to which they are registered is entered.
	/// </summary>
	protected Dictionary<State, List<StateChange>> StateEnterListeners
	{
		get { return _stateEnterListeners; }
	}

	/// <summary>
	/// In this dictionary are all registered states saved with their state update delegateres in a list
	/// The listeners are invoked when the state to which they are registered is updated.
	/// </summary>
	public Dictionary<State, List<StateUpdate>> StateUpdateListeners
	{
		get { return _stateUpdateListeners; }
	}

	/// <summary>
	/// In this dictionary are all registered states saved with their state change delegateres in a list.
	/// The listeners are invoked when the state to which they are registered is left.
	/// </summary>
	protected Dictionary<State, List<StateChange>> StateLeaveListeners
	{
		get { return _stateLeaveListeners; }
	}


	/// <summary>
	/// The current state
	/// If you set the state, then the registered StateChanges for this state are executed
	/// </summary>
	public State CurrentState
	{
		get { return _currentState; }
		set
		{
			if (value == null)
				throw new Exception("Try to set the currentState to null!");
			if (!IsStateRegistered(value))
				throw new Exception(String.Format("State \"{0}\" wasn't registered!", value.Name));

			Debug.Log("Set current state to " + value.Name);
			var oldState = _currentState;
			_currentState = value;
			StateLeaveListeners[oldState].ForEach(change => change.Invoke(oldState, _currentState));
			StateEnterListeners[_currentState].ForEach(change => change.Invoke(oldState, _currentState));
		}
	}


	protected virtual void Start()
	{
		_initState = RegisterState("Init");
		_currentState = InitState;
	}

	protected virtual void Update()
	{
		UpdateStates();
	}



	/// <summary>
	/// Invokes all update state delgaters which are registered under the current state
	/// </summary>
	protected virtual void UpdateStates()
	{
		if(!IsStateRegistered(CurrentState))
			throw new Exception(String.Format("Currrent state \"{0}\" isnt't registered!", CurrentState.Name));
		foreach (var stateUpdate in StateUpdateListeners[CurrentState])
		{
			stateUpdate.Invoke(CurrentState);
		}
	}

	/// <summary>
	/// Registers a State. After a state is registered it can be used in the state machine
	/// </summary>
	/// <param name="s">the state which should be registered</param>
	/// <returns>the registered state</returns>
	public State RegisterState(string name)
	{
		if (!IsStateRegistered(name))
		{
			State s = new State(name);
			RegisteredStates.Add(name, s);
			StateLeaveListeners.Add(s, new List<StateChange>());
			StateUpdateListeners.Add(s, new List<StateUpdate>());
			StateEnterListeners.Add(s, new List<StateChange>());
		}
		else
		{
			Debug.Log("state already registered");
		}

		return GetState(name);
	}

	public State GetState(string name)
	{
		return RegisteredStates[name];
	}

	public bool IsStateRegistered(string name)
	{
		return RegisteredStates.ContainsKey(name);
	}

	protected bool IsStateRegistered(State s)
	{
		return RegisteredStates.ContainsValue(s);
	}

	/// <summary>
	/// Adds a listener for the given state. It is invoked when the given state is entered
	/// </summary>
	/// <param name="s">the state when it is entered should trigger the listener</param>
	/// <param name="delegater">the listener</param>
	public void AddStateEnterListener(State s, StateChange delegater)
	{
		if (!IsStateRegistered(s))
			throw new Exception(String.Format("State \"{0}\" wasn't registered!", s.Name));
		StateEnterListeners[s].Add(delegater);
	}

	/// <summary>
	/// Adds a listener for the given state. It is invoked when the given state is updated
	/// </summary>
	/// <param name="s">the state when it is updated should trigger the listener</param>
	/// <param name="delegater">the listener</param>
	public void AddStateUpdateListener(State s, StateUpdate delegater)
	{
		if (!IsStateRegistered(s))
			throw new Exception(String.Format("State \"{0}\" wasn't registered!", s.Name));
		StateUpdateListeners[s].Add(delegater);
	}

	public void AddStateChangeCondition(State s, State to, StateChangeCondition stateChangeCondition)
	{
		AddStateUpdateListener(s, state =>
		{
			if (stateChangeCondition.Invoke(CurrentState))
				this.CurrentState = to;
		});
	}

	/// <summary>
	/// Adds a listener for the given state. It is invoked when the given state is left
	/// </summary>
	/// <param name="s">the state when it is left should trigger the listener</param>
	/// <param name="delegater">the listener</param>
	public void AddStateLeaveListener(State s, StateChange delegater)
	{
		if (!IsStateRegistered(s))
			throw new Exception(String.Format("State \"{0}\" wasn't registered!", s.Name));
		StateLeaveListeners[s].Add(delegater);
	}

	/// <summary>
	/// Represents a state
	/// </summary>
	[Serializable]
	public class State
	{
		private static int nextId = 0;

		/// <summary>
		/// a unique id for the state in a state machine. 
		/// </summary>
		[SerializeField]
		private int _id = 0;

		/// <summary>
		/// the name of the state
		/// </summary>
		[SerializeField]
		private string _name;

		/// <summary>
		/// the unique id for the state
		/// </summary>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		/// the name for the state
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="name">the name of the state</param>
		public State(string name)
		{
			this._name = name;
			this._id = GetNextId();
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(State)) return false;
			State other = obj as State;
			return this.Id == other.Id;

		}

		public override int GetHashCode()
		{
			return _id;
		}

		public override string ToString()
		{
			return String.Format("State {id: {0}, name: {1}}", Id, Name);
		}

		public static int GetNextId()
		{
			return nextId++;
		}
	}

}