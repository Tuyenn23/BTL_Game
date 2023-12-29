using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Common.FSM
{
	public class FSMState
	{
		private readonly int id;
		private readonly FSM owner;
		private readonly Dictionary<int, FSMState> transitionMap;
		private List<FSMAction> actions;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.FSM.FSMState"/> class.
		/// </summary>
		/// <param name="id">Name.</param>
		/// <param name="owner">Owner.</param>
		public FSMState (int id, FSM owner)
		{
			this.id = id;
			this.owner = owner;
			this.transitionMap = new Dictionary<int, FSMState> ();
			this.actions = new List<FSMAction> ();
		}

		/// <summary>
		/// Adds the transition.
		/// </summary>
		public void AddTransition (int id, FSMState destinationState)
		{
			if (transitionMap.ContainsKey (id)) {
				Debug.LogError (string.Format ("state {0} already contains transition for {1}", this.id, id));
				return;
			}

			transitionMap [id] = destinationState;
		}

		/// <summary>
		/// Gets the transition.
		/// </summary>
		public FSMState GetTransition (int eventId)
		{
			if (transitionMap.ContainsKey (eventId)) {
				return transitionMap [eventId];
			}

			return null;
		}

		/// <summary>
		/// Adds the action.
		/// </summary>
		public void AddAction(FSMAction action)
		{
			if (actions.Contains (action)) {
				Debug.LogWarning ("This state already contains " + action);
				return;
			}

			if (action.GetOwner () != this) {
				Debug.LogWarning ("This state doesn't own " + action);
			}

			actions.Add (action);
		}

		public bool RemoveAction(FSMAction action)
		{
			if (owner.currentState.GetActions().Contains(action)) 
				action.OnExit();
			return actions.Remove(action);
		}

		/// <summary>
		/// This gets the actions of this state
		/// </summary>
		/// <returns>The actions.</returns>
		public List<FSMAction> GetActions ()
		{
			return actions;
		}

		/// <summary>
		/// Sends the event.
		/// </summary>
		public void SendEvent (int eventId)
		{
			this.owner.SendEvent (eventId);
		}

        internal int GetId()
        {
            return id;
        }
    }
}
