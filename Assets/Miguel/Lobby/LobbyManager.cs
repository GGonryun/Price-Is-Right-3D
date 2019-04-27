using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login Panel")]
    [Tooltip("")]
    [SerializeField]
    private GameObject loginPanel = null;
    [Tooltip("")]
    [SerializeField]
    private InputField playerNameInputField = null;
    [Tooltip("")]
    [SerializeField]
    private Button loginButton = null;

    [Header("Join Room Panel")]
    [Tooltip("")]
    [SerializeField]
    private byte maxPlayers = 8;
    [Tooltip("")]
    [SerializeField]
    private byte maxRooms = 1;
    [Tooltip("")]
    [SerializeField]
    private GameObject joinRoomPanel = null;
    [Tooltip("")]
    [SerializeField]
    private Button joinRoomButton = null;

    [Header("Active Room Panel")]
    [Tooltip("")]
    [SerializeField]
    private GameObject activeRoomPanel = null;
    [Tooltip("")]
    [SerializeField]
    private Button enterGameButton = null;
    [Tooltip("Sets the Vertical Layout Group as the parent of our Player Name Prefabs.")]
    [SerializeField]
    private VerticalLayoutGroup playerNameVerticalGroup = null;
    [Tooltip("")]
    [SerializeField]
    private Text playerNameTextPrefab = null;

    #region UNITY CALLBACKS
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        playerNameInputField.text = $"Player #{Random.Range(0, System.Int32.MaxValue)}";
        SetActivePanel(loginPanel.name);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        loginButton.onClick.AddListener(OnClickLogin);
        joinRoomButton.onClick.AddListener(OnClickJoin);
        enterGameButton.onClick.AddListener(OnClickEnter);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        loginButton.onClick.RemoveListener(OnClickLogin);
        joinRoomButton.onClick.RemoveListener(OnClickJoin);
        enterGameButton.onClick.RemoveListener(OnClickEnter);
    }
    #endregion UNITY CALLBACKS

    #region PUN CALLBACKS
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN has connected to master server . ");
        SetActivePanel(joinRoomPanel.name);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"PUN has disconnected w/ reason: {cause} ! ");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"{returnCode}: PUN says: {message} . ");

        if (returnCode == _errorRoomDoesNotExist)
        {
            CreateRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We have joined a room . ");
        SetActivePanel(activeRoomPanel.name);

        CreatePlayerList();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("We have left the room . ");

        ClearPlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"PUN the player {newPlayer.NickName} has entered the room ! ");

        AddPlayerListEntry(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"PUN the player {otherPlayer.NickName} has left the room . ");

        RemovePlayerListEntry(otherPlayer.NickName);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("PUN has created a new room . ");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"{returnCode}: PUN has failed to create a room ! {message} . ");
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

    public void OnClickJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClickEnter()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
    #endregion UI CALLBACKS

    #region UTILITY
    private void SetActivePanel(string name)
    {
        loginPanel.SetActive(name.Equals(loginPanel.name));
        joinRoomPanel.SetActive(name.Equals(joinRoomPanel.name));
        activeRoomPanel.SetActive(name.Equals(activeRoomPanel.name));
    }

    private void CreateRoom()
    {
        if (PhotonNetwork.CountOfRooms > maxRooms)
        {
            Debug.LogWarning($"Cannot create more rooms ! ");
            return;
        }

        Debug.Log("Creating a room now ... ");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers });
    }

    private void CreatePlayerList()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            AddPlayerListEntry(player);
        }

    }

    private void AddPlayerListEntry(Player player)
    {
        Text t = Instantiate(playerNameTextPrefab);
        t.transform.SetParent(playerNameVerticalGroup.transform);
        t.transform.localScale = Vector3.one;

        t.text = player.NickName;
        playerList.Add(player.NickName, t);
    }

    private void ClearPlayerList()
    {
        foreach (string playerName in playerList.Keys)
        {
            RemovePlayerListEntry(playerName);
        }
    }

    private void RemovePlayerListEntry(string playerName)
    {
        GameObject textBox = playerList[playerName].gameObject;
        Destroy(textBox);
        playerList.Remove(playerName);
    }

    #endregion

    #region PRIVATE
    private const string _version = "1";
    private const short _errorRoomDoesNotExist = 32760;
    [System.NonSerialized] private Dictionary<string, Text> playerList = new Dictionary<string, Text>(8);
    #endregion PRIVATE
}
