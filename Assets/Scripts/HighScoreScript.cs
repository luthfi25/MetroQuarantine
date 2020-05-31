using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> highScorePanel;
    [SerializeField] private List<TextMeshProUGUI> highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        for (int level = 0; level <= 2; level++){
            float highScore = PlayerPrefs.GetFloat("HighScore-"+(level+1).ToString(), 0.0f);

            if(highScore <= 0){
                Image image;
                if(highScorePanel[level].TryGetComponent<Image>(out image)){
                    image.color = Color.black;
                }

                highScoreText[level].text = "N/A";
            } else {
                float seconds = highScore % 60;
                int minutes = (int)(highScore / 60) % 60;

                highScoreText[level].text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }
        }         
    }

    // Update is called once per frame
    void Update()
    {

    }
}
