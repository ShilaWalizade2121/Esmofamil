
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using System.Globalization;
using System.Text;
using System.Linq;

public class MainTableCtrl : MonoBehaviourPunCallbacks
{
   public PlayfabManager playfabManager;
    //public Text[] pNames;
    //public Text[] pScores;
    public Text voteText;
    public GameObject playerListPrefab;
    //public Button ShowResultBtn;
    public Button checkBTN;
    public GameObject playerListParent;
    public Text playerName;
    public Text generatedLetter;
    public Text generatedLetter2;
   
    public GameObject infoPanel;
    public GameObject resultPanel;
    public GameObject imgLetter;
    public GameObject left, right, timer, stopBtn, backBtn, plName;
    public GameObject mainPanel;
    public string[] values = new string[9];
    Dictionary<int, string[]> userData = new Dictionary<int, string[]>();
    Dictionary<int, int> userClicked = new Dictionary<int, int>();
    public Dictionary<int, string[]> userScores = new Dictionary<int, string[]>();
    ArrayList playerData = new ArrayList(9);
    Dictionary<int, Text> inputField2;
    //public InputField[] inputFieldText = new InputField[9];
    //public Text[] infoText = new Text[9];
    //public Text[] name;
    // Start is called before the first frame update
    PlayerManager playerManager;
    GameObject[] playerItems;
    [HideInInspector]
    public Player localPlayer;
    Text text;
    public int playerId;
    public GameObject[] playerCtrls;
    public Text[] infoTexts;
    public Text[] showPlayerText;
    ExitGames.Client.Photon.Hashtable custom_Properties = new ExitGames.Client.Photon.Hashtable();


    [SerializeField]
    InputField nameInputField;

    [SerializeField]
    InputField lastNameInputField;

    [SerializeField]
    InputField cityInputField;

    [SerializeField]
    InputField countryInputField;

    [SerializeField]
    InputField colorInputField;

    [SerializeField]
    InputField animalInputField;

    [SerializeField]
    InputField foodInputField;

    [SerializeField]
    InputField fruitInputField;

    [SerializeField]
    InputField thingsInputField;

    [SerializeField]
    TMPro.TMP_InputField allPlayersList;

    [SerializeField]
    TMPro.TMP_InputField allScores;

    // public GameObject imagesPanel;
    public Toggle[] check0 = new Toggle[9];
    public Toggle[] check5 = new Toggle[9];
    public Toggle[] check10 = new Toggle[9];
    int total = 0;
    bool isCountedScore;
    public GameObject waitingPanel;
    bool isShow;
    bool isOpponentLeft;
    public Text opponentLeftText;
    void Start()
    {//this runs before the system is connected to photon so it's always 0
        //isOpponentLeft = true;
        playerCtrls = new GameObject[PhotonNetwork.PlayerList.Length];
        Invoke("ActivateGameObjects", 12f);
        isCountedScore = true;
    }
    void ActivateGameObjects()
    {
        left.SetActive(true);
        right.SetActive(true);
        timer.SetActive(true);
        mainPanel.SetActive(true);
        stopBtn.SetActive(true);
      
    }

    private void Update()
    {
        //if (PhotonNetwork.PlayerList.Length == 1 && isOpponentLeft)
        //{
        //    string[] y = {PhotonNetwork.LocalPlayer.ActorNumber.ToString(),PhotonNetwork.LocalPlayer.NickName,"80" };
        //    userScores.Add(PhotonNetwork.LocalPlayer.ActorNumber - 1, y);
        //    isOpponentLeft = false;
        //    opponentLeftText.gameObject.SetActive(true);
        //}

        //playerName.text = "نام بازیکن : " + PhotonNetwork.LocalPlayer.NickName.faConvert();
    }

    public void StopBtnClicked()
    {

        this.gameObject.GetComponent<PhotonView>().RPC("StopBtnBuffered", RpcTarget.All);

    }

