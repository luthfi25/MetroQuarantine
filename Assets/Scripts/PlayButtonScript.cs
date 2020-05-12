using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayButtonScript : MonoBehaviour
{
    AudioSource audio;
    public GameObject creditScene;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        creditScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadSceneAsync(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void buttonPressed()
    {
        audio.Play();
    }

    public void openCredit()
    {
        creditScene.SetActive(true);
    }

    public void closeCredit()
    {
        creditScene.SetActive(false);
    }

    public void LoadTestGesture(TextMeshProUGUI password){
        if(password.text.Contains("luthfiganteng")){
            SceneManager.LoadScene(2);
        }
    }
}
