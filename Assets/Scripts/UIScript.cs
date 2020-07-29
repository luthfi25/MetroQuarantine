using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject healthManager;
    public GameObject HighScoreText;

    HealthManager hmScript;
    bool isWinTriggered;

    AudioSource audio;

    public GameObject cutScene;
    public GameObject chooseCharacter;

    // Start is called before the first frame update
    void Start()
    {
        isWinTriggered = false;
        if(HighScoreText != null){
            HighScoreText.gameObject.SetActive(false);
        }

        if(pauseScreen != null){
            pauseScreen.SetActive(false);
        }

        if(healthManager != null){
            hmScript = healthManager.GetComponent<HealthManager>();
        }

        audio = GetComponent<AudioSource>();

        chooseCharacter.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        
        //WIN CONDITION
        if (goals.Length == 0)
        {
            WinGame();
        }
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;

        MiniGameController mgc = GameObject.Find("Camera Mini Games").GetComponent<MiniGameController>();
        if(mgc != null){
            mgc.ToggleGUI(true);
        }
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;
        // hmScript.DestroyHealth(-1);
        // goalScript.ResetGoal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        // hmScript.DestroyHealth(-1);
        // goalScript.ResetGoal();
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        // hmScript.DestroyHealth(-1);
        // goalScript.ResetGoal();
        PlayerPrefs.DeleteKey("VolumeStatus");
        PlayerPrefs.DeleteKey("VolumeLevel");
        Application.Quit();
    }

    public void WinGame()
    {
        return;

        // if (isWinTriggered)
        // {
        //     return;
        // }

        // AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        // foreach (AudioSource audioS in allAudioSources)
        // {
        //     audioS.Stop();
        // }

        // audio.clip = winSound;
        // audio.loop = false;
        // audio.Play();

        // isWinTriggered = true;
        // winScreen.SetActive(true);

        // // set Highscore
        // float curTime = PlayerPrefs.GetFloat("Stopwatch", 0.0f);
        // float bestTime = PlayerPrefs.GetFloat("HighScore", float.MaxValue);

        // if (curTime < bestTime)
        // {
        //     HighScoreText.gameObject.SetActive(true);
        //     PlayerPrefs.SetFloat("HighScore", curTime);
        // }

        // float seconds = curTime % 60;
        // int minutes = (int)(curTime / 60) % 60;

        // TimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00.00");

        // PlayerPrefs.DeleteKey("Health");
        // PlayerPrefs.DeleteKey("Stopwatch");
        // Time.timeScale = 0;
    }

    public void buttonPressed()
    {
        audio.Play();
    }

    public void DisablePause(){
        this.gameObject.GetComponent<Button>().interactable = false;
    }

    public void EnablePause(){
        this.gameObject.GetComponent<Button>().interactable = true;
    }
}
