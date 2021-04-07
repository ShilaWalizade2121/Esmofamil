using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCtrl : MonoBehaviourPun
{
    // Start is called before the first frame update
    public GameObject bg;
    MainTableCtrl mainTableCtrl;
    public GameObject infoPanel;
    public GameObject mainPanel;
    public Text [] infoText;
    public InputField [] inputFieldText;
    void Start()
    {
        mainTableCtrl = GameObject.Find("MainTable").GetComponent<MainTableCtrl>();
        if (photonView.IsMine)
        {
            bg.SetActive(true);
            GameObject.Find("Btn_Stop").GetComponent<Button>().onClick.AddListener(mainTableCtrl.StopBtnClicked);
        }
        else
        {
            bg.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
