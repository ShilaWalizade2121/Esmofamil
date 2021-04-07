using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ButtonManager : MonoBehaviour
{
    public RectTransform music, sound;
    bool isOpen;
    public float ofset;
    public GameObject startPanel;
    public GameObject gameGuidePanel;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowGameGuidePanel()
    {
        if (gameGuidePanel.activeInHierarchy)
        {
            gameGuidePanel.SetActive(false);
            startPanel.SetActive(true);
            
        }
        else {
            gameGuidePanel.SetActive(true);
            startPanel.SetActive(false);
        }
    }

    public void ShowStartPanel()
    {
        if (startPanel.activeInHierarchy)
        {
            startPanel.SetActive(false);
            gameGuidePanel.SetActive(true);
        }
        else
        {
            startPanel.SetActive(true);
            gameGuidePanel.SetActive(false);
        }
    }

    public void move()
    {


        if (!isOpen)
        {
            isOpen = true;
            music.DOAnchorPosY(247 , (0.5f), false);
            sound.DOAnchorPosY(247+ofset, (0.5f), false);
        }
        else
        {
            isOpen = false;
            music.DOAnchorPosY(137.9999f, 0.5f, false);
            sound.DOAnchorPosY(137.9999f, 0.5f, false);
        }
        print("hiiii");
       
        
    }

}
