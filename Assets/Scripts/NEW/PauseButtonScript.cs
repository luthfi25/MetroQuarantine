using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePause(){
        this.gameObject.SetActive(false);
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void DisablePause(){
        this.gameObject.SetActive(true);
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}
