using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneScript : MonoBehaviour
{
    public UIScript ui;
    private GameObject firstScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCutscene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1){
            Time.timeScale = 0;
        }

        this.gameObject.SetActive(true);
    }

    public void ShowNext(GameObject next)
    {   
        if(next.name == this.gameObject.name)
        {
            if(SceneManager.GetActiveScene().buildIndex == 1){
                ui.ReplayGame();
                PlayerPrefs.SetInt("FirstTime", 0);
            } else {
                firstScene.SetActive(true);
                GameObject.Find("Cutscene 11").SetActive(false);
            }

            this.gameObject.SetActive(false);
            return;
        }

        string[] names = next.name.Split(' ');
        int prev = -1;
        bool result = int.TryParse(names[1], out prev);
        if(result){
            prev -= 1;
        }
        
        string currentGameObj = names[0] + " " + prev;
        GameObject current = GameObject.Find(currentGameObj);

        if(firstScene == null){
            firstScene = current;
        }

        current.SetActive(false);
        next.SetActive(true);
    }
}
