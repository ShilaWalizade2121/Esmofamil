using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextAppear : MonoBehaviour
{
    Text text;
    public float delay;
    public float endSize;
     
    // Start is called before the first frame update
    void Start()
    {
       text =  gameObject.GetComponent<Text>();
        text.fontSize = 0;
        text.color = Color.white;

    }

    // Update is called once per frame
    void Update()
    {
        Invoke("IncreaseFontSize", delay);
    }

    void IncreaseFontSize()
    {

        if(text.fontSize >=0 && text.fontSize <= endSize)
        {
            text.color = Color.black;
            text.fontSize +=2;
        }
    }
}
