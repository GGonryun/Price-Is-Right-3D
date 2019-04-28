using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CameraWork))]
public class Mage : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("Spell")]
    [SerializeField]
    private GameObject spellPrefab;

    #region UNITY CALLBACKS
    private void Awake()
    {
        cameraWork = GetComponent<CameraWork>();
        if (!cameraWork)
            Debug.LogError("<Color=Red> Mage <a></a></Color>has no component named CameraWork !! ", this);
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            cameraWork.OnStartFollowing();
        }
    }

    private void Update()
    {
        if (dead == true)
            GameManager.Instance.OnClickLeave();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;
        if (!other.gameObject.CompareTag("Deadzone"))
            return;
        Debug.Log("Entered");
        dead = true;
    }
    #endregion UNITY CALLBACKS

    private void Hit()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("FireSpell", RpcTarget.AllViaServer, transform.position, transform.rotation);
        }
    }

    [PunRPC]
    public void FireSpell(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject spell = Instantiate(spellPrefab, position, rotation) as GameObject;
        spell.GetComponent<Spell>().Initialize(photonView.Owner);
    }
    #region PUN CALLBACKS


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(dead);
        }
        else
        {
            dead = (bool)stream.ReceiveNext();
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion PUN CALLBACKS

    #region PRIVATES

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    private void FootL() { }

    private void FootR() { }

    private float cooldown = 0.0f;
    private bool dead = false;
    private CameraWork cameraWork = null;
    #endregion UTILITY
}
