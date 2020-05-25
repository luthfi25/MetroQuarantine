using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PuzzlePieceScript : EventTrigger
{
    private Vector2 initialPosition;
    private bool dragging;
    private Rect screenRect;
    private bool locked;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        screenRect = new Rect(0,0, Screen.width, Screen.height);
        locked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!locked && dragging && screenRect.Contains(Input.mousePosition)) {
            Vector2 normPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(normPosition.x, normPosition.y);
        }
    }

    public override void OnPointerDown(PointerEventData eventData){
        transform.SetAsLastSibling();
        dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData){
        dragging = false;

        int id = int.Parse(GetComponentInChildren<TextMeshProUGUI>().text);
        bool validPos = GameObject.Find("Puzzle").GetComponent<PuzzleScript>().ValidPosition(id, transform.position);
        
        if(validPos){
            locked = true;
        } else {
            transform.position = initialPosition;
        }
    }
}
