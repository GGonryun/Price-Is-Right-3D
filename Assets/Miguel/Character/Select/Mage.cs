using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CameraWork))]
public class Mage : MonoBehaviourPun
{
    //TODO: ADD MULTIPLE SPELLS
    [Tooltip("Spell")]
    [SerializeField]
    private GameObject spellPrefab;

    [Tooltip("Spell Spawn Point")]
    [SerializeField]
    private Transform spawnPoint;

    [Tooltip("")]
    [SerializeField]
    private GameObject mageUI = null;

    public string Name => this.photonView.Owner.NickName;
    public float Multiplier => this.impactDetector.Multiplier;
    public float Height => this.impactDetector.Height;

    #region UNITY CALLBACKS
    private void Awake()
    {
        cameraWork = GetComponent<CameraWork>();
        if (!cameraWork)
            Debug.LogError("<Color=Red> Mage <a></a></Color>is missing a CameraWork component !! ", this);
        impactDetector = GetComponent<ImpactDetector>();
        if(!impactDetector)
            Debug.LogError("<Color=Red> Mage <a></a></Color>is missing an ImpactDetection component !! ", this); 
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
        CreateSticker();

        if (photonView.IsMine && PhotonNetwork.IsConnected == true)
            cameraWork.OnStartFollowing();
    }

    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    [PunRPC]
    public void Cast(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        GameObject obj = Instantiate(spellPrefab, position, rotation) as GameObject;
        Spell spell = obj.GetComponent<Spell>();
        Spell.Link(spell, photonView.Owner);
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
            photonView.RPC("Cast", RpcTarget.AllViaServer, position, rotation);
        }
    }
    #endregion ANIMATION CALLBACKS

    #region PRIVATES
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        Debug.Log("Loading Screen!");
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }

    }

    private void CreateSticker()
    {
        if (!mageUI)
            Debug.LogError("<Color=Red> GameManager <a></a></Color>is missing a mage user interface sticker prefab !! ", this);
        else
        {
            GameObject go = Instantiate(mageUI) as GameObject;
            PlayerSticker ps = go.GetComponent<PlayerSticker>();
            ps.Link(this);
        }
    }

    private float cooldown = 0.0f;
    private CameraWork cameraWork = null;
    private ImpactDetector impactDetector = null;
    #endregion PRIVATES
}
