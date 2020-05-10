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
    public bool CoolingDown;
    public bool CooldownMistake;
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
    public GameObject MistakeBig;
    bool CoolingDownReverse;


    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable(){
        uIScriptInstance.gameObject.SetActive(false);
        CoolingDown = false;
        targetFill = 1f;
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

        if (CoolingDown)
        {
            ProgressBar.fillAmount += 0.25f * Time.deltaTime;
            if(ProgressBar.fillAmount >= targetFill){
                ProgressBar.fillAmount = targetFill;
                CoolingDown = false;
                disableSuccessText();
            }
        }

        if(CooldownMistake){
            if(targetFill < ProgressBar.fillAmount){
                ProgressBar.fillAmount -= 0.5f * Time.deltaTime;
                if(ProgressBar.fillAmount <= targetFill){
                    CooldownMistake = false;
                }
            }

            if(!MistakeBig.activeInHierarchy){
                MistakeBig.SetActive(true);
            }
        } else {
            if(MistakeBig.activeInHierarchy){
                MistakeBig.SetActive(false);
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
        CoolingDown = true;

        SuccessBig.SetActive(true);
    }

    public void RestartProgressTrack(){
        targetFill = 0f;
        CooldownByMistake();
    }

    void disableSuccessText(){
        SuccessBig.SetActive(false);
    }

    public void CooldownByMistake(){
        
        CooldownMistake = true;
        StartCoroutine(doCooldownByMistake());
    }

    IEnumerator doCooldownByMistake(){
        yield return new WaitForSeconds(0.5f);
        if(targetFill >= ProgressBar.fillAmount){
            CooldownMistake = false;
        }
    }
}
