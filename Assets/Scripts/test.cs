using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;

public class test : MonoBehaviourPun
{
     Text text;
    private ExitGames.Client.Photon.Hashtable custom_Properties = new ExitGames.Client.Photon.Hashtable();
    GameObject[] playerItems;
    private void Start()
    {
    }
    private void Update()
    {
        //never use findgameobjects in the Update, it's VERY costly
        //playerItems = GameObject.FindGameObjectsWithTag("Player");
            
    }
    public int index=0;
    public void SetPlayerInputField(string inputField)
    {
        
        if (string.IsNullOrEmpty(inputField))
        {

            print("Error! enter the name");
            return;
        }
     
    }
   
}
