using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace PlayMakerFsmTest
{
    [RequireComponent(typeof(Button))]

    public class FsmBuilder : MonoBehaviour
    {
        private const float DELAY_ACTION_DURATION = 3.0f;

        private readonly string _buttonClickStateName = "Button Click State";
        private readonly string _timeDelayStateName = "Time Delay State";
        private readonly string _logStateName = "Log State";
        private readonly string _onButtonClickedEventName = "Button Clicked";
        private readonly string _onTimeDelayOverEventName = "Delay is over";
        private readonly string _logText = $"The button was pressed {DELAY_ACTION_DURATION} seconds ago";

        private readonly Rect _buttonClickStatePosition = new Rect(0, 0, 150, 50);
        private readonly Rect _timeDelayStatePosition = new Rect(100, 100, 150, 50);
        private readonly Rect _logStatePosition = new Rect(200, 200, 150, 50);

        private void Start()
        {
            #region Fsm_Components_Initialization
            Button buttonUI = GetComponent<Button>();
            PlayMakerFSM fsmComponent = buttonUI.gameObject.AddComponent<PlayMakerFSM>();
            Fsm fsm = new Fsm();

            FsmEvent buttonClickedEvent = new FsmEvent(_onButtonClickedEventName);
            FsmEvent delayIsOverEvent = new FsmEvent(_onTimeDelayOverEventName);

            UiButtonOnClickEvent clickEventAction = new UiButtonOnClickEvent();
            Wait waitAction = new Wait();
            DebugLog logAction = new DebugLog();

            FsmState buttonClickState = new FsmState(fsm);
            FsmState timeDelayState = new FsmState(fsm);
            FsmState logState = new FsmState(fsm);

            FsmTransition clickStateTransition = new FsmTransition();
            FsmTransition timeDelayStateTransition = new FsmTransition();
            #endregion

            #region Actions_Setup
            clickEventAction.gameObject = new FsmOwnerDefault();
            clickEventAction.gameObject.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
            clickEventAction.gameObject.GameObject = (FsmGameObject)buttonUI.gameObject;
            clickEventAction.eventTarget = FsmEventTarget.TargetSelf;
            clickEventAction.sendEvent = buttonClickedEvent;
          
            waitAction.time = DELAY_ACTION_DURATION;
            waitAction.finishEvent = delayIsOverEvent;
        
            logAction.logLevel = LogLevel.Info;
            logAction.text = _logText;
            logAction.sendToUnityLog = true;
            #endregion

            #region States_Setup
            buttonClickState.Name = _buttonClickStateName;
            buttonClickState.Actions = new FsmStateAction[] { clickEventAction };
            buttonClickState.SaveActions();
            buttonClickState.Position = _buttonClickStatePosition;
      
            timeDelayState.Name = _timeDelayStateName;
            timeDelayState.Actions = new FsmStateAction[] { waitAction };
            timeDelayState.SaveActions();
            timeDelayState.Position = _timeDelayStatePosition;
      
            logState.Name = _logStateName;
            logState.Actions = new FsmStateAction[] { logAction };
            logState.SaveActions();
            logState.Position = _logStatePosition;

            fsm.States = new FsmState[] { buttonClickState, timeDelayState, logState };
            #endregion

            fsmComponent.Fsm = fsm;
       
            clickEventAction.Init(buttonClickState);
            waitAction.Init(timeDelayState);
            logAction.Init(logState);

            #region Transitions_Setup
            clickStateTransition.FsmEvent = buttonClickedEvent;
            clickStateTransition.ToFsmState = timeDelayState;
       
            timeDelayStateTransition.FsmEvent = delayIsOverEvent;
            timeDelayStateTransition.ToFsmState = logState;
           
            buttonClickState.Transitions = new FsmTransition[] { clickStateTransition };
            timeDelayState.Transitions = new FsmTransition[] { timeDelayStateTransition };
            #endregion

            fsm.StartState = buttonClickState.Name;
            fsm.Start();
        }
    }
}