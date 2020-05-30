using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{
    [SerializeField] private TextModifier scoreText;
    [SerializeField] private GameObject highScoreText;
    [SerializeField] private GameObject nextLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(string score, bool isHighScore, bool isNextLevel){
        scoreText.ChangeText(score);

        if(isHighScore){
            highScoreText.SetActive(true);
        }
        
        if(isNextLevel){
            nextLevelButton.SetActive(true);
        }
    }
}
