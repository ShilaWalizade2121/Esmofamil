using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void LoadAndExit(string scene)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(scene);
    }
    public void ExitFromGame()
    {
        Application.Quit();
    }
  
}
