using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureHolderScript : MonoBehaviour
{
    private bool MovingDown;
    private float targetY;
    private bool MovingUp;
    private RectTransform rectTransform;
    public GestureDetectorScript gestureDetectorScript;
    public GameObject Gesture;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable(){
        MovingDown = false;
        MovingUp = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(MovingDown){
            rectTransform.localPosition += new Vector3(0f, -0.25f, 0f) * 750f * Time.deltaTime;
            if(rectTransform.localPosition.y <= targetY){
                MovingDown = false;
                Gesture.SetActive(true);
                gestureDetectorScript.ActivateGesture();
            }
        } else if (MovingUp){
            rectTransform.localPosition += new Vector3(0f, 0.25f, 0f) * 750f * Time.deltaTime;
            if(rectTransform.localPosition.y >= targetY){
                MovingUp = false;
            }
        }
    }

    public void MoveDown(){
        MovingDown = true;
        targetY = 200f;
    }

    public void MoveUp(){
        MovingUp = true;
        targetY = 375f;
    }
}
