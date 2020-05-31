using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererAnim : MonoBehaviour
{
    public SpriteRenderer ReferenceSprite;
    private Sprite curSprite;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        curSprite = ReferenceSprite.sprite;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = curSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(ReferenceSprite.sprite != curSprite){
            curSprite = ReferenceSprite.sprite;
            spriteRenderer.sprite = curSprite;
        }
    }
}
