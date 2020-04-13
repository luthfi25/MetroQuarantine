using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirandaScript : MonoBehaviour
{
    Image img;
    public Sprite oldPic;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePicture(Sprite picture)
    {
        if (img.sprite == picture)
        {
            return;
        }

        oldPic = img.sprite;
        img.sprite = picture;

        StartCoroutine(restorePicture(1f));
    }

    IEnumerator restorePicture(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        img.sprite = oldPic;
    }
}
