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

    public List<AudioClip> clips;
    private AudioClip broomingClip;
    bool isClosing;

    // Start is called before the first frame update
    void Start()
    {
        miniGameControllerInstance = GameObject.Find("Camera Mini Games").GetComponent<MiniGameController>();
        broomingClip = clips[0];

        float[] broomingClipSample = new float[]{};
        broomingClip.GetData(broomingClipSample, 0);
        broomingClip.SetData(broomingClipSample, 2); 
    }

    void OnEnable(){
        isRight = true;
        broomCounter = 0;
        targetFill = 0f;
        coolingDown = false;
        isClosing = false;
    }

    void OnDisable(){
    }

    // Update is called once per frame
    void Update()
    {
        if(broomCounter >= 50 && !isClosing){
            isClosing = true;
            miniGameControllerInstance.PlaySound(clips[1], false);
            miniGameControllerInstance.CloseMiniGameDelay(this.gameObject, "Sapu", 2f);
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
        if(broomCounter >= 49){
            Destroy(btn.gameObject);
        }

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
        miniGameControllerInstance.AddProgressTrackSmall(broomCounter, 50, true);
        
        // targetFill = BackgroundMessy.fillAmount - (1f/ 20f);
        targetFill = BackgroundMessy.color.a - (1f / 50f);
        coolingDown = true;
        miniGameControllerInstance.PlaySound(broomingClip, false);
    }

    public void RestartBroom(){
        broomCounter = 0;
    }
}
