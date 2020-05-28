using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject GoalPrefab;
    [SerializeField] private int goalCounter;

    // Start is called before the first frame update
    void Start()
    {
        int initCounter = 0;
        while(initCounter < goalCounter){
            float randomX = Random.Range(Screen.width / 10, 9 * Screen.width / 10);
            float randomY = Random.Range(Screen.height / 10, 9 * Screen.height / 10);
            Vector3 randomPos = new Vector3(randomX, randomY, 10f);
            GameObject gp = Instantiate(GoalPrefab, Camera.main.ScreenToWorldPoint(randomPos), transform.rotation);
            gp.transform.SetParent(transform);
            initCounter++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
