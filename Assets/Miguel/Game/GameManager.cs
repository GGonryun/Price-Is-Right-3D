using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{

    [Header("User Interface")]
    [Tooltip("")]
    [SerializeField]
    private Button exit = null;

    #region UNITY CALLBACKS
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// In order to spawn a character, prefab must have the same name as the Character Selections in Settings.
    /// </summary>
    private void Start()
    {
        StartGame();

        PhotonNetwork.Instantiate(Settings.Instance.Character.ToString(), new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
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

    void LoadArena()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
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
    public void StartGame()
    {
        Debug.Log("Starting the game ... ");
    }
    #endregion UTILITY

    public static GameManager Instance = null;

}
