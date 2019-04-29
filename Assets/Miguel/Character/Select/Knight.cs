using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Hero
{
    [Tooltip("")]
    [SerializeField]
    private GameObject hitboxPrefab = null;

    [Tooltip("")]
    [SerializeField]
    private Transform hitboxPoint = null;

    [PunRPC]
    protected override void Primary(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        GameObject obj = Instantiate(hitboxPrefab, position, rotation);
        Spell spell = obj.GetComponent<Spell>();
        Spell.Link(spell, info.photonView.Owner);
    }

    protected override void Hit()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Primary", RpcTarget.AllViaServer, hitboxPoint.position, hitboxPoint.rotation);
        }
    }

    protected override float Secondary()
    {
        return 1f;
    }
}
