using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenScript : MonoBehaviour
{
    [SerializeField] private RTHStopWatchScript rTHStopWatchScript;
    [SerializeField] private TextModifier scoreText;
    [SerializeField] private GameObject highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        string scoreString = rTHStopWatchScript.GetTime();
        scoreText.ChangeText(scoreString);

        if(rTHStopWatchScript.IsHighScore()){
            highScoreText.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
