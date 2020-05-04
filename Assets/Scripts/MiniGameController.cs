using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    public GameObject CameraMiniGame;
    public GameObject CameraMain;
    public MovementScript movementScriptInstance;
    public GameObject StopWatch;

    public GameObject ProgressTrackMask;
    public GameObject ProgressTrack;

    private float height;
    private Vector3 destination;
    private Vector3 originDestination;

    // Start is called before the first frame update
    void Start()
    {
        height = ProgressTrackMask.GetComponent<SpriteMask>().bounds.size.y;
        destination = ProgressTrack.transform.position;
        originDestination = ProgressTrackMask.transform.position;
    }

    void OnEnable(){
        StopWatch.SetActive(true);
        ProgressTrack.SetActive(true);
        ProgressTrackMask.SetActive(true);
    }

    void OnDisable(){
        StopWatch.SetActive(false);
        ProgressTrack.SetActive(false);
        ProgressTrackMask.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ProgressTrackMask.transform.position = Vector3.Lerp(ProgressTrackMask.transform.position, destination, 2f * Time.deltaTime);
    }

    public void CloseMiniGame(){
        RestartProgressTrack();
        movementScriptInstance.Unpause();
        CameraMiniGame.SetActive(false);
        CameraMain.SetActive(true);
    }

    public void AddProgressTrack(int step, int bound){
        destination = ProgressTrack.transform.position;
        destination.y += step * height / bound;
    }

    public void RestartProgressTrack(){
        ProgressTrackMask.transform.position = originDestination;
        destination = originDestination;
    }
}
