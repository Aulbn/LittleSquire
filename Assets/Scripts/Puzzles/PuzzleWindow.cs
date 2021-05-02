using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleWindow : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Open()
    {
        animator.SetBool("IsOpen", true);
    }

    public void Close()
    {
        animator.SetBool("IsOpen", false);
    }

}
