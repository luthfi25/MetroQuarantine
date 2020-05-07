using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomScript : MonoBehaviour
{
    private int broomCounter;
    public MiniGameController miniGameControllerInstance;
    public GameObject BroomButton;
    bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        isRight = true;
        broomCounter = 0;
        BroomButton.SetActive(true);
    }

    void OnDisable(){
        BroomButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(broomCounter >= 20){
            miniGameControllerInstance.CloseMiniGame(this.gameObject);
        }
    }

    public void MoveButton(GameObject btn){
        if(isRight){
            btn.gameObject.transform.Translate(-5f, 0f, 0f);
            isRight = false;
        } else {
            btn.gameObject.transform.Translate(5f, 0f, 0f);
            isRight = true;
        }

        broomCounter++;
        miniGameControllerInstance.AddProgressTrack(broomCounter, 20);
    }

    public void RestartBroom(){
        broomCounter = 0;
    }
}
