﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject healthManager;
    public GameObject winScreen;
    public GameObject HighScoreText;
    [SerializeField]  Text TimeText;

    HealthManager hmScript;
    bool isWinTriggered;

    public GoalScript goalScript;
    AudioSource audio;
    public AudioClip winSound;

    int firstTimePlay;
    public GameObject cutScene;

    // Start is called before the first frame update
    void Start()
    {
        isWinTriggered = false;
        HighScoreText.gameObject.SetActive(false);
        pauseScreen.SetActive(false);
        hmScript = healthManager.GetComponent<HealthManager>();
        audio = GetComponent<AudioSource>();

        firstTimePlay = PlayerPrefs.GetInt("FirstTime", 1);
        if (firstTimePlay == 1)
        {
            PlayerPrefs.SetInt("FirstTime", 0);
            cutScene.GetComponent<CutsceneScript>().PlayCutscene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        
        //WIN CONDITION
        if (goals.Length == 0)
        {
            WinGame();
        }
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;
        hmScript.DestroyHealth(-1);
        goalScript.ResetGoal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        hmScript.DestroyHealth(-1);
        goalScript.ResetGoal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ExitGame()
    {
        hmScript.DestroyHealth(-1);
        goalScript.ResetGoal();
        Application.Quit();
    }

    public void WinGame()
    {
        if (isWinTriggered)
        {
            return;
        }

        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }

        audio.clip = winSound;
        audio.loop = false;
        audio.Play();

        isWinTriggered = true;
        winScreen.SetActive(true);

        // set Highscore
        float curTime = PlayerPrefs.GetFloat("Stopwatch", 0.0f);
        float bestTime = PlayerPrefs.GetFloat("HighScore", float.MaxValue);

        if (curTime < bestTime)
        {
            HighScoreText.gameObject.SetActive(true);
            PlayerPrefs.SetFloat("HighScore", curTime);
        }

        float seconds = curTime % 60;
        int minutes = (int)(curTime / 60) % 60;

        TimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00.00");

        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Stopwatch");
        Time.timeScale = 0;
    }

    public void buttonPressed()
    {
        audio.Play();
    }
}