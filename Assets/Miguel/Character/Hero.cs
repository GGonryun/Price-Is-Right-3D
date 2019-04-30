using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Hero : MonoBehaviourPun
{
    public string Name => this.photonView.Owner.NickName;
    public float Multiplier => this.impactDetector.Multiplier;
    public float Height => this.impactDetector.Height;

    [Tooltip("The hero's user interface prefab")]
    [SerializeField]
    private GameObject heroUserInterfacePrefab = null;

    [Tooltip("This object will be instantiated after attacking.")]
    [SerializeField]
    private GameObject spellPrefab = null;

    [Tooltip("This is the location where the object will be instantiated at.")]
    [SerializeField]
    private Transform spawnPoint = null;

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
        SetRandomColor();

        if (photonView.IsMine && PhotonNetwork.IsConnected == true)
            cameraWork.OnStartFollowing();
    }

    private void SetRandomColor()
    {
        Renderer _renderer = transform.GetChild(0).GetComponent<Renderer>();
        MaterialPropertyBlock _block = new MaterialPropertyBlock();
        int _id = Shader.PropertyToID("_Color");

        _block.SetColor(_id, UnityEngine.Random.ColorHSV(0,1));
        _renderer.SetPropertyBlock(_block);

    }

    private void Update()
    {
        if (photonView.IsMine)
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
            PlayerSticker ps = go.GetComponent<PlayerSticker>();
            ps.Link(this);
        }
    }

    private CameraWork cameraWork = null;
    private ImpactDetector impactDetector = null;
    private AnimationController animationController = null;
    #endregion PRIVATES

}
