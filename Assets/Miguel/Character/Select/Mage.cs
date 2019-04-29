using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Mage : Hero
{
    [Tooltip("Spell")]
    [SerializeField]
    private GameObject spellPrefab = null;

    [Tooltip("Spell Spawn Point")]
    [SerializeField]
    private Transform spawnPoint = null;

    [PunRPC]
    protected override void Primary(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        GameObject obj = Instantiate(spellPrefab, position, rotation) as GameObject;
        Spell spell = obj.GetComponent<Spell>();
        Spell.Link(spell, photonView.Owner);
    }

    protected override void Hit()
    {
        if (photonView.IsMine)
        {
            Vector3 position = spawnPoint.position;
            Quaternion rotation = spawnPoint.rotation;
            photonView.RPC("Primary", RpcTarget.AllViaServer, position, rotation);
        }
    }

    protected override float Secondary()
    {
        return 1f;
    }
}
