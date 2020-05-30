using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public GameObject stopWatch;
    StopWatchScript stopWatchInstance;

    public Image gameOver;

    AudioSource audio;

    public GameObject[] Characters;

    public GameObject[] Healths;
    public GameObject[] DeadHealths;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Health", 2);
        audio = GetComponent<AudioSource>();
        recalculateHealth();
    }

    void recalculateHealth(){
        // GameObject[] healths = GameObject.FindGameObjectsWithTag("Health");
        // GameObject[] deadHealths = GameObject.FindGameObjectsWithTag("Dead-Health");

        int healthCount = PlayerPrefs.GetInt("Health", 2);
        for(int i = 0; i < DeadHealths.Length; i++){
            if(healthCount >= 0){
                DeadHealths[i].GetComponent<Image>().enabled = false;
                healthCount--;
            } else {
                Healths[i].SetActive(false);
                DeadHealths[i].GetComponent<Image>().enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DestroyHealth(int toDestroy)
    {

        //from pause menu
        if (toDestroy == -1)
        {
            //???????
            if (stopWatchInstance == null)
            {
                stopWatchInstance = stopWatch.GetComponent<StopWatchScript>();
            }

            // stopWatchInstance.DestroyTime();
            PlayerPrefs.DeleteKey("Health");
            return;
        }

        //Animate losing health
        // GameObject[] healths = GameObject.FindGameObjectsWithTag("Health");
        int healthCount = PlayerPrefs.GetInt("Health", 2);
        Animator healthAnim = Healths[healthCount].GetComponent<Animator>();
        healthAnim.enabled = true;

        // Trigger Player Animation
        GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>().TriggerDead();

        if (healthCount > 0 && toDestroy > 1)
        {
            healthAnim = Healths[healthCount-1].GetComponent<Animator>();
            healthAnim.enabled = true;
        }

        healthCount -= toDestroy;

        if (healthCount < 0)
        {
            //???????
            if (stopWatchInstance == null)
            {
                stopWatchInstance = stopWatch.GetComponent<StopWatchScript>();
            }
            PlayerPrefs.DeleteKey("Health");
            StartCoroutine(showGameOver());

        }
        else
        {
            PlayerPrefs.SetInt("Health", healthCount);
            StartCoroutine(restartScene());
        }
    }

    IEnumerator restartScene()
    {
        yield return new WaitForSeconds(1.5f);
        foreach(GameObject c in Characters){
            MovementScript ms = c.GetComponent<MovementScript>();
            EnemyScript es = c.GetComponent<EnemyScript>();

            if (ms != null){
                ms.ResetPosition();
            } else if (es != null){
                es.ResetPosition();
            }
        }
        recalculateHealth();
    }

    IEnumerator showGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        // stopWatchInstance.DestroyTime();

        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }

        audio.loop = false;
        audio.volume = 0.4f;
        audio.Play();
        Time.timeScale = 0;
        gameOver.gameObject.SetActive(true);
    }
}
