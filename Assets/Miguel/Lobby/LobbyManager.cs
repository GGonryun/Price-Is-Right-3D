using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login Panel")]
    [Tooltip("")]
    [SerializeField]
    private GameObject loginPanel;

    [Tooltip("")]
    [SerializeField]
    private InputField playerNameInputField;

    [Tooltip("")]
    [SerializeField]
    private Button loginButton;

    #region UNITY CALLBACKS
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        playerNameInputField.text = $"Player #{Random.Range(0, System.Int32.MaxValue)}";
    }
    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN has connected to master server ! ");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"PUN has disconnected w/ reason {cause}");
    }
    #endregion PUN CALLBACKS

    #region UI CALLBACKS
    public void OnClickLogin()
    {
        string playerName = playerNameInputField.text;

        if (playerName.Equals(""))
            return;

        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.GameVersion = _version;
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion UI CALLBACKS

    #region PRIVATES
    private const string _version = "1";
    #endregion PRIVATES
}
