using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{
    [SerializeField] private RTHStopWatchScript rTHStopWatchScript;
    [SerializeField] private TextModifier scoreText;
    [SerializeField] private GameObject highScoreText;
    [SerializeField] private GameObject nextLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        string scoreString = rTHStopWatchScript.GetTime();
        scoreText.ChangeText(scoreString);

        if(rTHStopWatchScript.IsHighScore()){
            highScoreText.SetActive(true);
        }

        int curLevel = SceneManager.GetActiveScene().buildIndex;
        if(curLevel + 1 < SceneManager.sceneCount){
            nextLevelButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
