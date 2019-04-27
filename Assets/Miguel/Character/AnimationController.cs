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
    public void PlayerInput(Vector2 input)
    {
        float speed = (new Vector2(input.x, input.y).sqrMagnitude);

        animator.SetBool("Moving", (speed > 0f) ? true : false);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run"))
        {
            Rotate(input.x);
        }
        Attack();
    }

    private void Rotate(float h)
    {
        if (Mathf.Approximately(h, 0.0f))
            return;

        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + (h > 0 ? 10 : -10), 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, this.turningPower * Time.deltaTime);
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Attack1Trigger");
        }
    }
    #endregion PRIVATES

    private Animator animator = null;
}
