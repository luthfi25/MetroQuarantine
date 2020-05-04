using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopWatchScript : MonoBehaviour
{
    float minutes;
    float seconds;
    public GameObject alert;
    [SerializeField] Text alertText;
    [SerializeField] Text stopWatchText;
    bool timePause;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("Stopwatch", 0.0f);
        alert.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (MovementScript.isPaused)
        {
            if (timePause)
            {
                timePause = false;
                StartCoroutine(pausedCase(0.1f));

            }
        } else
        {
            StopWatchCalcul();
        }
    }

    IEnumerator pausedCase(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StopWatchCalcul();
    }

    void StopWatchCalcul()
    {
        float curTime = PlayerPrefs.GetFloat("Stopwatch", 0.0f);
        curTime += Time.deltaTime;
        seconds = curTime % 60;
        minutes = (int)(curTime / 60) % 60;

        stopWatchText.text = minutes.ToString("00") + ":" + seconds.ToString("00.00");
        PlayerPrefs.SetFloat("Stopwatch", curTime);
        
    }

    public void AddTime(int time)
    {
        float curTime = PlayerPrefs.GetFloat("Stopwatch", 0.0f);
        curTime += (float) time ;
        PlayerPrefs.SetFloat("Stopwatch", curTime);

        alertText = alert.GetComponent<Text>();
        alertText.text = "+" + time + "s!";
        alert.SetActive(true);
        StartCoroutine(disableAlert());

        timePause = MovementScript.isPaused;
    }

    IEnumerator disableAlert(){
        yield return new WaitForSeconds(2.0f);
        alert.SetActive(false);
    }

    public void DestroyTime()
    {
        PlayerPrefs.DeleteKey("Stopwatch");
    }
}
