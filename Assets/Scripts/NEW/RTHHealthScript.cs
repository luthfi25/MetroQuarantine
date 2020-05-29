using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTHHealthScript : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private List<GameObject> healthGameObjects;
    [SerializeField] private List<GameObject> deadHealthGameObjects;

    [SerializeField] private RTHGameManagerScript gameManagerScript;

    const int MAX_HEALTH = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        if(health <= 0){
            gameManagerScript.GameOver();
        }

        for(int i = 0; i < healthGameObjects.Count; i++){
            if(i >= health){
                //destroy
                healthGameObjects[i].SetActive(false);
            }
        }
    }

    public void DecreaseHealth(){
        health--;

        for(int i = 0; i < healthGameObjects.Count; i++){
            if(i >= health){
                //destroy
                if(healthGameObjects[i].activeInHierarchy){
                    Animator healthAnim;
                    if (healthGameObjects[i].TryGetComponent<Animator>(out healthAnim)){
                        healthAnim.enabled = true;
                    }   
                }
            }
        }
    }
}
