using System;
using Photon.Pun;
using UnityEngine;

public delegate void OnDeathEventHandler(object sender, EventArgs e);

public class ImpactDetector : MonoBehaviourPun, IPunObservable
{
    public float Multiplier => multiplier;

    public event OnDeathEventHandler OnDeath { add => onDeath += value; remove => onDeath -= value; }

    [Tooltip("")]
    [SerializeField]
    private float baseKnockback = 10f;

    #region UNITY CALLBACKS
    private void Awake()
    {
        hero = gameObject.GetComponent<Hero>();
        if (!hero)
            Debug.LogError("<Color=Red> ImpactDetector <a></a></Color>is missing a Hero component !! ", this);
    }

    private void Update()
    {
        ConsumeImpact();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Deadzone"))
            onDeath?.Invoke(this, new EventArgs());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        if (other.gameObject.CompareTag("Sword"))
            multiplier = CalculateKnockback(other.transform, swordMultiplier, swordInfluence);
        if (other.gameObject.CompareTag("Arrow"))
            multiplier = CalculateKnockback(other.transform, arrowMultiplier, arrowInfluence);
        if (other.gameObject.CompareTag("TwoHand"))
            multiplier = CalculateKnockback(other.transform, twoHandMultiplier, twoHandInfluence);
    }

    private void OnParticleCollision(GameObject other)
    {
        //if it hits someone else don't react.
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;
        if (other.gameObject.CompareTag("Spell"))
            multiplier = CalculateKnockback(other.transform, spellMultiplier, spellInfluence);
    }

    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(multiplier);
        }
        else
        {
            multiplier = (float)stream.ReceiveNext();
        }
    }
    #endregion PUN CALLBACKS

    #region PRIVATES
    private float CalculateKnockback(Transform target, float multiplier, float influence)
    {
        Vector3 heading = target.forward;
        heading.y *= 0f;
        currentMultiplier += multiplier;
        AddKnockback(heading, baseKnockback * influence * currentMultiplier);
        return currentMultiplier;
    }

    private void AddKnockback(Vector3 direction, float force)
    {
        Vector3 _dir = direction.normalized;
        impact += _dir * force;
    }

    private void ConsumeImpact()
    {
        if (impact.sqrMagnitude > impactThreshold)
        {
            hero.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, dissipationRate * Time.deltaTime);
        }
    }

    private OnDeathEventHandler onDeath;
    private float multiplier = 0;
    private Hero hero = null;
    private Vector3 impact = Vector3.zero;
    private float impactThreshold = 0.1f;
    private float dissipationRate = 5f;
    private float currentMultiplier = 1f;

    //Modify these values if you want the character taking damage to be influenced more heavily by the collision.
    //1 = 100%
    private float arrowInfluence = .5f;
    private float swordInfluence = 1.1f;
    private float twoHandInfluence = 1.5f;
    private float spellInfluence = 1.0f;

    //These values represent how much we will increment our multiplier by each collision.
    private float arrowMultiplier = .25f;
    private float swordMultiplier = .5f;
    private float twoHandMultiplier = .9f;
    private float spellMultiplier = .5f;
    #endregion PRIVATES

}
