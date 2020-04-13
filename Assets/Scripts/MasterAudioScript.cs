using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAudioScript : MonoBehaviour
{

    AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();
        DontDestroyOnLoad(audio);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
