// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
    [ActionTarget(typeof(PlayMakerFSM), "_fsmComponent")]
	[Tooltip("Gets the name of the specified FSMs current state. Either reference the _fsm component directly, or find it on a game object.")]
	public class GetFsmState : FsmStateAction
	{
        [Tooltip("Choose a PlayMakerFSM component. If you set a component here it overrides the Game Object and Fsm Name settings.")]
		public PlayMakerFSM fsmComponent;

        [Tooltip("If not specifying the component above, specify the GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object. If left blank it will find the first PlayMakerFSM on the GameObject.")]
		public FsmString fsmName;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the FSM's current State in a string variable.")]
		public FsmString storeResult;
		
        [Tooltip("Repeat every frame. E.g.,  useful if you're waiting for the State to change.")]
        public bool everyFrame;
		
		private PlayMakerFSM fsm;

		public override void Reset()
		{
			fsmComponent = null;
			gameObject = null;
			fsmName = "";
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetFsmState();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmState();
		}
		
		void DoGetFsmState()
		{
			if (fsm == null)
			{
				if (fsmComponent != null)
				{
					fsm = fsmComponent;
				}
				else
				{
					var go = Fsm.GetOwnerDefaultTarget(gameObject);
					if (go != null)
					{
						fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
					}
				}
				
				if (fsm == null)
				{
					storeResult.Value = "";
					return;
				}
			}

			storeResult.Value = fsm.ActiveStateName;
		}

	}
}