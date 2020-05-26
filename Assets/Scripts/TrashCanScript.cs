using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCanScript : EventTrigger
{
    private Animator animator;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnPointerEnter(PointerEventData eventData) {
        if(animator.enabled){
            animator.SetTrigger("Expand");
        } else {
            animator.enabled = true;
        }
    }

    public override void OnPointerExit(PointerEventData eventData) {
        if(animator.enabled){
            animator.SetTrigger("Shrink");
        }
    }
}
