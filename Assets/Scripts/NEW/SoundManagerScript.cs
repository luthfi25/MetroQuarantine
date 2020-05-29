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
        MasterAudioSource.clip = clip;
        MasterAudioSource.Play();
    }

    public void PlayMaster2nd(AudioClip clip){
        Master2ndAudioSource.clip = clip;
        Master2ndAudioSource.Play();
    }

    public void PlayOneShot(AudioClip clip){
        OneShotAudioSource.PlayOneShot(clip);
    }

    public void DisableMasters(){
        MasterAudioSource.Stop();
        Master2ndAudioSource.Stop();
    }
}
