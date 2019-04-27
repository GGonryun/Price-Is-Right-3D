using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mage : MonoBehaviourPunCallbacks, IPunObservable
{
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

    #region UTILITY

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    private void FootL() { }

    private void FootR() { }

    private void Hit()
    {
        Debug.Log("Throw fireball!");
    }
    #endregion UTILITY

    private bool dead = false;
    private CameraWork cameraWork = null;

}
