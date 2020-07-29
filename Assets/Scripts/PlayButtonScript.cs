using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayButtonScript : MonoBehaviour
{
    AudioSource audio;
    float audioLevelOrig;
    [SerializeField] AudioSource mainAudioSource;
    float mainAudioSourceLevelOrig;
    public GameObject creditScene;


    [SerializeField] private GameObject levelSelectButton;
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private Image soundSpriteRenderer;
    [SerializeField] private Sprite soundActive;
    [SerializeField] private Sprite soundMute;
    [SerializeField] private Slider volumeLevel;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
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
        PlayerPrefs.DeleteKey("VolumeStatus");
        PlayerPrefs.DeleteKey("VolumeLevel");
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

    public void ActivateWindow(GameObject go){
        go.SetActive(true);
    }

    public void CloseWindow(GameObject go){
        go.SetActive(false);
    }
    public void PlayGame(GameObject go){
        int rthFirstTime = PlayerPrefs.GetInt("RTH-FirstTime", -1);
        if(rthFirstTime == -1){
            ActivateWindow(loadingScreen);
            LoadLevel(1);            
        } else {
            ActivateWindow(go);
        }
    }
}
