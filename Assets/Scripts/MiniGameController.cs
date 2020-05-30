using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour
{
    public GameObject CameraMiniGame;
    public GameObject CameraMain;
    [SerializeField] private HouseGameManagerScript houseGameManagerScript;
    public GameObject StopWatch;
    public GameObject ProgressTrack;
    private Image ProgressBar;
    public bool CoolingDown;
    public bool CooldownMistake;
    private float targetFill;

    private bool closing;
    private GameObject toClose;
    private string miniGameName;

    public GameObject SuccessBig;
    public GameObject MistakeBig;
    public GameObject SucessSmall;

    public GameObject[] MiniGames;
    public GameObject[] Tutorials;
    public Transform MiniGameCanvas;

    AudioSource[] audioSources;
    public MiniGameStopWatchScript miniGameStopWatchScriptInstance;
    public Canvas MainCanvas;


    // Start is called before the first frame update
    void Start()
    {  
        audioSources = GetComponents<AudioSource>();

        GameObject gameManager = GameObject.Find("_GAME MANAGER");
        if(!gameManager.TryGetComponent<HouseGameManagerScript>(out houseGameManagerScript)){
            Debug.Log("Can't find IGameManager");
        }
    }

    void OnEnable(){
        CoolingDown = false;
        targetFill = 1f;
        closing = false;
        miniGameName = "";
        SuccessBig.SetActive(false);
        SucessSmall.SetActive(false);
        miniGameStopWatchScriptInstance.SetFreezeTime(false);
        MainCanvas.gameObject.SetActive(false);
    }

    void OnDisable(){
        StopWatch.SetActive(false);
        ProgressTrack.SetActive(false);
        MistakeBig.SetActive(false);
        CoolingDown = false;
        CooldownMistake = false;
        MainCanvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {      

        if(closing){
            if(ProgressBar.fillAmount >= 1.0f){
                doCloseMiniGame();
                return;
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
        StopSound();
        miniGameName = name;
    }

    public void CloseMiniGameDelay(GameObject go, string name, float delayTime){
        miniGameStopWatchScriptInstance.SetFreezeTime(true);
        StartCoroutine(closingDelay(go, name, delayTime));
    }

    void doCloseMiniGame(){
        // GameObject go = GameObject.Find(miniGameName);
        // goalScriptInstance.DestroyGoal(go);

        // toClose.SetActive(false);
        Destroy(toClose);
        RestartProgressTrack();
        CameraMiniGame.SetActive(false);
        CameraMain.SetActive(true);
        houseGameManagerScript.GoalFinished();
    }

    public void AddProgressTrack(int step, int bound, bool showSuccessText){
        targetFill = (float) step / bound;
        CoolingDown = true;

        if(showSuccessText){
            SuccessBig.SetActive(true);
        }
    }

    public void AddProgressTrackSmall(int step, int bound, bool showSuccessText){
        targetFill = (float) step / bound;
        CoolingDown = true;

        if(showSuccessText){
            SucessSmall.SetActive(true);
        }
    }

    public void RestartProgressTrack(){
        targetFill = 0f;
        CooldownByMistake();
    }

    void disableSuccessText(){
        SuccessBig.SetActive(false);
        SucessSmall.SetActive(false);
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

    public void InstantiateMiniGame(string name){
        GameObject go;
        switch(name){
            case "Book":
                go = Instantiate(MiniGames[0]);
                go.transform.SetParent(MiniGameCanvas, false);
                miniGameStopWatchScriptInstance.AddMiniGame(go);
                break;
            case "Soap":
                go = Instantiate(MiniGames[1]);
                go.transform.SetParent(MiniGameCanvas, false);
                miniGameStopWatchScriptInstance.AddMiniGame(go);
                break;
            case "Food":
                go = Instantiate(MiniGames[2]);
                go.transform.SetParent(MiniGameCanvas, false);
                miniGameStopWatchScriptInstance.AddMiniGame(go);
                break;
            case "Spray":
                go = Instantiate(MiniGames[3]);
                go.transform.SetParent(this.gameObject.transform);
                miniGameStopWatchScriptInstance.AddMiniGame(go);
                break;
            case "Broom":
                go = Instantiate(MiniGames[4]);
                go.transform.SetParent(MiniGameCanvas, false);
                miniGameStopWatchScriptInstance.AddMiniGame(go);
                break;
            default:
                break;
        }

        ProgressBar = GameObject.Find("Progress Track").GetComponent<Image>();
        ProgressBar.fillAmount = 0f;
    }

    public void ActivateTutorial(string name){
        switch(name){
            case "Buku":
                Tutorials[0].SetActive(true);
                break;
            case "Sabun":
                Tutorials[1].SetActive(true);
                break;
            case "Makan":
                Tutorials[2].SetActive(true);
                break;
            case "Semprot":
                Tutorials[3].SetActive(true);
                break;
            case "Sapu":
                Tutorials[4].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ToggleGUI(bool active){
        GameObject soap = GameObject.Find("Soap(Clone)");
        GameObject book = GameObject.Find("Book(Clone)");

        if(soap != null){
            if(active){
                soap.GetComponent<GestureDetectorScript>().EnableGUI();
            } else {
                soap.GetComponent<GestureDetectorScript>().DisableGUI();
            }  
        } else if (book != null){
            if(active){
                book.GetComponent<GestureDetectorScript>().EnableGUI();
            } else {
                book.GetComponent<GestureDetectorScript>().DisableGUI();
            }  
        }
    }

    public void PlaySound(AudioClip clip, bool loop){
        if(loop){
            audioSources[0].clip = clip;
            float clipTime = Random.Range(0f, 1f) * clip.length;
            audioSources[0].time = clipTime;
            audioSources[0].Play();
        } else {
            audioSources[1].PlayOneShot(clip);
        }
    }

    public void StopSound(){
        audioSources[0].clip = null;
        audioSources[0].Stop();
        audioSources[1].Stop();
    }

    IEnumerator closingDelay(GameObject go, string name, float delayTime){
        yield return new WaitForSeconds(delayTime);
        CloseMiniGame(go, name);
    }
}
