using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    private AudioSource MasterAudioSource;
    private AudioSource Master2ndAudioSource;
    private AudioSource OneShotAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        doGetComponents();
    }

    void doGetComponents(){
        AudioSource[] audioSources = GetComponents<AudioSource>();
        MasterAudioSource = audioSources[0];
        Master2ndAudioSource = audioSources[1];
        OneShotAudioSource = audioSources[2];
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
}
