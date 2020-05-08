using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalScript : MonoBehaviour
{
    int[] goalCount;
    public GameObject[] goals;
    Dictionary<int, bool> intToBool = new Dictionary<int, bool>();
    AudioSource audio;
    public AudioClip[] clip;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Buku", 1);
        PlayerPrefs.SetInt("Sabun", 1);
        PlayerPrefs.SetInt("Makan", 1);
        PlayerPrefs.SetInt("Sapu", 1);
        PlayerPrefs.SetInt("Semprot",1);

        intToBool.Add(0, false);
        intToBool.Add(1, true);

        goalCount = new int[5];
        goalCount[0] = PlayerPrefs.GetInt("Buku", 1);
        goalCount[1] = PlayerPrefs.GetInt("Sabun", 1);
        goalCount[2] = PlayerPrefs.GetInt("Makan", 1);
        goalCount[3] = PlayerPrefs.GetInt("Sapu", 1);
        goalCount[4] = PlayerPrefs.GetInt("Semprot", 1);

        for (int i = 0; i < goalCount.Length; i++)
        {
            goals[i].SetActive(intToBool[goalCount[i]]);
        }

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyGoal(GameObject go)
    {
        // switch (go.name)
        // {
        //     case "Buku":
        //         audio.clip = clip[0];
        //         break;
        //     case "Sabun":
        //         audio.time = 60f;
        //         audio.clip = clip[1];
        //         break;
        //     case "Makan":
        //         audio.clip = clip[2];
        //         break;
        //     case "Sapu":
        //         audio.clip = clip[3];
        //         break;
        //     case "Semprot":
        //         audio.clip = clip[4];
        //         break;
        //     default:
        //         break;
        // }

        // audio.loop = true;
        // audio.volume = 0.25f;
        // audio.time = 0.0f;
        // audio.Play();
        Destroy(go.GetComponent<Collider2D>());

        Destroy(go);
        PlayerPrefs.SetInt(go.name, 0);

        // StartCoroutine(finishDestroy(go));
    }

    IEnumerator finishDestroy(GameObject go)
    {
        yield return new WaitForSeconds(1.5f);
        // audio.Stop();
        Destroy(go);
        PlayerPrefs.SetInt(go.name, 0);
    }

    public void ResetGoal()
    {
        PlayerPrefs.DeleteKey("Buku");
        PlayerPrefs.DeleteKey("Sabun");
        PlayerPrefs.DeleteKey("Sapu");
        PlayerPrefs.DeleteKey("Semprot");
        PlayerPrefs.DeleteKey("Makan");
    }
}
