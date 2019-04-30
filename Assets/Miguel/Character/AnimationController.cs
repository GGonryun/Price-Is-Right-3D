using Photon.Pun;
using UnityEngine;

public class AnimationController : MonoBehaviourPun
{
    [Header("Animation Settings")]
    [Tooltip("How quickly this player will rotate.")]
    [SerializeField]
    float turningSpeed = 250;
    [Tooltip("Multiplier for the base movement speed.")]
    [SerializeField]
    float movementSpeed = 1f;
    [Tooltip("Multiplier for the base animation speed.")]
    [SerializeField]
    float attackSpeed = 1f;
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, this.turningSpeed * Time.deltaTime);
    }

    public void Attack(bool inUse)
    {
        if (inUse)
            animator.SetTrigger("Attack1Trigger");
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
        Animator.StringToHash("Input Z");
    }

    private void Start()
    {
        animator.SetFloat("Input X", movementSpeed);
        animator.SetFloat("Input Z", attackSpeed);
    }
    #endregion UNITY CALLBACKS

    #region PRIVATES
    /// <param name="input">Horizontal = x, Vertical = y</param>

    private Animator animator = null;
    #endregion PRIVATES

}
