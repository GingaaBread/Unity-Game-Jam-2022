using System.Collections;
using System.Collections.Generic;
using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TransitionUI : MonoBehaviour
{
    public static TransitionUI Instance;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject objToEnable;

    public UnityEvent OnAnimationComplete = new UnityEvent();

    private void Awake() {
        Assert.IsNull(Instance);
        Instance = this;

        Assert.IsNotNull(animator);
        Assert.IsNotNull(objToEnable);

        objToEnable.SetActive(true);
        TransitionInInstantly();
    }

    private const string TRIGGER_TRANSITIONOUT = "TransitionOut";
    private const string TRIGGER_TRANSITIONIN = "TransitionIn";
    private const string TRIGGER_TRANSITIONOUTINSTANTLY = "TransitionOutInstantly";
    private const string TRIGGER_TRANSITIONININSTANTLY = "TransitionInInstantly";

    public void TransitionOut()            { ResetAnimationTriggers(); animator.SetTrigger(TRIGGER_TRANSITIONOUT); }
    public void TransitionOutInstantly()   { ResetAnimationTriggers(); animator.SetTrigger(TRIGGER_TRANSITIONOUTINSTANTLY); }
    public void TransitionIn()             { ResetAnimationTriggers(); animator.SetTrigger(TRIGGER_TRANSITIONIN); }
    public void TransitionInInstantly()    { ResetAnimationTriggers(); animator.SetTrigger(TRIGGER_TRANSITIONININSTANTLY); }

    private void ResetAnimationTriggers() {
        animator.ResetTrigger(TRIGGER_TRANSITIONIN);
        animator.ResetTrigger(TRIGGER_TRANSITIONOUT);
        animator.ResetTrigger(TRIGGER_TRANSITIONININSTANTLY);
        animator.ResetTrigger(TRIGGER_TRANSITIONOUTINSTANTLY);
    }

    public void HandleAnimationComplete() {
        OnAnimationComplete.Invoke();
    }

}
