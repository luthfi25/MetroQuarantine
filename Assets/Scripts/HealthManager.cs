using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    GameObject[] healths;
    public GameObject deadHealth;

    public GameObject stopWatch;
    StopWatchScript stopWatchInstance;

    public Image gameOver;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        int healthCount = PlayerPrefs.GetInt("Health", 2);
        int healthToDisable =  2 - healthCount;
        GameObject[] healths = GameObject.FindGameObjectsWithTag("Health");

        stopWatchInstance = stopWatch.GetComponent<StopWatchScript>();
        if (healthToDisable == 0)
        {
            stopWatchInstance.DestroyTime();
        }

        foreach (GameObject health in healths){
            if(healthToDisable > 0)
            {
                GameObject dh = Instantiate(deadHealth, health.transform.position, health.transform.rotation);
                
                dh.transform.SetParent(transform);
                dh.transform.localScale = new Vector3(1f, 1f, 1f);
                health.SetActive(false);
                healthToDisable--;
            }
        }

        audio = GetComponent<AudioSource>();
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

            stopWatchInstance.DestroyTime();
            PlayerPrefs.DeleteKey("Health");
            return;
        }

        //Animate losing health
        GameObject[] healths = GameObject.FindGameObjectsWithTag("Health");
        Animator healthAnim = healths[0].GetComponent<Animator>();
        healthAnim.enabled = true;

        // Trigger Player Animation
        GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>().TriggerDead();

        if (healths.Length > 1 && toDestroy > 1)
        {
            healthAnim = healths[1].GetComponent<Animator>();
            healthAnim.enabled = true;
        }

        int healthCount = PlayerPrefs.GetInt("Health", 2);
       

        if (healthCount <= 0)
        {
            //???????
            if (stopWatchInstance == null)
            {
                stopWatchInstance = stopWatch.GetComponent<StopWatchScript>();
            }

            stopWatchInstance.DestroyTime();
            PlayerPrefs.DeleteKey("Health");

            StartCoroutine(showGameOver());

        }
        else
        {
            healthCount--;

            if (toDestroy > 1)
            {
                if(healthCount == 0)
                {
                    //???????
                    if (stopWatchInstance == null)
                    {
                        stopWatchInstance = stopWatch.GetComponent<StopWatchScript>();
                    }

                    stopWatchInstance.DestroyTime();
                    PlayerPrefs.DeleteKey("Health");

                    StartCoroutine(showGameOver());

                    return;
                }
                healthCount--;
            }


            PlayerPrefs.SetInt("Health", healthCount);
            StartCoroutine(restartScene());
        }
    }

    IEnumerator restartScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator showGameOver()
    {
        yield return new WaitForSeconds(1.5f);

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
