﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MMUISystem.UIButton
{
    [AddComponentMenu("UI/Extensions/UIButton")]
    [RequireComponent(typeof(CanvasRenderer), typeof(Image), typeof(Button)), DisallowMultipleComponent]
    public class UnityUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool StartListeningOnEnable;
        public bool IsDelayedButton;
        public float DelayDurationBeforeFire;

        protected bool IsListening;

        private UIButtonStateMachine StateMachine;
        private PointerEventData LastEventData;

        #region Events
    public Action<PointerEventData> OnButtonPressDown;
        public Action<PointerEventData> OnButtonPress;
        public Action<PointerEventData> OnDelayedButtonPressDown;
        public Action<PointerEventData> OnButtonPressUp;
        public Action<PointerEventData> OnButtonDoubleTap;
        public Action<PointerEventData> OnTapAndPress;
        public Action<PointerEventData> OnButtonPressCancel;

        public static Action<PointerEventData> OnButtonPressDelayStarted;
        public static Action<float> OnButtonPressDelayUpdate;
        public static Action<PointerEventData> OnButtonPressDelayFinished;
        public static Action OnButtonPressDelayCanceled;

        void FireOnButtonPressDelayCanceled()
        {
            if (OnButtonPressDelayCanceled != null)
                OnButtonPressDelayCanceled();
        }

        void FireOnButtonPressDelayUpdate(float progress)
        {
            if (OnButtonPressDelayUpdate != null)
                OnButtonPressDelayUpdate(progress);
        }

        void FireOnDelayStarted(PointerEventData eventData)
        {
            if (OnButtonPressDelayStarted != null)
                OnButtonPressDelayStarted(eventData);
        }

        void FireOnDelayFinished(PointerEventData eventData)
        {
            if (OnButtonPressDelayFinished != null)
                OnButtonPressDelayFinished(eventData);
        }

        void FireOnDelayedButtonPressDown(PointerEventData eventData)
        {
            if (OnDelayedButtonPressDown != null)
                OnDelayedButtonPressDown(eventData);
        }

        void FireOnButtonPressDown(PointerEventData eventData)
        {
            if (OnButtonPressDown != null)
                OnButtonPressDown(eventData);
        }

        void FireOnButtonPressUp(PointerEventData eventData)
        {
            if (OnButtonPressUp != null)
                OnButtonPressUp(eventData);
        }

        void FireOnButtonDoubleTap(PointerEventData eventData)
        {
            if (OnButtonDoubleTap != null)
                OnButtonDoubleTap(eventData);
        }

        void FireOnButtonPressCancel(PointerEventData eventData)
        {
            if (OnButtonPressCancel != null)
                OnButtonPressCancel(eventData);
        }

        void FireOnButtonPress(PointerEventData eventData)
        {
            if (OnButtonPress != null)
                OnButtonPress(eventData);
        }

        void FireOnTapAndPress(PointerEventData eventData)
        {
            if (OnTapAndPress != null)
                OnTapAndPress(eventData);
        }
        #endregion

        private void Awake()
        {
            StateMachine = new UIButtonStateMachine();
            StateMachine.Init();
        }

        protected virtual void OnEnable()
        {
            if (StartListeningOnEnable)
                StartListening();
        }

        protected virtual void OnDisable()
        {
            StateMachine.ResetMachine();

            if (StartListeningOnEnable)
                StopListening();
        }

        public virtual void StartListening()
        {
            IsListening = true;

            StateMachine.OnStateEntered += OnStateEntered;
            StateMachine.OnStateHandled += OnStateHandled;
            StateMachine.OnStateExited += OnStateExited;
        }

        public virtual void StopListening()
        {
            IsListening = false;

            StateMachine.OnStateEntered -= OnStateEntered;
            StateMachine.OnStateHandled -= OnStateHandled;
            StateMachine.OnStateExited -= OnStateExited;
        }

        #region Interface Implementation
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (IsListening)
            {
                if (eventData.dragging)
                    return;

                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.PressDown);
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (IsListening)
            {
                if (eventData.dragging)
                    return;

                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.PressUp);
            }
        }
        #endregion

        #region StateMachine Implementation
        private void OnStateEntered(InteractionStateEnum state)
        {
        }

        private void OnStateHandled(InteractionStateEnum state)
        {
            Debug.Log(state);
            switch(state)
            {
                //case InteractionStateEnum.DelayedPress:
                //    break;
                case InteractionStateEnum.DoubleTap:
                    FireOnButtonDoubleTap(LastEventData);
                    break;
                case InteractionStateEnum.Press:
                    FireOnButtonPress(LastEventData);
                    break;
                case InteractionStateEnum.PressDown:
                    FireOnButtonPressDown(LastEventData);
                    break;
                case InteractionStateEnum.Idle:
                    FireOnButtonPressUp(LastEventData);
                    break;
                case InteractionStateEnum.TapAndPress:
                    FireOnTapAndPress(LastEventData);
                    break;
            }
        }

        private void OnStateExited(InteractionStateEnum state)
        {
        }
        #endregion

        private void TriggerStateMachine(CommandEnum command)
        {
            StateMachine.UpdateState(command);
        }
    }
}
