using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameStopWatchScript : MonoBehaviour
{
    public TextMeshProUGUI SprayStopWatchText;
    private float curTime;
    public GameObject[] miniGames;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        curTime = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        decreaseTime();
    }

    void decreaseTime(){
        if (curTime <= 0f){
            restartTime();

            foreach(GameObject mg in miniGames){
                if (!mg.activeSelf){
                    continue;
                }

                GestureDetectorScript gds = mg.GetComponent<GestureDetectorScript>();
                if (gds != null){
                    gds.RestartGesture();
                }

                SprayScript ss = mg.GetComponent<SprayScript>();
                if(ss != null){
                    ss.RestartSpray();
                }

                MiniGameController mgc = mg.GetComponent<MiniGameController>();
                if (mgc != null){
                    mgc.RestartProgressTrack();
                }

                FoodScript fs = mg.GetComponent<FoodScript>();
                if (fs != null) {
                    fs.RestartFood();
                }
            }
        }

        curTime -= Time.deltaTime;
        float seconds = curTime % 60;

        SprayStopWatchText.text = seconds.ToString("00.00");
    }

    void restartTime(){
        curTime = 60f;
    }
}
