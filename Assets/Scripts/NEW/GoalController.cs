using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject GoalPrefab;
    [SerializeField] private List<Sprite> goalSprites; 
    [SerializeField] private List<Vector2> goalShadowPos;
    [SerializeField] private List<Vector2> goalShadowScales;
    [SerializeField] private List<BoxCollider2D> goalSpawnArea;

    // Start is called before the first frame update
    void Start()
    {
        foreach(BoxCollider2D collider2D in goalSpawnArea){
            int i = 1;
            while (i <= 2){
                float randomX = Random.Range(collider2D.bounds.min.x, collider2D.bounds.max.x);
                float randomY = Random.Range(collider2D.bounds.min.y, collider2D.bounds.max.y);
                Vector3 randomPos = new Vector3(randomX, randomY, 0f);

                GameObject gp = Instantiate(GoalPrefab, randomPos, transform.rotation);
            
                int randSpriteIndex = Random.Range(0, goalSprites.Count);
                SpriteRenderer spriteRenderer;
                if(gp.TryGetComponent<SpriteRenderer>(out spriteRenderer)){    
                    spriteRenderer.sprite = goalSprites[randSpriteIndex];
                } else {
                    Debug.LogError("no sprite renderer.");
                }

                Transform shadow = gp.transform.Find("Shadow");
                if(shadow != null){
                    shadow.localPosition = goalShadowPos[randSpriteIndex];
                    shadow.localScale = goalShadowScales[randSpriteIndex];
                } else {
                    Debug.LogError("no shadow.");
                }

                gp.transform.SetParent(transform);
                i++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
