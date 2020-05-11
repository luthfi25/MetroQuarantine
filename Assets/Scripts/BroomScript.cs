using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroomScript : MonoBehaviour
{
    private int broomCounter;
    public MiniGameController miniGameControllerInstance;
    public Image BackgroundMessy;
    bool isRight;

    private float targetFill;
    private bool coolingDown;
    public GameObject Tutorial;
    public GameObject SuccessText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        isRight = true;
        broomCounter = 0;
        targetFill = 0f;
        coolingDown = false;
        Tutorial.SetActive(true);
        SuccessText.SetActive(false);
    }

    void OnDisable(){
    }

    // Update is called once per frame
    void Update()
    {
        if(broomCounter >= 50){
            miniGameControllerInstance.CloseMiniGame(this.gameObject, "Sapu");
        }

        if(!miniGameControllerInstance.CoolingDown){
            SuccessText.SetActive(false);
        }

        if(coolingDown){
            // BackgroundMessy.fillAmount -= 0.25f * Time.deltaTime;
            // if(BackgroundMessy.fillAmount <= targetFill){
            //     coolingDown = false;
            // }

            float newA = BackgroundMessy.color.a - 0.25f * Time.deltaTime;
            BackgroundMessy.color = new Color(BackgroundMessy.color.r, BackgroundMessy.color.g, BackgroundMessy.color.b, newA);

            if(BackgroundMessy.color.a <= targetFill){
                coolingDown = false;
            }
        }
    }

    public void MoveButton(GameObject btn){
        if(isRight){
            btn.gameObject.transform.Translate(-5f, 0f, 0f);
            isRight = false;
        } else {
            btn.gameObject.transform.Translate(5f, 0f, 0f);
            isRight = true;
        }

        // float randX = Random.Range(Screen.width / 3, 2 * Screen.width / 3);
        // Vector3 curPos = btn.gameObject.GetComponent<RectTransform>().localPosition;
        // Vector3 parentPos = btn.transform.GetComponentInParent<RectTransform>().anchoredPosition.
        // btn.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(randX, curPos.y, curPos.z);

        broomCounter++;
        miniGameControllerInstance.AddProgressTrack(broomCounter, 50, false);
        SuccessText.SetActive(true);
        
        // targetFill = BackgroundMessy.fillAmount - (1f/ 20f);
        targetFill = BackgroundMessy.color.a - (1f / 50f);
        coolingDown = true;
    }

    public void RestartBroom(){
        broomCounter = 0;
    }
}
