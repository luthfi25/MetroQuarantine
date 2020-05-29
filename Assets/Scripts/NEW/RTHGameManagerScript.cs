using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTHGameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private RTHStopWatchScript rTHStopWatchScript;
    [SerializeField] private TextModifier goalText;
    [SerializeField] private int goalsTook;
    [SerializeField] private MainCharacterScript mainCharacterScript;
    [SerializeField] private EnemySpawnerScript enemySpawnerScript;
    [SerializeField] private RTHHealthScript rTHHealthScript;
    [SerializeField] private GameObject chooseCharacterScreen;
    [SerializeField] private GameObject tutorialScreen;

    [SerializeField] private SoundManagerScript soundManagerScript;
    [SerializeField] private AudioClip masterClip;
    [SerializeField] private AudioClip master2ndClip;
    [SerializeField] private AudioClip buttonPressedClip;
    [SerializeField] private AudioClip playerDamageClip;
    [SerializeField] private AudioClip goalTookClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip winClip;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        tutorialScreen.SetActive(true);
        PlayClip("master");
        PlayClip("master2nd");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReplayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void GoToScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }

    public void NextLevel(){
        int curLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curLevel + 1);
    }

    public void WinGame(){
        Time.timeScale = 0;
        winScreen.SetActive(true);
        PlayClip("win");
    }

    public void GoalTook(){
        goalsTook++;
        goalText.ChangeText(goalsTook.ToString() + "/12");
        PlayClip("goal");

        if(goalsTook >= 12){
            WinGame();
        }
    }

    public IEnumerator PlayerDamage(){
        Handheld.Vibrate();
        rTHStopWatchScript.AddTime(10);
        rTHHealthScript.DecreaseHealth();
        PlayClip("damage");

        yield return new WaitForSeconds(1.5f);

        rTHStopWatchScript.DisableExtension();
        mainCharacterScript.Reset();
        enemySpawnerScript.Reset();
        rTHHealthScript.Reset();

        goalsTook = 0;
        goalText.ChangeText(goalsTook.ToString() + "/12");
    }

    public void GameOver(){
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
        PlayClip("game over");
    }

    public void StartGame(){
        Time.timeScale = 1;
        goalText.ChangeText(goalsTook.ToString() + "/12");
    }

    public void PlayClip(string option){
        switch(option){
            case "master":
                soundManagerScript.PlayMaster(masterClip);
                break;
            case "master2nd":
                soundManagerScript.PlayMaster2nd(master2ndClip);
                break;
            case "button":
                soundManagerScript.PlayOneShot(buttonPressedClip);
                break;   
            case "damage":
                soundManagerScript.PlayOneShot(playerDamageClip);
                break;  
            case "goal":
                soundManagerScript.PlayOneShot(goalTookClip);
                break;
            case "win":
                soundManagerScript.DisableMasters();
                soundManagerScript.PlayOneShot(winClip);
                break;
            case "game over":
                soundManagerScript.DisableMasters();
                soundManagerScript.PlayOneShot(gameOverClip);
                break;         
            default:
                break;
        }
    }

    public void activateChooseCharacter(){
        Destroy(tutorialScreen);
        chooseCharacterScreen.SetActive(true);
    }
}
