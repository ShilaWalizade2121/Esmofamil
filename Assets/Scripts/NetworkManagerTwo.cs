using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class NetworkManagerTwo : MonoBehaviourPunCallbacks
{
    [Header("Login Field Panel")]
    public InputField loginField;
    [Header("Connection Status Panel")]
    public Text connectionText;

    [Header("Login Panel")]
    public GameObject loginPanel;
    [Header("Gameoptions Panel")]
    public GameObject gameOptionsPanel;
    [Header("Create Room Panel")]
    public GameObject createRoomPanel;
    [Header("Inside Room Panel")]
    public GameObject insideRoomPanel;
    public Text roomInfoText;
    [Header("Room List Panel")]
    public GameObject roomListPanel;
    private Dictionary<string, RoomInfo> cashedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;
    public GameObject roomListEntryPrefab;
    public GameObject roomListEntryParentGameObject;
    [Header("Join Radom Room List Panel")]
    public GameObject joinRandomListPanel;
    public GameObject playerListGameObject;
    public GameObject playerListGameObjectParent;
    public GameObject startGameBtn;
    [Header("Room Name Field")]
    public InputField roomNameField;
    [Header("Max Player Field")]
    public InputField maxPlayerField;
    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(loginPanel.name);
        cashedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        connectionText.text = "Connection Status : "+
            PhotonNetwork.NetworkClientState;
    }
    public void CreateRoom()
    {
        string roomName = roomNameField.text;
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room# " + Random.Range(0 , 1000);
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(maxPlayerField.text);
        PhotonNetwork.CreateRoom(roomName,roomOptions);

    }
    public void OnLoginBtnClicked() {
        string playerName = loginField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("Field is empty!");
                       
        }
    }
    public void CancelBtn()
    {
        ActivatePanel(gameOptionsPanel.name);
    }
    public override void OnConnected()
    {
        Debug.Log("Internet Connected");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " Connected to Master");
        ActivatePanel(gameOptionsPanel.name);
    }
    public override void OnCreatedRoom()
    {
        print(PhotonNetwork.CurrentRoom.Name + " Created");
    }
    public override void OnJoinedRoom()
    {
        print(PhotonNetwork.LocalPlayer.NickName + " Joined to : " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(insideRoomPanel.name);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameBtn.SetActive(true);
        }
        else
        {
            startGameBtn.SetActive(false);
        }
        roomInfoText.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name
            + " "+PhotonNetwork.CurrentRoom.PlayerCount +
            " / "+PhotonNetwork.CurrentRoom.MaxPlayers;
        if (playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, GameObject>();
        }
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            GameObject plListGameObject = Instantiate(playerListGameObject);
            plListGameObject.transform.
                SetParent(playerListGameObjectParent.transform);
            plListGameObject.transform.localScale = Vector3.one;

            plListGameObject.transform.Find("PlayerNameText").
                GetComponent<Text>().text = player.NickName;

            if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                plListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
            }
            else
            {
                plListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
            }
            playerListGameObjects.Add(player.ActorNumber, plListGameObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomInfoText.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name
         + " " + PhotonNetwork.CurrentRoom.PlayerCount +
         " / " + PhotonNetwork.CurrentRoom.MaxPlayers;


        GameObject plListGameObject = Instantiate(playerListGameObject);
        plListGameObject.transform.SetParent(playerListGameObjectParent.transform);
        plListGameObject.transform.localScale = Vector2.one;

        plListGameObject.transform.Find("PlayerNameText").GetComponent<Text>().text = newPlayer.NickName;
        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            plListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
        }
        else
        {
            plListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
        }
        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameBtn.SetActive(true);
        }
    }

    

    public override void OnLeftRoom()
    {
        roomInfoText.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name
         + " " + PhotonNetwork.CurrentRoom.PlayerCount +
         " / " + PhotonNetwork.CurrentRoom.MaxPlayers;


        ActivatePanel(gameOptionsPanel.name);
        foreach(GameObject plGameObject in playerListGameObjects.Values)
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


    public void ActivatePanel(string panelToBeActivated)
    {
        loginPanel.SetActive(panelToBeActivated.Equals(loginPanel.name));
        gameOptionsPanel.SetActive(panelToBeActivated.Equals(gameOptionsPanel.name));
        createRoomPanel.SetActive(panelToBeActivated.Equals(createRoomPanel.name));
        insideRoomPanel.SetActive(panelToBeActivated.Equals(insideRoomPanel.name));
        roomListPanel.SetActive(panelToBeActivated.Equals(roomListPanel.name));
        joinRandomListPanel.SetActive(panelToBeActivated.Equals(joinRandomListPanel.name));
    }

    public void OnShowRoomListBtnClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePanel(roomListPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();
        foreach (RoomInfo room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cashedRoomList.ContainsKey(room.Name))
                {
                    cashedRoomList.Remove(room.Name);
                }
            }
            else
            {
                if (cashedRoomList.ContainsKey(room.Name))
                {
                    cashedRoomList[room.Name] = room;
                }
                else
                {
                    cashedRoomList.Add(room.Name, room);
                }
            }
            
        }
        foreach(RoomInfo room in cashedRoomList.Values)
        {
            GameObject roomListEntryGameObject = Instantiate(roomListEntryPrefab);
            roomListEntryGameObject.transform.SetParent
                (roomListEntryParentGameObject.transform);
            roomListEntryGameObject.transform.localScale = Vector2.one;


            roomListEntryGameObject.transform.Find("RoomNameText")
                .GetComponent<Text>().text = room.Name;

            roomListEntryGameObject.transform.Find("RoomPlayersText")
                .GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers;

            roomListEntryGameObject.transform.Find("JoinRoomButton")
                .GetComponent<Button>().onClick.AddListener(() => 
                OnJoinRoomBtnClicked(room.Name));

            roomListGameObjects.Add(room.Name,roomListEntryGameObject);
        }
    }

    public override void OnLeftLobby()
    {
        ClearRoomListView();
        cashedRoomList.Clear();
    }

    public void onBackBtnClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivatePanel(gameOptionsPanel.name);
    }

    void ClearRoomListView() {
        foreach(var roomListGameObject in roomListGameObjects.Values)
        {
            Destroy(roomListGameObject);
        }

        roomListGameObjects.Clear();
    }

   void OnJoinRoomBtnClicked(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnJoinRandomRoomBtnClicked()
    {
        ActivatePanel(joinRandomListPanel.name);
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(roomName, roomOptions);

    }

    public void LoadGameLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}
