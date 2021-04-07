using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DG.Tweening;

using UnityEngine.UI;

public class ImageAppear : MonoBehaviour
{

    public float delay;
    Image image;
    
    
    // Start is called before the first frame update
    void Start()
    {
       
        image = GetComponent<Image>() ;
        image.fillAmount = 0;
       
    }

    // Update is called once per frame
    void Update()
    {

        Invoke("GameobjectAppear", delay);

        
    }
    public void GameobjectAppear()
    {
        if (image.fillAmount >= 0 && image.fillAmount <= 1)
        {
            
             image.fillAmount += 0.01f;
        }
        
    }

   

  

}
