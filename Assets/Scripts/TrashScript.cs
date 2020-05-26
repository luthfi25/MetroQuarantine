using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashScript : EventTrigger
{
    private Vector3[] thrashCan;
    private Animator thrashCanAnimator;
    private bool dragging;
    private Rect screenRect;
    private GarbageScript garbageScriptInstance;

    // Start is called before the first frame update
    void Start()
    {
        thrashCan = new Vector3[4];
        //bot left, top left, top right, bot right
        GameObject.FindGameObjectWithTag("Trash Can").GetComponent<RectTransform>().GetWorldCorners(thrashCan);
        thrashCanAnimator = GameObject.FindGameObjectWithTag("Trash Can").GetComponent<Animator>();
        
        dragging = false;   
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        garbageScriptInstance = GameObject.Find("Garbage(Clone)").GetComponent<GarbageScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dragging && screenRect.Contains(Input.mousePosition)) {
            Vector2 normPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(normPosition.x, normPosition.y);
            isInsideThrashCan("down");
        }
    }

    public override void OnPointerDown(PointerEventData eventData){
        transform.SetAsLastSibling();
        dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData) {
        dragging = false;
        isInsideThrashCan("up");
    }

    void isInsideThrashCan(string mode){
        if(transform.position.x <= thrashCan[2].x &&
        transform.position.x >= thrashCan[0].x &&
        transform.position.y <= thrashCan[1].y &&
        transform.position.y >= thrashCan[3].y){
            if(mode == "down"){
                if(thrashCanAnimator.enabled && !thrashCanAnimator.GetBool("Expand")){
                    thrashCanAnimator.SetBool("Expand", true);
                    thrashCanAnimator.SetBool("Shrink", false);
                } else {
                    thrashCanAnimator.enabled = true;
                }
            } else if(mode == "up"){
                if(thrashCanAnimator.enabled && !thrashCanAnimator.GetBool("Shrink")){
                    thrashCanAnimator.SetBool("Shrink", true);
                    thrashCanAnimator.SetBool("Expand", false);
                }
                garbageScriptInstance.ThrashThrown();
                Destroy(this.gameObject);
            }
        } else {
            if(thrashCanAnimator.enabled && !thrashCanAnimator.GetBool("Shrink")){
                thrashCanAnimator.SetBool("Shrink", true);
                thrashCanAnimator.SetBool("Expand", false);
            }
        }
    }
}
