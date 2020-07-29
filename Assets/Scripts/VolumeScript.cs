using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image volumeToggle;
    [SerializeField] private Sprite volumeActive;
    [SerializeField] private Sprite volumeMute;
    [SerializeField] private SoundManagerScript soundManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        initVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initVolume(){
        int checkVolStatus = PlayerPrefs.GetInt("VolumeStatus", -1);
        if(checkVolStatus == -1){
            checkVolStatus = 1;
            PlayerPrefs.SetInt("VolumeStatus", checkVolStatus);
        }

        if(checkVolStatus == 0){
            volumeToggle.sprite = volumeMute;
        } else {
            volumeToggle.sprite = volumeActive;
        }

        float checkVolLevel = PlayerPrefs.GetFloat("VolumeLevel", -1f);
        if(checkVolLevel <= -0.9f){
            checkVolLevel = 1f;
            PlayerPrefs.SetFloat("VolumeLevel", checkVolLevel);
        }
        volumeSlider.value = checkVolLevel;

        soundManagerScript.ToggleVolume();
        soundManagerScript.CalibrateVolumeLevel();
    }

    public void ToggleVolume(){
        int curState = -1;
        int checkVolStatus = PlayerPrefs.GetInt("VolumeStatus", 0);
        
        if(checkVolStatus == 0){
            curState = 1;
            volumeToggle.sprite = volumeActive;            
        } else {
            curState = 0;
            volumeToggle.sprite = volumeMute;
        }

        PlayerPrefs.SetInt("VolumeStatus", curState);
        soundManagerScript.ToggleVolume();
    }

    public void CalibrateVolumeLevel(){
        float volumelevel = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeLevel", volumelevel);
        soundManagerScript.CalibrateVolumeLevel();
    }
}
