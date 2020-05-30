using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    Animator animator;
    SpriteRenderer srr;
    Sprite sr;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        srr = GetComponent<SpriteRenderer>();
        sr = srr.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doAnimateOpen()
    {
        //animator.SetBool("open", true);
        srr.sprite = null;
    }

    public void doAnimateClose()
    {
        // animator.SetBool("open", false);
        srr.sprite = sr;
    }

    void OnTriggerEnter2D(Collider2D coll){
        if(coll.gameObject.CompareTag("Player") || coll.gameObject.CompareTag("Enemy")){
            doAnimateOpen();
        }
    }

    void OnTriggerExit2D(Collider2D coll){
        if(coll.gameObject.CompareTag("Player") || coll.gameObject.CompareTag("Enemy")){
            doAnimateClose();
        }
    }
}
