using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Hero : MonoBehaviourPun
{
    public string Name => this.photonView.Owner.NickName;
    public float Multiplier => this.impactDetector.Multiplier;
    public float Height => this.characterController.height;

    [Tooltip("The hero's user interface prefab")]
    [SerializeField]
    private GameObject heroUserInterfacePrefab = null;

    [Tooltip("This object will be instantiated after attacking.")]
    [SerializeField]
    private GameObject spellPrefab = null;

    [Tooltip("This is the location where the object will be instantiated at.")]
    [SerializeField]
    private Transform spawnPoint = null;

    public void Move(Vector3 movement) => characterController.Move(movement);
    public void ShrinkSticker() => playerSticker.Shrink();
    #region CALLBACKS
    public void DeactivatePlayer(object sender, System.EventArgs e)
    {
        active = false;
        transform.position = new Vector3(0, -20f, 0);
        cameraWork.Detach();
        playerSticker.Detach();
        animationController.Moving(0);
        characterController.enabled = false;
        animationController.Animator.enabled = false;
    }
    #endregion CALLBACKS

    #region UNITY CALLBACKS
    private void Awake()
    {
        cameraWork = GetComponent<CameraWork>();
        if (!cameraWork)
            Debug.LogError("<Color=Red> Hero <a></a></Color>is missing a CameraWork component !! ", this);
        impactDetector = GetComponent<ImpactDetector>();
        if (!impactDetector)
            Debug.LogError("<Color=Red> Hero <a></a></Color>is missing an ImpactDetection component !! ", this);
        animationController = GetComponent<AnimationController>();
        if(!animationController)
            Debug.LogError("<Color=Red> Hero <a></a></Color>is missing an AnimationController component !! ", this);
        characterController = GetComponent<CharacterController>();
        if(!characterController)
            Debug.LogError("<Color=Red> Hero <a></a></Color>is missing a CharacterController component !! ", this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        impactDetector.OnDeath += DeactivatePlayer;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        impactDetector.OnDeath -= DeactivatePlayer;
    }

    private void Start()
    {
        active = true;
        transform.position = new Vector3(0, 5f, 0);

        if (photonView.IsMine && PhotonNetwork.IsConnected == true)
            cameraWork.OnStartFollowing();

        CreateSticker();
    }

    private void Update()
    {
        if (photonView.IsMine && active)
        {
            PlayerInput();
        }
    }

    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    [PunRPC]
    private void Primary(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        GameObject obj = Instantiate(spellPrefab, position, rotation) as GameObject;
        Spell spell = obj.GetComponent<Spell>();
        Spell.Link(spell, photonView.Owner);
    }
    #endregion ABSTRACT

    #region ANIMATION CALLBACKS
    private void FootL() { }

    private void FootR() { }

    private void Hit()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Primary", RpcTarget.AllViaServer, spawnPoint.position, spawnPoint.rotation);
        }
    }
    #endregion ANIMATION CALLBACKS

    #region PRIVATES
    private void PlayerInput()
    {
        bool attack = Input.GetButtonDown("Fire1");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v < 0)
            v = 0;
        float speed = (new Vector2(h, v).sqrMagnitude);

        animationController.Moving(speed);

        animationController.Rotating(h);

        animationController.Attack(attack);
    }

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
        if (!heroUserInterfacePrefab)
            Debug.LogError("<Color=Red> Hero <a></a></Color>is missing a mage user interface sticker prefab !! ", this);
        else
        {
            GameObject go = Instantiate(heroUserInterfacePrefab) as GameObject;
            playerSticker = go.GetComponent<PlayerSticker>();
            playerSticker.Link(this);
        }
    }

    private bool active = false;
    private PlayerSticker playerSticker = null;
    private CharacterController characterController = null;
    private CameraWork cameraWork = null;
    private ImpactDetector impactDetector = null;
    private AnimationController animationController = null;
    #endregion PRIVATES

}
