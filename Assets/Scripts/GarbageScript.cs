using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageScript : MonoBehaviour
{
    private Rect screenRect;
    private int counter;
    public GameObject Trash;
    public MiniGameController miniGameControllerInstance;
    public List<AudioClip> clips;

    // Start is called before the first frame update
    void Start()
    {
        counter = 10;
        screenRect = new Rect(0,0, Screen.width, Screen.height);
        int i = 0;
        while (i < counter){
            float randomX = Random.Range(-1 * Screen.width / 4, Screen.width / 4);
            float randomY = Random.Range(-1 * Screen.height / 4, Screen.height / 4);
            GameObject t = Instantiate(Trash, transform.position, transform.rotation);
            t.transform.Translate(new Vector2(randomX, randomY));
            t.transform.SetParent(transform, false);
            i++;
        }

        counter = 0;
        miniGameControllerInstance = GameObject.Find("Camera Mini Games").GetComponent<MiniGameController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ThrashThrown(){
        counter++;
        miniGameControllerInstance.AddProgressTrack(counter, 10, true);

        if(counter >= 10){
            miniGameControllerInstance.PlaySound(clips[0], false);
            miniGameControllerInstance.CloseMiniGameDelay(this.gameObject, "Sapu", 2f);
        }
    }
}
