using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseGameManagerScript : MonoBehaviour, IGameManager
{
    [SerializeField] private GameObject tutorialScreen;
    [SerializeField] private GameObject chooseCharacterScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject cutSceneScreen;

    [SerializeField] private int health = 3;
    const int MAX_HEALTH = 3;
    [SerializeField] private HealthScript healthScript;

    [SerializeField] MainCharacterScript mainCharacterScript;
    [SerializeField] EnemySpawnerScript enemySpawnerScript;
    [SerializeField] StopWatchScript stopWatchScript;

    [SerializeField] private int goalsTook;
    [SerializeField] private GameObject CameraMiniGames;
    [SerializeField] private GameObject CameraMain;

    [SerializeField] private SoundManagerScript soundManagerScript;
    [SerializeField] private AudioClip masterClip;
    [SerializeField] private AudioClip buttonPressedClip;
    [SerializeField] private AudioClip playerDamageClip;
    [SerializeField] private AudioClip playerDamageMaleClip;
    [SerializeField] private AudioClip goalTookClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip doorClip;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;

        int firstTime = PlayerPrefs.GetInt("House-FirstTime", -1);
        if(firstTime == -1){
            cutSceneScreen.SetActive(true);
        } else {
            ActivateTutorial();
        }
        
        PlayClip("master");
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
        PlayClip("goal");

        CameraMiniGames.SetActive(true);
        CameraMain.SetActive(false);
        stopWatchScript.SetFreeze(true); 
        enemySpawnerScript.ForceFreeze();

        MiniGameController miniGameController;
        if(CameraMiniGames.TryGetComponent<MiniGameController>(out miniGameController)){
            miniGameController.ActivateTutorial(goalName);
        }
    }

    public void GoalFinished(){
        goalsTook++;

        CameraMiniGames.SetActive(false);
        CameraMain.SetActive(true);
        stopWatchScript.SetFreeze(false);
        enemySpawnerScript.UnFreeze();
        
        if(goalsTook >= 5){
            WinGame();
        }
    }

    public IEnumerator PlayerDamage(string enemyName){
        health--;
        int timeExtension = 10;

        Handheld.Vibrate();
        enemySpawnerScript.ForceFreeze();

        
        if(mainCharacterScript.GetName().Equals("Fritz")){
            PlayClip("damage-male");
        } else {
            PlayClip("damage");
        }

        if(enemyName.Contains("Babeh")){
            health--;
            timeExtension += 10;
        }

        stopWatchScript.AddTime(timeExtension);
        healthScript.DecreaseHealth(health);
    

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
    }

    public void PlayClip(string option){
        switch(option){
            case "master":
                soundManagerScript.PlayMaster(masterClip);
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
            case "door":
                soundManagerScript.PlayOneShot(doorClip);
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

    public void ActivateTutorial(){
        Destroy(cutSceneScreen);
        tutorialScreen.SetActive(true);
    }

    public void RunCustomFunction(string functionName){
        Invoke(functionName, 0f);
    }

    public void PlayDoorSound(){
        PlayClip("door");
    }
}
