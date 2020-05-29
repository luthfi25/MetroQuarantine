using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterScript : MonoBehaviour
{
    [SerializeField] private List<RuntimeAnimatorController> characterControllers;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private MainCharacterScript mainCharacterScript;
    [SerializeField] private RTHGameManagerScript gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseCharacter(int index){
        List<Sprite> chosenSprites = new List<Sprite>();
        for(int i = 0; i <= 3; i++){
            chosenSprites.Add(sprites[(4*index) + i]);
        }

        mainCharacterScript.ChangeAsset(characterControllers[index], chosenSprites);
        gameManagerScript.StartGame();
        Destroy(this.gameObject);
    }
}
