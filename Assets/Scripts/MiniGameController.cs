using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    public GameObject CameraMiniGame;
    public GameObject CameraMain;
    public MovementScript movementScriptInstance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseMiniGame(){
        movementScriptInstance.Unpause();
        CameraMiniGame.SetActive(false);
        CameraMain.SetActive(true);
    }
}
