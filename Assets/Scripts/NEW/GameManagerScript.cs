using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        chooseCharacterScreen.SetActive(true);
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

    public void WinGame(){
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    public void GoalTook(){
        goalsTook++;
        goalText.ChangeText(goalsTook.ToString() + "/10");

        if(goalsTook >= 10){
            WinGame();
        }
    }

    public IEnumerator PlayerDamage(){
        Handheld.Vibrate();
        rTHStopWatchScript.AddTime(10);
        rTHHealthScript.DecreaseHealth();

        yield return new WaitForSeconds(1.5f);

        rTHStopWatchScript.DisableExtension();
        mainCharacterScript.Reset();
        enemySpawnerScript.Reset();
        rTHHealthScript.Reset();

        goalsTook = 0;
        goalText.ChangeText(goalsTook.ToString() + "/10");
    }

    public void GameOver(){
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }

    public void StartGame(){
        Time.timeScale = 1;
        goalText.ChangeText(goalsTook.ToString() + "/10");
    }
}