    [PunRPC]
    public void StopBtnBuffered()
    {
        mainPanel.SetActive(false);
        infoPanel.SetActive(true);
        left.SetActive(false);
        right.SetActive(false);
        timer.SetActive(false);
        imgLetter.SetActive(false);
        stopBtn.SetActive(false);
        plName.SetActive(true);
        backBtn.SetActive(true);
        Invoke("ActivateCheckBtn", 25f);
        //if (!custom_Properties.ContainsKey("name"))
        //{

        //}
        custom_Properties.Add("name", nameInputField.text);
        custom_Properties.Add("lastname", lastNameInputField.text);
        custom_Properties.Add("city", cityInputField.text);
        custom_Properties.Add("country", countryInputField.text);
        custom_Properties.Add("color", colorInputField.text);
        custom_Properties.Add("animal", animalInputField.text);
        custom_Properties.Add("food", foodInputField.text);
        custom_Properties.Add("fruit", fruitInputField.text);
        custom_Properties.Add("things", thingsInputField.text);

        localPlayer.SetCustomProperties(custom_Properties);

        mainPanel.SetActive(false);
        infoPanel.SetActive(true);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (isCountedScore)
        {
            allPlayersList.text += (targetPlayer.ActorNumber + ": " + changedProps["name"] + " : " + changedProps["lastname"]) + " : " + changedProps["city"] + " : " + changedProps["country"] + " : " + changedProps["color"] + " : " + changedProps["animal"] + " : " + changedProps["food"] + " : " + changedProps["fruit"] + " : " + changedProps["things"] + " : " + targetPlayer.NickName + " : " + changedProps["Score"] + ";";



            string source = allPlayersList.text;
            string[] op = new string[] { ";" };
            string[] x = source.Split(op, StringSplitOptions.None);

            string[][] finalOutput = { };


            string[] y = { };
            for (int i = 0; i < x.Length - 1; i++)
            {
                string[] colonOp = new string[] { ":" };

                y = x[i].Split(colonOp, StringSplitOptions.None);



            }
            userData.Add(targetPlayer.ActorNumber - 1, y);

            Invoke("T", 2f);
        }

        else
        {


            allScores.text += (targetPlayer.ActorNumber + " : " + changedProps["pName"] + " : " + changedProps["Score"] + ";");
            string source = allScores.text;
            string[] op = new string[] { ";" };
            string[] x = source.Split(op, StringSplitOptions.None);

            string[][] finalOutput = { };


            string[] y = { };
            for (int i = 0; i < x.Length - 1; i++)
            {
                string[] colonOp = new string[] { ":" };

                y = x[i].Split(colonOp, StringSplitOptions.None);



            }


            userScores.Add(targetPlayer.ActorNumber - 1, y);


            //if (userScores.Count >= PhotonNetwork.PlayerList.Length)
            //    this.gameObject.GetComponent<PhotonView>().RPC("ActiveResPanel", RpcTarget.All, targetPlayer.ActorNumber - 1);



        }


    }
    [PunRPC]
    void ActiveResPanel(int actNumber)
    {
        //StartCoroutine(ActivateReaultPanel(actNumber));
    }
   void ActivateWaitingPanel()
    {
        mainPanel.SetActive(false);
        infoPanel.SetActive(false);
        waitingPanel.SetActive(true);
    }

    void ActivateReaultPanel()
    {
    //    yield return new WaitForSeconds(2f);
        mainPanel.SetActive(false);
        infoPanel.SetActive(false);
        resultPanel.SetActive(true);
    

    }
    void T()
    {
        imgLetter.transform.position = transform.position+Vector3.up*10;
        voteText.text = "را امتیاز دهید! ".faConvert() + playerName.text;

        isCountedScore = false;
        for (int i = 0; i < userData.Count; i++)
        {

            if (PhotonNetwork.LocalPlayer.ActorNumber.ToString() == userData[i][0].ToString())
            {
                if (i == userData.Count - 1)
                {
                    playerName.text = userData[0][10].faConvert().ToString();
                    infoTexts[0].text = userData[0][1].ToString();
                    showPlayerText[0].text = userData[0][1].faConvert().ToString();
                    infoTexts[1].text = userData[0][2].ToString();
                    showPlayerText[1].text = userData[0][2].faConvert().ToString();
                    infoTexts[2].text = userData[0][3].ToString();
                    showPlayerText[2].text = userData[0][3].faConvert().ToString();
                    infoTexts[3].text = userData[0][4].ToString();
                    showPlayerText[3].text = userData[0][4].faConvert().ToString();
                    infoTexts[4].text = userData[0][5].ToString();
                    showPlayerText[4].text = userData[0][5].faConvert().ToString();
                    infoTexts[5].text = userData[0][6].ToString();
                    showPlayerText[5].text = userData[0][6].faConvert().ToString();
                    infoTexts[6].text = userData[0][7].ToString();
                    showPlayerText[6].text = userData[0][7].faConvert().ToString();
                    infoTexts[7].text = userData[0][8].ToString();
                    showPlayerText[7].text = userData[0][8].faConvert().ToString();
                    infoTexts[8].text = userData[0][9].ToString();
                    showPlayerText[8].text = userData[0][9].faConvert().ToString();
                    break;
                }
                playerName.text = userData[i + 1][10].faConvert().ToString();
                infoTexts[0].text = userData[i + 1][1].ToString();
                showPlayerText[0].text = userData[i + 1][1].faConvert().ToString();
                infoTexts[1].text = userData[i + 1][2].ToString();
                showPlayerText[1].text = userData[i + 1][2].faConvert().ToString();
                infoTexts[2].text = userData[i + 1][3].ToString();
                showPlayerText[2].text = userData[i + 1][3].faConvert().ToString();
                infoTexts[3].text = userData[i + 1][4].ToString();
                showPlayerText[3].text = userData[i + 1][4].faConvert().ToString();
                infoTexts[4].text = userData[i + 1][5].ToString();
                showPlayerText[4].text = userData[i + 1][5].faConvert().ToString();
                infoTexts[5].text = userData[i + 1][6].ToString();
                showPlayerText[5].text = userData[i + 1][6].faConvert().ToString();
                infoTexts[6].text = userData[i + 1][7].ToString();
                showPlayerText[6].text = userData[i + 1][7].faConvert().ToString();
                infoTexts[7].text = userData[i + 1][8].ToString();
                showPlayerText[7].text = userData[i + 1][8].faConvert().ToString();
                infoTexts[8].text = userData[i + 1][9].ToString();
                showPlayerText[8].text = userData[i + 1][9].faConvert().ToString();
            }


        }
        Invoke("CalculateZeroScore", 1);
        Invoke("CalculateFiveScore", 1.1f);
        Invoke("CalculateTenScore", 1.11f);
        //  Invoke("TotalScore", 5);
    }

