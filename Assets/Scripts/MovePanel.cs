using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MovePanel : MonoBehaviour
{

    RectTransform trans;
    bool turnLeft;
    bool turnRight;
    // Start is called before the first frame update
    void Start()
    {
        turnLeft = true;
        turnRight = true;

        trans = GetComponent<RectTransform>();
      

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveRight(float offset)
    {

        if (turnRight)
        {
            turnRight = false;
            if (transform.localPosition.x < -0.5f)
            {
                trans.DOAnchorPosX(transform.localPosition.x + offset, 0.5f, false);
                Invoke("ChangeTurn", 0.5f);
            }
                
        }

    }
    public void MoveLeft(float offset)
    {
        if (turnLeft)
        {
            turnLeft = false;
            if (transform.localPosition.x > -11559.5f)
            {
                trans.DOAnchorPosX(transform.localPosition.x - offset, 0.5f, false);
                Invoke("ChangeTurn", 0.5f);
            }

        }

    }
    void ChangeTurn()
    {
        turnLeft = true;
        turnRight = true;
    }
    
}
