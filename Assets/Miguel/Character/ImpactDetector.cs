using Photon.Pun;
using UnityEngine;

public class ImpactDetector : MonoBehaviourPun, IPunObservable
{
    #region UNITY CALLBACKS
    private void Awake()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        if (!characterController)
            Debug.LogError("<Color=Red> ImpactDetector <a></a></Color>is missing a CharacterController component !! ", this);


        animationController = gameObject.GetComponent<AnimationController>();
        if(!animationController)
            Debug.LogError("<Color=Red> ImpactDetector <a></a></Color>is missing an AnimationController component !! ", this); 
    }

    private void Update()
    {
        if (isDead == true)
            GameManager.Instance.OnClickLeave();

        ConsumeImpact();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;
        if (other.gameObject.CompareTag("Deadzone"))
            isDead = true;
    }

    private void OnParticleCollision(GameObject other)
    {
        //if it hits someone else don't react.
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;
        Spell spell = other.GetComponent<Spell>();

        //We only care about the movement in the x/z plane.
        Vector3 heading = transform.position - other.transform.position;
        heading.y *= 0f;

        //Increase the knockback multiplier.
        AddKnockback(heading, baseKnockback * multiplier);
    }
    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(isDead);
        }
        else
        {
            isDead = (bool)stream.ReceiveNext();
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
    private AnimationController animationController = null;
    private Vector3 impact = Vector3.zero;
    private float impactThreshold = 0.1f;
    private float baseKnockback = 10f;
    private float dissipationRate = 4f;
    private float multiplier = 1f;
    private bool isDead = false;
    #endregion PRIVATES

}
