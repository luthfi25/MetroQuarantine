using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void ReplayGame();
    void QuitGame();
    void GoToScene(int sceneIndex);
    void NextLevel();
    void WinGame();
    void GoalTook(string goalName);
    void GameOver();
    void StartGame();
    void PlayClip(string option);
    void ActivateChooseCharacter();
    IEnumerator PlayerDamage(string enemyName);
    void RunCustomFunction(string functionName);
}