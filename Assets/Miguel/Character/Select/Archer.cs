using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Hero
{
    [Tooltip("")]
    [SerializeField]
    private GameObject arrowPrefab = null;

    [Tooltip("")]
    [SerializeField]
    private Transform arrowSpawnPoint = null;

    [PunRPC]
    protected override void Primary(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        GameObject obj = Instantiate(arrowPrefab, position, rotation);
        Spell spell = obj.GetComponent<Spell>();
        Spell.Link(spell, info.photonView.Owner);
    }

    protected override void Hit()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Primary", RpcTarget.AllViaServer, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        }
    }

    protected override float Secondary()
    {
        return 1f;
    }
}
