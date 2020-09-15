using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class disables the animator after animation
/// </summary>
public class DisableAnimatorAfterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    /// <summary>
    /// Disable the attached animator
    /// </summary>
    private void DisableAnimator()
    {
        if (animator)
        {
            animator.enabled = false;
        }
    }
}
