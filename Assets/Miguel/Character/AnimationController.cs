using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviourPun
{
    [Header("Animation Settings")]
    [Tooltip("")]
    [SerializeField]
    float turningPower = 250;

    public void Moving(float speed)
    {
        animator.SetBool("Moving", (speed > 0f) ? true : false);
    }

    public void Rotating(float h)
    {
        if (Mathf.Approximately(h, 0.0f))
            return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack1"))
            return;
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + (h > 0 ? 10 : -10), 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, this.turningPower * Time.deltaTime);
    }

    public void Attack(bool inUse)
    {
        if (inUse)
            animator.SetTrigger("Attack1Trigger");
    }

    public void Defend(float delta)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack1"))
            return;
        animator.SetFloat("Input X", delta);
    }

    #region UNITY CALLBACKS
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator)
            Debug.LogError($"<Color=Red><a> AnimationController </a></Color>is missing an animator component.", this);

        Animator.StringToHash("Attack1Trigger");
        Animator.StringToHash("Moving");
        Animator.StringToHash("Input X");
    }
    #endregion UNITY CALLBACKS

    #region PRIVATES
    /// <param name="input">Horizontal = x, Vertical = y</param>

    private Animator animator = null;
    #endregion PRIVATES

}
