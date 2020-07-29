using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManagerScript : MonoBehaviour
{
    private AudioSource MasterAudioSource;
    private float masterLevelOrig;
    private AudioSource Master2ndAudioSource;
    private float master2ndLevelOrig;
    private AudioSource OneShotAudioSource;
    private float oneShotLevelOrig;

    [SerializeField] private AudioSource mainMenu1;
    [SerializeField] private AudioSource mainMenu2;


    [SerializeField] private MiniGameController miniGameController;
    [SerializeField] private UIScript miniGameCanvas;
    [SerializeField] private List<AudioSource> miniGameAudioSource;
    private float[] miniGameLevelOrig;


    // Start is called before the first frame update
    void Start()
    {
        doGetComponents();
        ToggleVolume();
        CalibrateVolumeLevel();
    }
    void doGetComponents(){
        if(SceneManager.GetActiveScene().buildIndex == 0){
            //only for main menu
            MasterAudioSource = mainMenu1;
            masterLevelOrig = MasterAudioSource.volume;
            Master2ndAudioSource = mainMenu1;
            master2ndLevelOrig = Master2ndAudioSource.volume;
            OneShotAudioSource = mainMenu2;
            oneShotLevelOrig = OneShotAudioSource.volume;
            return;
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();
        MasterAudioSource = audioSources[0];
        masterLevelOrig = MasterAudioSource.volume;
        Master2ndAudioSource = audioSources[1];
        master2ndLevelOrig = Master2ndAudioSource.volume;
        OneShotAudioSource = audioSources[2];
        oneShotLevelOrig = OneShotAudioSource.volume;

        if(miniGameController != null){
            miniGameAudioSource = new List<AudioSource>(miniGameController.gameObject.GetComponents<AudioSource>());
            miniGameAudioSource.Add(miniGameCanvas.gameObject.GetComponent<AudioSource>());
            miniGameLevelOrig = new float[miniGameAudioSource.Count];
        }

        if(miniGameAudioSource != null){
            for(int i = 0; i < miniGameAudioSource.Count; i++){
                miniGameLevelOrig[i] = miniGameAudioSource[i].volume;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMaster(AudioClip clip){
        if(MasterAudioSource == null){
            doGetComponents();
        }

        MasterAudioSource.clip = clip;
        MasterAudioSource.Play();
    }

    public void PlayMaster2nd(AudioClip clip){
        if(Master2ndAudioSource == null){
            doGetComponents();
        }

        Master2ndAudioSource.clip = clip;
        Master2ndAudioSource.Play();
    }

    public void PlayOneShot(AudioClip clip){
        if(OneShotAudioSource == null){
            doGetComponents();
        }

        OneShotAudioSource.PlayOneShot(clip);
    }

    public void DisableMasters(){
        MasterAudioSource.Stop();
        Master2ndAudioSource.Stop();
    }

    public void CalibrateVolumeLevel(){
        float curVolumeLevel = PlayerPrefs.GetFloat("VolumeLevel", 1f);
        MasterAudioSource.volume = masterLevelOrig * curVolumeLevel;
        Master2ndAudioSource.volume = master2ndLevelOrig * curVolumeLevel;
        OneShotAudioSource.volume = oneShotLevelOrig * curVolumeLevel;

        if(miniGameAudioSource != null){
            for(int i = 0; i < miniGameAudioSource.Count; i++){
                miniGameAudioSource[i].volume = miniGameLevelOrig[i] * curVolumeLevel;
            }
        }
    }

    public void ToggleVolume(){
        int volStatus = PlayerPrefs.GetInt("VolumeStatus", -1);
        if(volStatus == 0){
            MasterAudioSource.mute = true;
            Master2ndAudioSource.mute = true;
            OneShotAudioSource.mute = true;

            if(miniGameAudioSource != null){
                for(int i = 0; i < miniGameAudioSource.Count; i++){
                    miniGameAudioSource[i].mute = true;
                }
            }
        } else {
            MasterAudioSource.mute = false;
            Master2ndAudioSource.mute = false;
            OneShotAudioSource.mute = false;

            for(int i = 0; i < miniGameAudioSource.Count; i++){
                miniGameAudioSource[i].mute = false;
            }
        }
    }
}
