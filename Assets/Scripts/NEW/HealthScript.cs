using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private List<GameObject> healthGameObjects;
    [SerializeField] private List<GameObject> deadHealthGameObjects;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(int newHealth){
        for(int i = 0; i < healthGameObjects.Count; i++){
            if(i >= newHealth){
                //destroy
                healthGameObjects[i].SetActive(false);
            }
        }
    }

    public void DecreaseHealth(int newHealth){
        for(int i = 0; i < healthGameObjects.Count; i++){
            if(i >= newHealth){
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
