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
    [PunRPC]
    public void FireSpell(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        Debug.Log($"Send Firespell: {position}, {rotation}");
        Debug.Log($"Photon Message: {info.Sender}, {info.photonView}, {info.SentServerTime}");
    }

    private void Hit()
    {
        photonView.RPC("FireSpell", RpcTarget.AllViaServer, transform.position, transform.rotation);
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





    //private void Fire()
    //{
    //    float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

    //    GameObject go = Instantiate(spell, transform.position, Quaternion.identity, this.transform);
    //    go.GetComponent<Spell>().Initialize(photonView.Owner, (rotation * Vector3.forward));
    //}

    private float cooldown = 0.0f;
    private bool dead = false;
    private CameraWork cameraWork = null;
    #endregion UTILITY
}
