﻿using UITweening;
using UnityEngine;

public class UIVerticalAccordionBehaviour : UIBehaviourBase<UIVerticalAccordionBehaviour>
{
    private bool _isActive;
    private UIVirtualValueTweener _activeTween;

    public override void Activate(params object[] parameters)
    {
        StartTransition(true, parameters);
    }

    public override void Deactivate(params object[] parameters)
    {
        StartTransition(false, parameters);
    }

    //parameters: float height
    public override void ResetUI(bool isActivate, params object[] parameters)
    {
        if (_activeTween != null)
        {
            UIVirtualValueTweenController.Instance.StopTweener(_activeTween);

            _activeTween = null;
        }

        _isActive = isActivate;

        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetNewVertSize((float)parameters[0]));

        if (_isActive)
            ShowContent();
        else
            HideContent();
    }

    protected override void ShowContent()
    {
        ContentCanvasGroup.alpha = 1f;
    }

    protected override void HideContent()
    {
        ContentCanvasGroup.alpha = 0f;
    }

    public float GetNewVertSize(float sizeCoef)
    {
        return InitialSize.y * sizeCoef;
    }

    //parameters: float height, float duration, MMTweeningEaseEnum ease
    protected override void StartTransition(bool isActivate, object[] parameters)
    {
        if (_activeTween != null)
        {
            UIVirtualValueTweenController.Instance.StopTweener(_activeTween);

            _activeTween = null;
        }

        _isActive = isActivate;

        if (!_isActive)
            HideContent();

        float height = (float)parameters[0];
        float duration = (float)parameters[1];
        UITweeningEaseEnum ease = (UITweeningEaseEnum)parameters[2];

        UIVirtualValueTweenerInfo tweenInfo = new UIVirtualValueTweenerInfo()
        {
            Duration = duration,
            Ease = ease,
            From = RectTransform.sizeDelta.y,
            IgnoreTimeScale = true,
            To = GetNewVertSize(height),
            Callback = OnTweenUpdated
        };

        UIVirtualValueTweener newTween = new UIVirtualValueTweener(tweenInfo);
        UIVirtualValueTweenController.Instance.StartTweener(newTween);
    }

    public void OnTweenUpdated(float value, bool isFinished)
    {
        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);

        if (isFinished && _isActive)
            ShowContent();

        if(isFinished)
            FireOnTweenFinished(_isActive);
    }
}
