using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;
    public Text playerNameText;
    public Text noRoomText;
    [Header("Login field panel")]
    public InputField loginField;

    public Text connectionText;
    public GameObject leaderboardPanel;
    [Header("Login  panel")]
    public GameObject loginPanel;
    [Header("Register  panel")]
    public GameObject createPanel;
    [Header("GameOptions panel")]
    public GameObject GameOptionsPanel;

    [Header("CreateRoom panel")]
    public GameObject CreateRoomPanel;


    [Header("InsideRoom panel")]
    public GameObject InsideRoomPanel;
    public Text roomInfoTxt;
    public Text roomNumberPlayer;
    [Header("RoomList panel")]
    public GameObject RoomListPanel;

    [Header("JoinRandomRoom panel")]
    public GameObject JoinRandomRoomPanel;

    [Header("roomName")]
    public InputField roomNameField;

    [Header("max player")]
    public InputField maxPlayer;

    private Dictionary<string, RoomInfo> chasedRoomList;

    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;
    public GameObject roomListEntryPrefab;

    public GameObject roomListEntryParentGameObject;
    public GameObject playerListGameObject;
    public GameObject playerListParentGameObject;
    public GameObject startBtn;
    public Text infoJoinRoomText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void ActivePanel(string panelToBeActiveted)
    {
        leaderboardPanel.SetActive(panelToBeActiveted.Equals(leaderboardPanel.name));

        createPanel.SetActive(panelToBeActiveted.Equals(createPanel.name));

        loginPanel.SetActive(panelToBeActiveted.Equals(loginPanel.name));

        GameOptionsPanel.SetActive(panelToBeActiveted.Equals(GameOptionsPanel.name));

        CreateRoomPanel.SetActive(panelToBeActiveted.Equals(CreateRoomPanel.name));

        InsideRoomPanel.SetActive(panelToBeActiveted.Equals(InsideRoomPanel.name));

        RoomListPanel.SetActive(panelToBeActiveted.Equals(RoomListPanel.name));

        JoinRandomRoomPanel.SetActive(panelToBeActiveted.Equals(JoinRandomRoomPanel.name));

    }






    public void OnLoginBtnClicked()
    {

        string playerName = loginField.text;

        if (!string.IsNullOrEmpty(playerName))
        {

            PhotonNetwork.LocalPlayer.NickName = playerName;
            playerNameText.text = playerName.faConvert();
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {


            connectionText.text = Fa.faConvert("نام تان را وارد نمایید!");
            connectionText.color = Color.red;
            connectionText.fontStyle = FontStyle.Bold;

        }

    }

    void Start()
    {

        PhotonNetwork.SendRate = 40;
        PhotonNetwork.SerializationRate = 40;
        ActivePanel(loginPanel.name);

        chasedRoomList = new Dictionary<string, RoomInfo>();

        roomListGameObjects = new Dictionary<string, GameObject>();
        PhotonNetwork.AutomaticallySyncScene = true;
        
    }

    void Update()
    {

        if (PhotonNetwork.NetworkClientState == ClientState.ConnectingToMasterServer || PhotonNetwork.NetworkClientState==ClientState.ConnectingToNameServer)
        {
            connectionText.text = Fa.faConvert(" در حال اتصال...");
            connectionText.color = Color.black;
            connectionText.fontStyle = FontStyle.Bold;
        }
        
      

    }

    public override void OnConnected()
    {

    }

    public override void OnConnectedToMaster()
    {
        connectionText.text = Fa.faConvert(" ");
        ActivePanel(GameOptionsPanel.name);
    }
    public void OnJoinRandomBtnClicked()
    {
        ActivePanel(JoinRandomRoomPanel.name);
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void CreateRoom()
    {

        string roomName = roomNameField.text;

        if (string.IsNullOrEmpty(roomName) )
        {

            roomName = "" + Random.Range(0, 1000);
        }
        RoomOptions roomOptions = new RoomOptions();
        if (string.IsNullOrEmpty(maxPlayer.text))
        {
            roomOptions.MaxPlayers = 20;
        }
        else
        {
            roomOptions.MaxPlayers = (byte)int.Parse(maxPlayer.text);
        }
        
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);
        if (PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.PlayerList.Length>1)
        {
            startBtn.SetActive(true);
        }
    }
    public void LoadGameLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("MainTable");
        }
    }
    public override void OnLeftRoom()
    {
        //roomInfoTxt.text = "نام اتاق : " + PhotonNetwork.CurrentRoom.Name + "    " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        ActivePanel(GameOptionsPanel.name);
        foreach (GameObject plGameObject in playerListGameObjects.Values)
        {
            Destroy(plGameObject);
        }
        playerListGameObjects.Clear();
        playerListGameObjects = null;
    }
    public void OnLeaveBtnClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {

        print(PhotonNetwork.CurrentRoom.Name + "created");

    }
    
    public override void OnJoinedRoom()
    {
        
        ActivePanel(InsideRoomPanel.name);
        if (PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.PlayerList.Length>1)
        {
            infoJoinRoomText.gameObject.SetActive(false);
            startBtn.SetActive(true);
        }
        else
        {
            infoJoinRoomText.gameObject.SetActive(true);
            startBtn.SetActive(false);
        }
        roomInfoTxt.text = PhotonNetwork.CurrentRoom.Name;
        roomNumberPlayer.text = "    " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        if (playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, GameObject>();
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject plList = Instantiate(playerListGameObject);
            plList.transform.SetParent(playerListParentGameObject.transform);
            plList.transform.localScale = Vector3.one;
            plList.transform.Find("PlayerNameText").GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName.faConvert();

            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                plList.transform.Find("PlayerIndicator").gameObject.SetActive(true);
            }
            else
            {
                plList.transform.Find("PlayerIndicator").gameObject.SetActive(false);
            }
            playerListGameObjects.Add(player.ActorNumber, plList);
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomInfoTxt.text = PhotonNetwork.CurrentRoom.Name;
        roomNumberPlayer.text = "    " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        GameObject plList = Instantiate(playerListGameObject);
        plList.transform.SetParent(playerListParentGameObject.transform);
        plList.transform.localScale = Vector3.one;
        plList.transform.Find("PlayerNameText").GetComponent<Text>().text = newPlayer.NickName.faConvert();
        if (PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
        {
            startBtn.SetActive(true);
            infoJoinRoomText.gameObject.SetActive(false);
        }
        else
        {
            startBtn.SetActive(false);
            infoJoinRoomText.gameObject.SetActive(true);
        }
        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            plList.transform.Find("PlayerIndicator").gameObject.SetActive(true);
        }
        else
        {
            plList.transform.Find("PlayerIndicator").gameObject.SetActive(false);
        }
        playerListGameObjects.Add(newPlayer.ActorNumber, plList);
    }
  
    public void CancelBtn()
    {


        ActivePanel(GameOptionsPanel.name);


    }
    public void OnShowRoomListBtnClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivePanel(RoomListPanel.name);

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count >0)
        {
            noRoomText.gameObject.SetActive(false);
        }
        else if (roomList.Count <= 0)
        {
            noRoomText.gameObject.SetActive(true);
        }

        print("Rooms" +PhotonNetwork.CountOfRooms);
        ClearRoomListView();
        foreach (RoomInfo room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (chasedRoomList.ContainsKey(room.Name))
                {
                    chasedRoomList.Remove(room.Name);
                }

            }
            else
            {
                if (chasedRoomList.ContainsKey(room.Name))
                {
                    chasedRoomList[room.Name] = room;
                }
                else
                {
                    chasedRoomList.Add(room.Name, room);
                }
            }

        }
        foreach (RoomInfo room in chasedRoomList.Values)
        {
            GameObject roomListEntryGameObject = Instantiate(roomListEntryPrefab);
            Debug.Log("Rooms");

            roomListEntryGameObject.transform.SetParent(roomListEntryParentGameObject.transform);

            roomListEntryGameObject.transform.localScale = Vector3.one;

            roomListEntryGameObject.transform.Find("RoomNameText").GetComponent<Text>().text = room.Name;

            roomListEntryGameObject.transform.Find("RoomPlayersText").GetComponent<Text>().text = room.PlayerCount + "/" + room.MaxPlayers;

            roomListEntryGameObject.transform.Find("JoinRoomButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomBtnClicked(room.Name));

            roomListGameObjects.Add(room.Name, roomListEntryGameObject);
        }

    }
    void OnJoinRoomBtnClicked(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(roomName);
    }

    void ClearRoomListView()
    {
        foreach (var roomListGameObject in roomListGameObjects.Values)
        {
            Destroy(roomListGameObject);
        }
        roomListGameObjects.Clear();
    }
    public override void OnLeftLobby()
    {
        ClearRoomListView();
        chasedRoomList.Clear();
    }
    public void OnBackBtnClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();

        }
        ActivePanel(GameOptionsPanel.name);
    }
    public void DisconnectBtnClicked()
    {
        PhotonNetwork.Disconnect();
    }







}









