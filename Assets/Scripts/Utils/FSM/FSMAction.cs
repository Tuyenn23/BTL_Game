using UnityEngine;
using System.Collections;

namespace Common.FSM
{
	public class FSMAction
	{
		private readonly FSMState owner;

		public FSMAction (FSMState owner)
		{
			this.owner = owner;
		}

		public FSMState GetOwner ()
		{
			return owner;
		}

		///<summary>
		/// Starts the action.
		///</summary>
		public virtual void OnEnter ()
		{           
        }

		///<summary>
		/// Updates the action.
		///</summary>
		public virtual void OnUpdate ()
		{
		}

		public virtual void OnLateUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {

        }

		///<summary>
		/// Finishes the action.
		///</summary>
		public virtual void OnExit ()
		{
		}
	}
}
