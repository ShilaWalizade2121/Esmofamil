using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class RandomCharacter : MonoBehaviourPun
{

    public Text txt;
    public Text anotherText;
    public float time;
    string output;
    float timer, timer2;
    bool isContiniue;
    // PhotonView photonView;
      public Image[] images;
    public GameObject imagesPanel

        ;
    List<char> keysMap = new List<char>();
    static Dictionary<char, char> map = new Dictionary<char, char>()
{


        {'آ', 'آ'},
        {'ا', 'ﺍ'},
        {'ب','ﺑ' },
        {'پ', 'ﭘ'},
        {'ت','ﺗ'},

        {'ج',   'ﺟ'},
        {'چ',   'ﭼ'},
        {'ح', 'ﺣ'},
        {'خ',  'ﺧ'},
        {'د',   'ﺩ'},
        {'ر',   'ﺭ'},
        {'ز',   'ﺯ'},
        {'س',   'ﺳ'},
        {'ش', 'ﺷ'},
        {'ص',  'ﺻ'},
        {'ض',   'ﺿ'},
        {'ط',   'ﻃ'},
        {'ظ',  'ﻇ'},

};
    private void Awake()
    {
        StartCoroutine(ActivateImageLetter());
        foreach (KeyValuePair<char, char> item in map)
        {
            keysMap.Add(item.Key);
            // Access the key with item.Key
            // Access the value with item.Value
        }
    }

    IEnumerator ActivateImageLetter()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = true;
            yield return new WaitForSeconds(0.5f);
            images[i].enabled = false;
        }
        Invoke("StartMethod", 2f);
    }

    void Start()
    {
     //   Invoke("StartMethod", 10f);
    }

    void StartMethod()
    {

        //txt = gameObject.GetComponent<Text>();
        if (PhotonNetwork.IsMasterClient)
        {


            output = keysMap[(int)(Random.Range(0, keysMap.Count))].ToString();
            this.photonView.RPC("CharGenerator", RpcTarget.All, output);

        }


    }


    [PunRPC]
    public void CharGenerator(string output2)
    {


        if (output2 != null)
        {
            txt.text = output2.ToString();
            char[] x = output2.ToCharArray();
            anotherText.text = map[x[0]].ToString();
        }
      

       // GameObject letter = GameObject.Find(txt.text);
         GameObject letter2 = GameObject.Find(anotherText.text);
        //if (letter != null ) {
        //    letter.GetComponent<Image>().enabled = true;

        //}
         if(letter2 != null)
        {

            letter2.GetComponent<Image>().enabled = true;


        }


        //this.photonView.RPC("DeactivatePanel", RpcTarget.All);
        imagesPanel.SetActive(false);


    }
   
    void DeactivatePanel()
    {
        //yield return new WaitForSeconds(2f);
        imagesPanel.SetActive(false);
    }





}
