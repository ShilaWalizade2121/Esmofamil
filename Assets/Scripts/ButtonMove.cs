using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonMove : MonoBehaviour
{
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosX(227f, delay, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
