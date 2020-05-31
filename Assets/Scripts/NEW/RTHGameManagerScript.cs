using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTHGameManagerScript : MonoBehaviour, IGameManager
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private StopWatchScript stopWatchScript;
    [SerializeField] private TextModifier goalText;
    [SerializeField] private int goalsTook;
    [SerializeField] private MainCharacterScript mainCharacterScript;
    [SerializeField] private EnemySpawnerScript enemySpawnerScript;
    [SerializeField] private GameObject chooseCharacterScreen;
    [SerializeField] private GameObject tutorialScreen;

    [SerializeField] private int health = 3;
    const int MAX_HEALTH = 3;
    [SerializeField] private HealthScript healthScript;

    [SerializeField] private SoundManagerScript soundManagerScript;
    [SerializeField] private AudioClip masterClip;
    [SerializeField] private AudioClip master2ndClip;
    [SerializeField] private AudioClip buttonPressedClip;
    [SerializeField] private AudioClip playerDamageClip;
    [SerializeField] private AudioClip playerDamageMaleClip;
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

        PlayerPrefs.SetInt("RTH-FirstTime", 1);
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

        WinScreenScript winScreenScript;

        if(winScreen.TryGetComponent<WinScreenScript>(out winScreenScript)){
            string scoreString = stopWatchScript.GetTime();
            int curLevel = SceneManager.GetActiveScene().buildIndex;
            bool isHighScore = stopWatchScript.IsHighScore(curLevel);
            bool isNextLevel = curLevel + 1 < SceneManager.sceneCountInBuildSettings;
            
            winScreenScript.SetScore(scoreString, isHighScore, isNextLevel);
        }
    }

    public void GoalTook(string goalName){
        goalsTook++;
        goalText.ChangeText(goalsTook.ToString() + "/12");
        PlayClip("goal");

        if(goalsTook >= 12){
            WinGame();
        }
    }

    public IEnumerator PlayerDamage(string enemyName){
        health--;
        Handheld.Vibrate();
        stopWatchScript.AddTime(10);
        healthScript.DecreaseHealth(health);
        enemySpawnerScript.ForceFreeze();
        
        if(mainCharacterScript.GetName().Equals("Fritz")){
            PlayClip("damage-male");
        } else {
            PlayClip("damage");
        }

        yield return new WaitForSeconds(1.5f);

        if(health <= 0){
            GameOver();
        } else {
            healthScript.Reset(health);

            stopWatchScript.DisableExtension();
            mainCharacterScript.Reset();
            enemySpawnerScript.Reset();
        }
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
            case "damage-male":
                soundManagerScript.PlayOneShot(playerDamageMaleClip);
                break;
            default:
                break;
        }
    }

    public void ActivateChooseCharacter(){
        Destroy(tutorialScreen);
        chooseCharacterScreen.SetActive(true);
    }

    public void RunCustomFunction(string functionName){
       return; 
    }
}
