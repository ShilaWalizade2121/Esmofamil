using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject playerGameobject;
    public static PlayerManager instance;
    MainTableCtrl mainTableCtrl;
    //public InputField[] inputFieldText = new InputField[9];
    // public Text[] infoText = new Text[9];

    void Start()
    {
        mainTableCtrl = GameObject.FindGameObjectWithTag("MainTable").GetComponent<MainTableCtrl>();
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            spawn();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // Create a new room
        PhotonNetwork.CreateRoom("test");
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnJoinedRoom()
    {        
        base.OnJoinedRoom();
        spawn();
    }

    void spawn()
    {
        if (playerGameobject != null)
        {
            if (PhotonNetwork.IsConnected)
            {
                
                GameObject pl = PhotonNetwork.Instantiate(playerGameobject.name, Vector3.zero, Quaternion.identity);
                mainTableCtrl.localPlayer = pl.GetComponent<PhotonView>().Owner;
                    //mainTableCtrl.GetComponent<MainTableCtrl>().playerCtrls[PhotonNetwork.LocalPlayer.ActorNumber-1] = pl;               
            }
        }
    }

}
