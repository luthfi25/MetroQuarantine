using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskAnim : MonoBehaviour
{
    public SpriteRenderer ReferenceSprite;
    private Sprite curSprite;
    private SpriteMask spriteMask;
    // Start is called before the first frame update
    void Start()
    {
        curSprite = ReferenceSprite.sprite;
        spriteMask = GetComponent<SpriteMask>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ReferenceSprite.sprite != curSprite){
            curSprite = ReferenceSprite.sprite;
            spriteMask.sprite = curSprite;
        }
    }
}
