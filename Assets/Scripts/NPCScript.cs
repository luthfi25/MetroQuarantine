using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public Sprite FaceDown;
    public Sprite FaceUp;
    public Sprite FaceLeft;
    public Sprite FaceRight;

    readonly string[] ORIENTATION = {"down", "up", "left", "right"};

    // Start is called before the first frame update
    void Start()
    {
        int initOrientationIndex = Random.Range(0, ORIENTATION.Length);
        string initOrientation = ORIENTATION[initOrientationIndex];
        ChangeOrientation(initOrientation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeOrientation(string orientationVal){
        SpriteRenderer spriteRenderer;

        if(TryGetComponent<SpriteRenderer>(out spriteRenderer)){
            switch(orientationVal){
                case "down":
                    spriteRenderer.sprite = FaceDown;
                    break;
                case "up":
                    spriteRenderer.sprite = FaceUp;
                    break;
                case "left":
                    spriteRenderer.sprite = FaceLeft;
                    break;
                case "right":
                    spriteRenderer.sprite = FaceRight;
                    break;
                default:
                    break;
            }
        }
    }
}
