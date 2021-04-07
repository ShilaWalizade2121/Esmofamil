using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameCtrl : MonoBehaviour
{
    public static GameCtrl Instance;

    public bool startGame;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        startGame = true;
    }

    public void ExitGameLobby()
    {
        NetworkManager.Instance.ActivePanel(NetworkManager.Instance.GameOptionsPanel.name);
    }
}
