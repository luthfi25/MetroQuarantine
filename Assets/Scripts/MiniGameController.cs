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
    public GameObject ProgressTrack;
    private Image ProgressBar;
    private bool coolingDown;
    private float targetFill;

    private float height;
    private Vector3 destination;
    private Vector3 originDestination;

    public UIScript uIScriptInstance;
    private bool closing;
    private GameObject toClose;
    private string miniGameName;

    public GoalScript goalScriptInstance;
    public GameObject SuccessBig;


    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable(){
        uIScriptInstance.gameObject.SetActive(false);
        coolingDown = false;
        targetFill = 0;
        closing = false;
        miniGameName = "";
        SuccessBig.SetActive(false);
    }

    void OnDisable(){
        uIScriptInstance.gameObject.SetActive(true);
        StopWatch.SetActive(false);
        ProgressTrack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   
        if(ProgressBar == null){
            ProgressBar = GameObject.Find("Progress Track").GetComponent<Image>();
        }

        if(closing){
            if(ProgressBar.fillAmount >= 1.0f){
                doCloseMiniGame();
            }
        }

        if (coolingDown)
        {
            ProgressBar.fillAmount += 0.25f * Time.deltaTime;
            if(ProgressBar.fillAmount >= targetFill){
                coolingDown = false;
                disableSuccessText();
            }
        }
    }

    public void CloseMiniGame(GameObject go, string name){
        toClose = go;
        closing = true;
        miniGameName = name;
    }

    void doCloseMiniGame(){
        GameObject go = GameObject.Find(miniGameName);
        goalScriptInstance.DestroyGoal(go);

        toClose.SetActive(false);
        RestartProgressTrack();
        movementScriptInstance.Unpause();
        CameraMiniGame.SetActive(false);
        CameraMain.SetActive(true);
    }

    public void AddProgressTrack(int step, int bound){
        targetFill = (float) step / bound;
        coolingDown = true;

        SuccessBig.SetActive(true);
    }

    public void RestartProgressTrack(){
        ProgressBar.fillAmount = 0f;
    }

    void disableSuccessText(){
        SuccessBig.SetActive(false);
    }
}
