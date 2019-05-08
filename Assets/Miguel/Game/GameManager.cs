using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance = null;

    [Header("User Interface")]
    [Tooltip("")]
    [SerializeField]
    private Button exit = null;

    [Tooltip("")]
    [SerializeField]
    private Text information = null;

    [Header("Environment Timers")]
    [Tooltip("")]
    [SerializeField]
    private float gameDelay = 50f;

    [Tooltip("")]
    [SerializeField]
    private float paintDelay = 5f;

    [Tooltip("")]
    [SerializeField]
    private float roundDelay = 20f;


    #region UNITY CALLBACKS
    private void Awake()
    {
        Instance = this;
        environmentController = GetComponent<IEnvironmentController>();
        if (environmentController == null)
            Debug.LogError("<Color=Red> GameManager <a></a></Color>is missing an IEnvironmentController component !! ", this);

    }

    /// <summary>
    /// In order to spawn a character, prefab must have the same name as the Character Selections in Settings.
    /// </summary>
    private void Start()
    {
        StartGame();

        GameObject _go = PhotonNetwork.Instantiate(Settings.Instance.Character.ToString(), new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }

    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    public override void OnEnable()
    {
        base.OnEnable();
        exit.onClick.AddListener(OnClickLeave);
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        exit.onClick.RemoveListener(OnClickLeave);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("PUN is now leaving the room ... ");
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"MasterClient -- {newPlayer} has joined ! ");
            LoadArena();
        }
        else
        {
            Debug.Log($"PUN -- {newPlayer} has joined ! ");
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"MasterClient -- {otherPlayer} has quit ! ");
        }
        else
        {
            Debug.Log($"PUN -- {otherPlayer} has quit ! ");
        }
    }
    #endregion PUN CALLBACKS

    #region UI CALLBACKS
    public void OnClickLeave()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion UI CALLBACKS

    #region UTILITY
    private void LoadArena()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }

    private async void StartGame()
    {
        environmentController.Initialize();   
        await new WaitForSeconds(gameDelay);
        DriveEnvironment();
    }

    private async void DriveEnvironment()
    {
        bool hasNext = true;
        while (hasNext)
        {
            await new WaitForSeconds(roundDelay);
            hasNext = environmentController.Paint();
            environmentController.PaintRandom(); // create bounds on max size 
        
            await new WaitForSeconds(paintDelay);
            hasNext = environmentController.Release();
            environmentController.ReleaseRandom();

        }
        Debug.Log("Game Complete");
    }

    private List<Transform> players = new List<Transform>(20);
    private IEnvironmentController environmentController;
    #endregion UTILITY


}
