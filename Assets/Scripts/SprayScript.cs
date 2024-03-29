﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SprayScript : MonoBehaviour
{
    public GameObject coronaGameObject;
    public Transform coronaSpawnPoint;
    const float WAIT_TIME = 1f;
    public MiniGameController miniGameControllerInstance;
    private int coronaCounter;

    public List<AudioClip> clips;
    private bool isClosing;
    // Start is called before the first frame update
    void Start()
    {
        miniGameControllerInstance = GameObject.Find("Camera Mini Games").GetComponent<MiniGameController>();    
        ActivateCorona();
    }

    void OnEnable(){
        coronaCounter = 0;
        isClosing = false;
    }

    void OnDisable(){
    }

    // Update is called once per frame
    void Update()
    {
        if(coronaCounter >= 15){
            if(!isClosing){
                isClosing = true;
                miniGameControllerInstance.PlaySound(clips[2], false);
                miniGameControllerInstance.CloseMiniGameDelay(this.gameObject, "Semprot", 1f);

                GameObject[] coronas = GameObject.FindGameObjectsWithTag("Corona");
                foreach(GameObject corona in coronas){
                    Destroy(corona);
                }
            }
            return;
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {    
            Vector2 touchPosWorld2D = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if(hitInformation.collider != null) {
                GameObject touchedObject = hitInformation.transform.gameObject;
                if(touchedObject.tag == "Corona"){
                    touchedObject.GetComponent<CoronaScript>().speed = 0f;
                    touchedObject.GetComponent<Animator>().SetTrigger("dead");
                    Destroy(touchedObject, 0.5f);
                    coronaCounter++;
                    miniGameControllerInstance.AddProgressTrackSmall(coronaCounter, 15, true);

                    int randIndex = Random.Range(0, 2);
                    miniGameControllerInstance.PlaySound(clips[randIndex], false);
                }
            }

        } else if(Input.GetMouseButtonDown(0)){
            Vector2 touchPosWorld2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if(hitInformation.collider != null) {
                GameObject touchedObject = hitInformation.transform.gameObject;
               if(touchedObject.tag == "Corona"){
                    touchedObject.GetComponent<CoronaScript>().speed = 0f;
                    touchedObject.GetComponent<Animator>().SetTrigger("dead");
                    Destroy(touchedObject, 0.5f);
                    coronaCounter++;
                    miniGameControllerInstance.AddProgressTrackSmall(coronaCounter, 15, true);

                    int randIndex = Random.Range(0, 2);
                    miniGameControllerInstance.PlaySound(clips[randIndex], false);
                }
            }
        }
    }

    IEnumerator spawnCorona(float waitTime)
    {
        while(this.gameObject.activeSelf && !isClosing) {
            float xValue = Random.Range(1f * (Screen.width / 4), Screen.width - 200f);
            Vector3 normvalue = Camera.main.ScreenToWorldPoint(new Vector3(xValue, 0f, 0f));
            Instantiate(coronaGameObject, new Vector3(normvalue.x, coronaSpawnPoint.position.y, 0f), transform.rotation);

            yield return new WaitForSeconds(waitTime);
        }
    }

    public void RestartSpray(){
        coronaCounter = 0;

        GameObject[] coronas = GameObject.FindGameObjectsWithTag("Corona");
        foreach(GameObject corona in coronas){
            Destroy(corona);
        }
    }

    public void ActivateCorona(){
        StartCoroutine(spawnCorona(WAIT_TIME));
    }
}
