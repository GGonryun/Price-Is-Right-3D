using Photon.Pun;
using UnityEngine;

public class ImpactDetector : MonoBehaviourPun, IPunObservable
{
    public float Multiplier => multiplier;
    public float Height => characterController.height;

    [SerializeField] private float multiplier = 0;

    [Tooltip("")]
    [SerializeField]
    private float baseKnockback = 10f;

    #region UNITY CALLBACKS
    private void Awake()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        if (!characterController)
            Debug.LogError("<Color=Red> ImpactDetector <a></a></Color>is missing a CharacterController component !! ", this);
    }

    private void Update()
    {
        if (isDead == true)
            GameManager.Instance.OnClickLeave();

        ConsumeImpact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;
        if (other.gameObject.CompareTag("Deadzone"))
            isDead = true;

        if (other.gameObject.CompareTag("Sword"))
        {
            Vector3 heading = other.transform.forward;
            heading.y *= 0f;
            currentMultiplier += swordMultiplier;
            AddKnockback(heading, baseKnockback * swordInfluence * currentMultiplier);
        }

        if (other.gameObject.CompareTag("Arrow"))
        {
            Vector3 heading = other.transform.forward;
            heading.y *= 0f;
            currentMultiplier += arrowMultiplier;
            AddKnockback(heading, baseKnockback * arrowInfluence * currentMultiplier);
        }

        if (other.gameObject.CompareTag("TwoHand"))
        {
            Vector3 heading = other.transform.forward;
            heading.y *= 0f;
            currentMultiplier += twoHandMultiplier;
            AddKnockback(heading, baseKnockback * twoHandInfluence * currentMultiplier);
        }
        multiplier = currentMultiplier;
    }

    private void OnParticleCollision(GameObject other)
    {
        //if it hits someone else don't react.
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;
        if (!other.gameObject.CompareTag("Spell"))
            return;

        //We only care about the movement in the x/z plane.
        Vector3 heading = transform.position - other.transform.position;
        heading.y *= 0f;

        //Increase the knockback multiplier.
        currentMultiplier += spellMultiplier;
        multiplier = currentMultiplier;
        AddKnockback(heading, baseKnockback * spellInfluence * currentMultiplier);
    }
    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(isDead);
            stream.SendNext(multiplier);
        }
        else
        {
            isDead = (bool)stream.ReceiveNext();
            multiplier = (float)stream.ReceiveNext();
        }
    }
    #endregion PUN CALLBACKS

    #region PRIVATES
    private void ConsumeImpact()
    {
        if (impact.sqrMagnitude > impactThreshold)
        {
            characterController.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, dissipationRate * Time.deltaTime);
        }
    }

    private void AddKnockback(Vector3 direction, float force)
    {
        Vector3 _dir = direction.normalized;
        impact += _dir * force;
    }

    private CharacterController characterController = null;
    private Vector3 impact = Vector3.zero;
    private float impactThreshold = 0.1f;
    private float dissipationRate = 5f;
    private float currentMultiplier = 1f;
    private bool isDead = false;

    //Modify these values if you want the character taking damage to be influenced more heavily by the collision.
    //1 = 100%
    private float arrowInfluence = .5f;
    private float swordInfluence = 1.1f;
    private float twoHandInfluence = 1.5f;
    private float spellInfluence = .9f;

    //These values represent how much we will increment our multiplier by each collision.
    private float arrowMultiplier = .25f;
    private float swordMultiplier = .5f;
    private float twoHandMultiplier = .9f;
    private float spellMultiplier = .5f;
    #endregion PRIVATES

}
