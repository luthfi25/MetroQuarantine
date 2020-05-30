using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorScript : MonoBehaviour
{
    [SerializeField] private GameObject FirstLevel;
    [SerializeField] private GameObject SecondLevel;

    // Start is called before the first frame update
    void Start()
    {
        int houseFirstTime = PlayerPrefs.GetInt("House-FirstTime", -1);
        int rthFirstTime = PlayerPrefs.GetInt("RTH-FirstTime", -1);

        if(houseFirstTime == 1){
            FirstLevel.SetActive(true);
        }

        if(rthFirstTime == 1){
            SecondLevel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
