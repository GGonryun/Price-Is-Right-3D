using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region MAIN MENU
    [Header("Main Menu Panel")]
    [Tooltip("")]
    [SerializeField]
    private GameObject mainMenuPanel = null;
    [Tooltip("")]
    [SerializeField]
    private Button mainMenuPlayButton = null;
    [Tooltip("")]
    [SerializeField]
    private Button mainMenuCreditsButton = null;
    [Tooltip("")]
    [SerializeField]
    private Button mainMenuInstructionsButton = null;
    [Tooltip("")]
    [SerializeField]
    private Button mainMenuExitButton = null;
    #endregion
    #region LOGIN
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
    #endregion
    #region JOIN ROOM
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
    #endregion
    #region ACTIVE ROOM
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
    #endregion
    [Tooltip("")]
    [SerializeField]
    private Dropdown characterSelectDropdown = null;
    #region INSTRUCTIONS
    [Header("Instructions Room Panel")]
    [Tooltip("")]
    [SerializeField]
    private GameObject instructionsRoomPanel = null;
    [Tooltip("")]
    [SerializeField]
    private Button instructionsExitButton = null;
    #endregion
    #region CREDITS
    [Header("Credits Panel")]
    [Tooltip("")]
    [SerializeField]
    private GameObject creditPanel = null;
    [Tooltip("")]
    [SerializeField]
    private Button creditsExitButton = null;
    #endregion

    #region UNITY CALLBACKS

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        playerNameInputField.text = $"Player #{Random.Range(0, System.Int32.MaxValue)}";
        SetActivePanel(mainMenuPanel.name);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        mainMenuPlayButton.onClick.AddListener(OnClickStart);
        mainMenuCreditsButton.onClick.AddListener(OnCreditsSelected);
        mainMenuInstructionsButton.onClick.AddListener(OnInstructionsSelected);
        mainMenuExitButton.onClick.AddListener(OnQuitGame);

        loginButton.onClick.AddListener(OnClickLogin);
        joinRoomButton.onClick.AddListener(OnClickJoin);
        enterGameButton.onClick.AddListener(OnClickEnter);
        characterSelectDropdown.onValueChanged.AddListener(OnCharacterSelected);

        instructionsExitButton.onClick.AddListener(OnReturnToMenu);
        creditsExitButton.onClick.AddListener(OnReturnToMenu);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        mainMenuPlayButton.onClick.RemoveListener(OnClickStart);
        mainMenuCreditsButton.onClick.RemoveListener(OnCreditsSelected);
        mainMenuInstructionsButton.onClick.RemoveListener(OnInstructionsSelected);
        mainMenuExitButton.onClick.RemoveListener(OnQuitGame);

        loginButton.onClick.RemoveListener(OnClickLogin);
        joinRoomButton.onClick.RemoveListener(OnClickJoin);
        enterGameButton.onClick.RemoveListener(OnClickEnter);
        characterSelectDropdown.onValueChanged.RemoveListener(OnCharacterSelected);

        instructionsExitButton.onClick.RemoveListener(OnReturnToMenu);
        creditsExitButton.onClick.RemoveListener(OnReturnToMenu);
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
    public void OnClickStart()
    {
        SetActivePanel(loginPanel.name);
    }

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

    public void OnCharacterSelected(int index) => Settings.Instance.Character = Characters.Get(characterSelectDropdown.options[index].text);
    
    public void OnCreditsSelected()
    {
        SetActivePanel(creditPanel.name);
    }

    public void OnInstructionsSelected()
    {
        SetActivePanel(creditPanel.name);
    }

    public void OnReturnToMenu()
    {
        SetActivePanel(mainMenuPanel.name);
    }

    public void OnQuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("we quit boyz");
#else
        Application.Quit();
#endif
    }
#endregion UI CALLBACKS

#region PRIVATES
    private void SetActivePanel(string name)
    {
        mainMenuPanel.SetActive(name.Equals(mainMenuPanel.name));
        loginPanel.SetActive(name.Equals(loginPanel.name));
        joinRoomPanel.SetActive(name.Equals(joinRoomPanel.name));
        activeRoomPanel.SetActive(name.Equals(activeRoomPanel.name));
        instructionsRoomPanel.SetActive(name.Equals(instructionsRoomPanel.name));
        creditPanel.SetActive(name.Equals(creditPanel.name));
    }

    private void CreateRoom()
    {
        if (PhotonNetwork.CountOfRooms >= maxRooms)
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


    private const string _version = "3";
    private const short _errorRoomDoesNotExist = 32760;
    [System.NonSerialized] private Dictionary<string, Text> playerList = new Dictionary<string, Text>(8);
#endregion PRIVATES
}
