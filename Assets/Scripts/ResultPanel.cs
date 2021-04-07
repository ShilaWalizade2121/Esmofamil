using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;
using System;

public class ResultPanel : MonoBehaviour
{
    public GameObject playerListPrefab;
    public GameObject playerListParent;
    public GameObject WaitingPanel;
    public MainTableCtrl mainTable;
    //public GameObject resultPanel;

    // Start is called before the first frame update
    bool isShow;
    void Start()
    {
        WaitingPanel.SetActive(true);
        isShow = false;
       

    }
    private void Update()
    {
        if (mainTable.userScores.Count >= PhotonNetwork.PlayerList.Length && !isShow)
        {
            // GetComponent<PhotonView>().RPC("ActivateResultPanel", RpcTarget.All);
            Invoke("ActivateResultPanel", 1f);
            isShow = true;
        }
    }
    //[PunRPC]
    void ActivateResultPanel()
    {

        WaitingPanel.SetActive(false);
        mainTable.userScores = mainTable.userScores.OrderByDescending(x => Int32.Parse(x.Value[2])).ToDictionary(x => x.Key, x => x.Value);  //Dictionary<int, string> dctTemp = new Dictionary<int, string>();
        int i = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject plList = Instantiate(playerListPrefab);

            plList.transform.SetParent(playerListParent.transform);
            plList.transform.localScale = Vector3.one;
            if (i != 0)
            {
                plList.transform.Find("first-img").GetComponent<Image>().sprite = null;
                plList.transform.Find("first-img").GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
            if (mainTable.userScores.ContainsKey(player.ActorNumber - 1))
            {
                plList.transform.Find("name-txt").GetComponent<Text>().text = mainTable.userScores.Values.ElementAt(player.ActorNumber - 1)[1].faConvert();
                plList.transform.Find("score-txt").GetComponent<Text>().text = mainTable.userScores.Values.ElementAt(player.ActorNumber - 1)[2];  
            }
             i++;
        }
    }

    
}
