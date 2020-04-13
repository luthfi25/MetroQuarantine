using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour
{
    [SerializeField] Text highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        highScoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0.0f);
        
        if(highScore <= 0)
        {
            highScoreText.text = "N/A";
        } else
        {
            float seconds = highScore % 60;
            int minutes = (int)(highScore / 60) % 60;

            highScoreText.text = minutes.ToString("00") + ":" + seconds.ToString("00.00");
        }
    }
}
