using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Hero : MonoBehaviourPun
{
    public string Name => this.photonView.Owner.NickName;
    public float Multiplier => this.impactDetector.Multiplier;
    public float Height => this.impactDetector.Height;

    [Tooltip("The hero's user interface prefab")]
    [SerializeField]
    protected GameObject heroUserInterfacePrefab = null;

    #region UNITY CALLBACKS
    protected virtual void Awake()
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

    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected virtual void Start()
    {
        CreateSticker();

        if (photonView.IsMine && PhotonNetwork.IsConnected == true)
            cameraWork.OnStartFollowing();
    }

    protected virtual void Update()
    {
        if (photonView.IsMine)
        {
            PlayerInput();
        }
    }

    #endregion UNITY CALLBACKS

    #region ASTRACT
    protected abstract void Primary(Vector3 position, Quaternion rotation, PhotonMessageInfo info);
    protected abstract void Hit();
    protected abstract float Secondary();
    #endregion ABSTRACT

    #region ANIMATION CALLBACKS
    private void FootL() { }

    private void FootR() { }


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
        
        animationController.Defend(Secondary());
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
