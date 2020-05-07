using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour
{
    public GameObject CameraMiniGame;
    public GameObject CameraMain;
    public MovementScript movementScriptInstance;
    public GameObject StopWatch;
    public Image ProgressTrack;
    private bool coolingDown;
    private float targetFill;

    private float height;
    private Vector3 destination;
    private Vector3 originDestination;

    public UIScript uIScriptInstance;
    private bool closing;
    private GameObject toClose;


    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable(){
        uIScriptInstance.DisablePause();
        StopWatch.SetActive(true);
        ProgressTrack.gameObject.SetActive(true);
        coolingDown = false;
        targetFill = 0;
        closing = false;
    }

    void OnDisable(){
        uIScriptInstance.EnablePause();
        StopWatch.SetActive(false);
        ProgressTrack.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(closing){
            if(ProgressTrack.fillAmount >= 1.0f){
                doCloseMiniGame();
            }
        }

        if (coolingDown)
        {
            ProgressTrack.fillAmount += 0.25f * Time.deltaTime;
            if(ProgressTrack.fillAmount >= targetFill){
                coolingDown = false;
            }
        }
    }

    public void CloseMiniGame(GameObject go){
        toClose = go;
        closing = true;
    }

    void doCloseMiniGame(){
        toClose.SetActive(false);
        RestartProgressTrack();
        movementScriptInstance.Unpause();
        CameraMiniGame.SetActive(false);
        CameraMain.SetActive(true);
    }

    public void AddProgressTrack(int step, int bound){
        targetFill = (float) step / bound;
        coolingDown = true;
    }

    public void RestartProgressTrack(){
        ProgressTrack.fillAmount = 0f;
    }
}