    void CalculateZeroScore()
    {

        for (int i = 0; i < userData.Count; i++)
        {
            for (int j = 0; j < infoTexts.Length; j++)
            {
                //print("Empty not"+ userData[i][j]);
                if (string.IsNullOrEmpty(infoTexts[j].text) || string.IsNullOrWhiteSpace(infoTexts[j].text) || string.Equals(infoTexts[j].text, generatedLetter.text) || infoTexts[j].text.Trim().Length <= 1)
                {
                    check0[j].isOn = true;
                    check0[j].enabled = false;
                    check5[j].enabled = false;
                    check10[j].enabled = false;
                    check10[j].interactable = false;
                    check5[j].interactable = false;
                }
            }
        }
    }
    void CalculateTenScore()
    {
        for (int i = 0; i < userData.Count; i++)
        {
            for (int j = 0; j < infoTexts.Length; j++)
            {
                //print("Empty not"+ userData[i][j]);
                if (!check10[j].isOn && !check5[j].isOn && !check0[j].isOn)
                { 
                    check5[j].enabled = false;
                
                    check5[j].interactable = false;
                }
            }
        }
    }
    void CalculateFiveScore()
    {
        int count = 0;
        for (int j = 0; j < infoTexts.Length; j++)
        {

            string fL = infoTexts[j].text.Trim();

            if (!string.IsNullOrWhiteSpace(infoTexts[j].text))
            {

                if ((fL[0].ToString() == generatedLetter.text.ToString() || fL[0].ToString() == generatedLetter2.text.ToString()))
                {



                    for (int i = 0; i < userData.Count; i++)
                    {

                        string str1 = userData[i][j + 1].Trim();
                        string str2 = infoTexts[j].text.Trim();


                        if (string.Equals(str1, str2) && !check0[j].isOn)
                        {
                            print("Equal" + count);
                            if (count == 0)
                            {
                                count++;
                            }
                            else if (count != 0)
                            {
                                check5[j].isOn = true;
                                check5[j].enabled = false;
                                check0[j].enabled = false;
                                check10[j].enabled = false;
                                check10[j].interactable = false;
                                check0[j].interactable = false;
                            }

                        }

                    }
                    count = 0;
                }
                else
                {
                    check0[j].isOn = true;
                    check5[j].enabled = false;
                    check0[j].enabled = false;
                    check10[j].enabled = false;
                    
                }
            }

        }


    }

    
    
    public void TotalScore()
    {
        this.gameObject.GetComponent<PhotonView>().RPC("CalculateTotalScore", RpcTarget.All);
    }
    
    public void CalculateTotalScore()
    {
        total = 0;

        for (int i = 0; i < check5.Length; i++)
        {
            if (check5[i].isOn)
            {
                total += 5;
                
            }

        }
        for (int i = 0; i < check10.Length; i++)
        {
            if (check10[i].isOn)
            {
                 
                total += 10;
            }

        }
      
        playfabManager.SendLeaderboard(total);
        ActivateReaultPanel();
        //ActivateWaitingPanel();
        custom_Properties.Add("Score", total);
        custom_Properties.Add("pName", playerName.text);
        localPlayer.SetCustomProperties(custom_Properties);
        showPlayerText[8].text = total.ToString();
        // infoTexts[8].text = total.ToString();
        // return total;
    }
   

    void ActivateCheckBtn()
    {
        checkBTN.interactable = true;
    }
}