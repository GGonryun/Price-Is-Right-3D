using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviourPun, IPunObservable
{
    #region UNITY CALLBACKS
    private void Awake()
    {
        animationController = GetComponent<AnimationController>();
        if (!animationController)
            Debug.LogError("<Color=Red> Mage <a></a></Color> has no component named AnimationController !! ", this);

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

        if (photonView.IsMine)
        {
            PlayerInput();
        }
    }

    private void PlayerInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v < 0)
            v = 0;

        animationController.PlayerInput(new Vector2(h, v));
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
    #endregion PUN CALLBACKS

    #region UTILITY
    void FootL()
    {
        Debug.Log("FootL");
    }

    void FootR()
    {
        Debug.Log("FootR");
    }

    void Hit()
    {
        Debug.Log("Throw fireball!");
    }
    #endregion UTILITY

    private bool dead = false;
    private AnimationController animationController = null;
    private CameraWork cameraWork = null;

}
