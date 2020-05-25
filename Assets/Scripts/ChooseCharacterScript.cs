using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterScript : MonoBehaviour
{
    public List<RuntimeAnimatorController> characterControllers;
    public Animator MainCharacterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        MainCharacterAnimator.enabled = false;
    }

    void OnEnable(){
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(int index){
        MainCharacterAnimator.enabled = true;
        MainCharacterAnimator.runtimeAnimatorController = characterControllers[index];
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }
}
