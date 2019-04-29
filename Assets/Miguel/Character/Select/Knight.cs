using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Hero
{
    [Tooltip("")]
    [SerializeField]
    private GameObject hitbox = null;

    [PunRPC]
    protected async override void Primary(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        hitbox.SetActive(true);
        await new WaitForSeconds(0.4f);
        hitbox.SetActive(false);
    }

    protected override void Hit()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Primary", RpcTarget.AllViaServer, Vector3.zero, Quaternion.identity);
        }
    }

    protected override float Secondary()
    {
        return 1f;
    }


}
