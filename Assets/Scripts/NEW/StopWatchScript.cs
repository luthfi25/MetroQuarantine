using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatchScript : MonoBehaviour
{
    [SerializeField] private TextModifier stopWatchText;
    [SerializeField] private TextModifier stopWatchExtensionText;

    private float minutes;
    private float seconds;
    private float currentTime;
    private bool isFreeze = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFreeze){
            return;
        }

        CalculateTime();
    }

    void CalculateTime(){
        currentTime += Time.deltaTime;
        seconds = currentTime % 60;
        minutes = (int)(currentTime / 60) % 60;

        stopWatchText.ChangeText(minutes.ToString("00") + ":" + seconds.ToString("00.00"));
    }

    public string GetTime(){
        seconds = currentTime % 60;
        minutes = (int)(currentTime / 60) % 60; 
        return minutes.ToString("00") + ":" + seconds.ToString("00.00");
    }

    public bool IsHighScore(int level){
        float bestTime = PlayerPrefs.GetFloat("HighScore-"+level.ToString(), float.MaxValue);

        if (currentTime < bestTime)
        {
            PlayerPrefs.SetFloat("HighScore-"+level.ToString(), currentTime);
            return true;
        }

        return false;
    }

    public void AddTime(int time)
    {
        currentTime += (float) time ;
        string extensionText = "+" + time + "s!";
        stopWatchExtensionText.ChangeText(extensionText);
        stopWatchExtensionText.gameObject.SetActive(true);
        isFreeze = true;
    }

    public void AddTimeFloat(float time){
        currentTime += time;
    }

    public void DisableExtension(){
        isFreeze = false;
        stopWatchExtensionText.gameObject.SetActive(false);
    }

    public void SetFreeze(bool freezeValue){
        isFreeze = freezeValue;
    }
}
