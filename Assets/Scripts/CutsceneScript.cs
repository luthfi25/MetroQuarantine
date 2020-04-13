using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneScript : MonoBehaviour
{
    public UIScript ui;
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
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
    }

    public void ShowNext(GameObject next)
    {
        if(next.name == this.gameObject.name)
        {
            this.gameObject.SetActive(false);
            ui.ReplayGame();
        }

        string[] names = next.name.Split(' ');
        int prev = int.Parse(names[1]) - 1;
        string currentGameObj = names[0] + " " + prev;
        GameObject current = GameObject.Find(currentGameObj);

        current.SetActive(false);
        next.SetActive(true);
    }
}
