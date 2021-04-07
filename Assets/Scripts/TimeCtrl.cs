using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeCtrl : MonoBehaviour
{
    public float maxTime;
    float leftTime;
    public Text timer;
    public MainTableCtrl mainTableCtrl;
    // Start is called before the first frame update
    void Start()
    {
        leftTime = maxTime;
       
    }

    // Update is called once per frame
    void Update()
    {

        if (leftTime >= 0)
        {
            updateTimer();
        }
    }
    void updateTimer()
    {
        //timer.fontSize = currentSize;
        leftTime -= Time.deltaTime;
        if (leftTime <= 11)
        {
            timer.color = Color.red;
            
        }
        timer.text =  ((int)leftTime).ToString();

        if (leftTime < 0)
        {
            mainTableCtrl.StopBtnClicked();
        }

       
    }
}
