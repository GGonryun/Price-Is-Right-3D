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

    private void Update()
    {
        if (photonView.IsMine)
        {
            PlayerInput();
        }
    }
    #endregion UNITY CALLBACKS

    #region PRIVATES
    /// <param name="input">Horizontal = x, Vertical = y</param>
    private void PlayerInput()
    {
        bool attack = Input.GetButtonDown("Fire2");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v < 0)
            v = 0;
        float speed = (new Vector2(h, v).sqrMagnitude);

        animator.SetBool("Moving", (speed > 0f) ? true : false);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack1")) {
            Rotate(h);
        }

        if (attack)
        {
            //Debug.Log($"Attacking: {PhotonNetwork.LocalPlayer.NickName}");
            animator.SetTrigger("Attack1Trigger");
        }
    }

    private void Rotate(float h)
    {
        if (Mathf.Approximately(h, 0.0f))
            return;

        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + (h > 0 ? 10 : -10), 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, this.turningPower * Time.deltaTime);
    }

    private Animator animator = null;
    #endregion PRIVATES

}
