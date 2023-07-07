using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationSystem
{
public class AnimationHandler : MonoBehaviour
{
    [Tooltip("<ActionName,AnimationName>")]
    [SerializeField] private List<AnimationClip> Animations;
    [SerializeField] private Dictionary<string,AnimationClip> m_animations;

    [SerializeField] private Animator m_animator;
    private AnimationClip m_currentAnimation;

    Action OnAnimationEndCallback;
    Action<string> OnKeyframeEventCallback;

    private void Awake()
    {
        m_animations=new Dictionary<string,AnimationClip>();
        m_animator= GetComponent<Animator>();

        for (int i = 0; i < Animations.Count; i++)
        {
            m_animations.Add(Animations[i].Action, Animations[i]);
        }
    }

    public void StartAnimation(string aAction)
    {
        AnimationClip anim = m_animations[aAction];

        if (anim != null)
        {
            if (m_currentAnimation != null)
            {
                m_currentAnimation.EndAnimation(m_animator);
            }
            else if (m_currentAnimation == anim)
            {
                return;
            }

            anim.BeginAnimation(m_animator);
            m_currentAnimation = anim;
        }
        else YCLogger.Error(aAction + " Clip not found");
    }
    public void StartAnimation(string aAction, Action aOnAnimationEnd)
    {
        AnimationClip anim = m_animations[aAction];

        if (anim != null)
        {
            anim.BeginAnimation(m_animator);
            m_currentAnimation = anim;
            OnAnimationEndCallback = aOnAnimationEnd;
        }
        else YCLogger.Error(aAction + " Clip not found");

    }
    public void StartAnimation(string aAction, Action<string> aOnKeyFrameEvent)
    {
        AnimationClip anim = m_animations[aAction];

        if (anim != null)
        {
            anim.BeginAnimation(m_animator);
            m_currentAnimation = anim;
            OnKeyframeEventCallback += aOnKeyFrameEvent;
        }
        else YCLogger.Error(aAction + " Clip not found");

    }
    public void StartAnimation(string aAction, Action aOnAnimationEnd, Action<string> aOnKeyFrameEvent)
    {
        AnimationClip anim = m_animations[aAction];

        if (anim != null)
        {
            anim.BeginAnimation(m_animator);
            m_currentAnimation = anim;
            OnAnimationEndCallback = aOnAnimationEnd;
            OnKeyframeEventCallback = aOnKeyFrameEvent;
        }
        else YCLogger.Error(aAction + " Clip not found");

    }

    //UnityEvent
    public void OnKeyFrameEvent(string aEventAction)
    {
        if (OnKeyframeEventCallback != null)
        {
            OnKeyframeEventCallback.Invoke(aEventAction);
            OnKeyframeEventCallback = null;
        }
    }
    //UnityEvent
    public void OnAnimationEnd()
    {
        OnAnimationEndCallback.Invoke();
        OnAnimationEndCallback=null;
        OnKeyframeEventCallback = null;
    }

    public bool IsAnimationPlaying(string aAnimationName) 
    { 
        if(m_currentAnimation==null) return false;
        return aAnimationName == m_currentAnimation.Action ? true : false; 
    }
}

[Serializable]
public class AnimationClip
{
    public string Action;

    [SerializeField] AnimatorControllerParameterType parameterType;

    [SerializeField] string m_parameter;
    [SerializeField] string m_triggerValue;

    [SerializeField] bool resetToPrevValueOnStop=false;
    [SerializeField] dynamic m_previousValue=false;
      
    public void BeginAnimation(Animator aAnimator)
    {
        if (m_parameter == "") return;
        SetValue(aAnimator,m_parameter, m_triggerValue);
    }

    public void EndAnimation(Animator aAnimator)
    {
        if (m_parameter != "" && resetToPrevValueOnStop)
        {
            SetValue(aAnimator, m_parameter, m_previousValue);
        }
    }
        
    //Function to support "dynamic" parameter type support,it converts to the value in string to appropriate type and sets it in Animator controller/
    public void SetValue(Animator aAnimator,string aParameter,string aValue)
    {
        switch (parameterType)
        {
            case AnimatorControllerParameterType.Bool:
                m_previousValue = aAnimator.GetBool(aParameter);
                aAnimator.SetBool(aParameter, bool.Parse(aValue));
                break;
            case AnimatorControllerParameterType.Trigger:
                aAnimator.SetTrigger(aParameter);
                break;
            case AnimatorControllerParameterType.Float:
                m_previousValue = aAnimator.GetFloat(aParameter);
                aAnimator.SetFloat(aParameter, float.Parse(aValue));
                break;
            case AnimatorControllerParameterType.Int:
                m_previousValue = aAnimator.GetInteger(aParameter);
                aAnimator.SetInteger(aParameter, int.Parse(aValue));
                break;
        }

    }

    //Function to reset the parameter back to intial value.
    public void SetValue(Animator aAnimator,string aParameter,dynamic aValue)
    {
        switch (parameterType)
        {
            case AnimatorControllerParameterType.Bool:
                aAnimator.SetBool(aParameter, m_previousValue);
                break;
            case AnimatorControllerParameterType.Trigger:
                aAnimator.SetTrigger(aParameter);
                break;
            case AnimatorControllerParameterType.Float:
                aAnimator.SetFloat(aParameter, m_previousValue);
                break;
            case AnimatorControllerParameterType.Int:
                aAnimator.SetInteger(aParameter, m_previousValue);
                break;
        }

    }
}
}
