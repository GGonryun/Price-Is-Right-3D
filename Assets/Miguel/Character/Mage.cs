using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CameraWork))]
public class Mage : MonoBehaviourPun
{
    [Tooltip("Spell")]
    [SerializeField]
    private GameObject spellPrefab;

    [Tooltip("Spell Spawn Point")]
    [SerializeField]
    private Transform spawnPoint;
    
    #region UNITY CALLBACKS
    private void Awake()
    {
        cameraWork = GetComponent<CameraWork>();
        if (!cameraWork)
            Debug.LogError("<Color=Red> Mage <a></a></Color>has no component named CameraWork !! ", this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected == true)
            cameraWork.OnStartFollowing();
    }
    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    [PunRPC]
    public void FireSpell(Vector3 position, Quaternion rotation)
    {
        GameObject spell = Instantiate(spellPrefab, position, rotation) as GameObject;
        spell.GetComponent<Spell>().Initialize(photonView.Owner);
    }
    #endregion PUN CALLBACKS

    #region ANIMATION CALLBACKS
    private void FootL() { }

    private void FootR() { }

    private void Hit()
    {
        if(photonView.IsMine)
        {
            Vector3 position = spawnPoint.position;
            Quaternion rotation = spawnPoint.rotation;
            photonView.RPC("FireSpell", RpcTarget.AllViaServer, position, rotation);
        }
    }
    #endregion ANIMATION CALLBACKS

    #region PRIVATES
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    private float cooldown = 0.0f;
    private CameraWork cameraWork = null;
    #endregion PRIVATES
}
