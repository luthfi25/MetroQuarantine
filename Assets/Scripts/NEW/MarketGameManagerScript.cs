using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarketGameManagerScript : MonoBehaviour, IGameManager
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private StopWatchScript stopWatchScript;
    [SerializeField] private MainCharacterScript mainCharacterScript;
    [SerializeField] private EnemySpawnerScript enemySpawnerScript;
    [SerializeField] private GameObject chooseCharacterScreen;
    [SerializeField] private GameObject tutorialScreen;

    [SerializeField] private int health = 3;
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

    [SerializeField] private List<Vector2> goalPool;
    [SerializeField] private List<string> goalPoolSprite;
    [SerializeField] const int GOAL_POOL_SIZE = 6;

    [SerializeField] private List<Image> goalPoolImage;
    [SerializeField] private GameObject cashierIcon;
    [SerializeField] private GameObject cashierText;


    [SerializeField] private GameObject CameraMiniGames;
    [SerializeField] private GameObject CameraMain;
    [SerializeField] private float goalTime;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        tutorialScreen.SetActive(true);
        PlayClip("master");

        PlayerPrefs.SetInt("Market-FirstTime", 1);
        goalPool = new List<Vector2>();
        goalPoolSprite = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if(goalPool.Count > 0){
            return;
        }

        updateGoalPool();
    }

    void updateGoalPool(){
        List<GameObject> goals = new List<GameObject>(GameObject.FindGameObjectsWithTag("Goal-Market"));
        int i = 0;

        while(i < GOAL_POOL_SIZE){
            int randIndex = Random.Range(0, goals.Count);
            Vector2 randPos = goals[randIndex].transform.position;

            SpriteRenderer sr = goals[randIndex].GetComponentInChildren<SpriteRenderer>();

            if(!goalPool.Contains(randPos)){
                goalPool.Add(randPos);

                if(sr != null){
                    goalPoolImage[i].sprite = sr.sprite;
                    goalPoolSprite.Add(sr.sprite.name);
                }

                goals.RemoveAt(randIndex);
                i++;
            }
        }
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
        if(goalName.Contains("Cashier")){
            PlayClip("goal");
            CameraMiniGames.SetActive(true);
            CameraMain.SetActive(false);
            // stopWatchScript.SetFreeze(true); 
            goalTime = Time.fixedTime;
            enemySpawnerScript.ForceFreeze();

            MiniGameController miniGameController;
            if(CameraMiniGames.TryGetComponent<MiniGameController>(out miniGameController)){
                miniGameController.ActivateTutorial(goalName);
            }
        }

        if(goalPoolSprite.Count > 0){
            return;
        }

        GameObject[] remainingGoals = GameObject.FindGameObjectsWithTag("Goal-Market");
        foreach(GameObject g in remainingGoals){
            Destroy(g);
        }

        cashierIcon.SetActive(true);
        cashierText.SetActive(true);
    }

    void GoalFinished(){
        CameraMiniGames.SetActive(false);
        CameraMain.SetActive(true);

        float timeSpent = Time.fixedTime - goalTime;
        goalTime = 0f;
        stopWatchScript.AddTimeFloat(timeSpent);
        
        WinGame();
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

    public void RunCustomFunction(string functionName){
        if(functionName.Contains("validateGoal")){
            string[] parameters = functionName.Split('/');

            if(parameters[1].Equals("Cashier")){
                return;
            }

            GameObject candGoal = GameObject.Find(parameters[1]);

            if(candGoal != null){
                StartCoroutine(validateGoal(candGoal));
            }
        } else {
            Invoke(functionName, 0f);
        }
    }

    IEnumerator validateGoal(GameObject candGoal){
        yield return new WaitForSeconds(0f);
        
        SpriteRenderer sr = candGoal.GetComponentInChildren<SpriteRenderer>();
        if(sr != null){
            if(goalPoolSprite.Contains(sr.sprite.name)){
                PlayClip("goal");
                foreach(Image img in goalPoolImage){
                    if(img.sprite.name.Equals(sr.sprite.name)){
                        if(img.color == Color.black){
                            continue;
                        }

                        img.color = Color.black;
                        int index = goalPoolSprite.IndexOf(sr.sprite.name);
                        goalPoolSprite.RemoveAt(index);
                        
                        GoalTook(candGoal.name);
                        Destroy(candGoal);
                        break;
                    }
                }
            } else {
                sr.color = new Color(1f, 0f, 0f, 0.5f);
            }
        }
    }
}
