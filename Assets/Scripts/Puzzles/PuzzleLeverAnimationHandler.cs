using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLeverAnimationHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    public PuzzleLeverController leverController;

    //public Animator aa { get => animator; }
    //public Animator Aa => animator;

    public enum Puller
    {
        Cedric, Eleanor
    }

    public void StartPullAnimation(Puller puller)
    {
        //Do different animations depending on puller
        animator.SetInteger("Character", puller == Puller.Cedric ? 1 : 0);
        animator.SetBool("IsPulled", !leverController.isPulled);
    }

    public void OnPull()
    {
        leverController.Pull();
    }
}
