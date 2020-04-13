using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SortingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] homeBorders = GameObject.FindGameObjectsWithTag("Home Border");
        GameObject[] furnitures = GameObject.FindGameObjectsWithTag("Furniture");
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        GameObject[] objects = players.Concat(enemies).ToArray();
        GameObject[] fullObjects = objects.Concat(homeBorders).ToArray();
        fullObjects = fullObjects.Concat(furnitures).ToArray();
        fullObjects = fullObjects.Concat(doors).ToArray();


        //fullObjects = fullObjects.OrderBy(go => go.transform.position.y).ToArray();
        Array.Sort(fullObjects, CompareYPos); 

        int sortingOrder = fullObjects.Length;
        foreach(GameObject go in fullObjects)
        {
            if(go.transform.name != "Radius")
            {
                SpriteRenderer sr = go.gameObject.GetComponent<SpriteRenderer>();
                sr.sortingOrder = sortingOrder;
                sortingOrder--;
            }

            /*SpriteRenderer sr = go.gameObject.GetComponent<SpriteRenderer>();
            sr.sortingOrder = sortingOrder;
            sortingOrder--;*/
        }

    }

    private int CompareYPos(GameObject a, GameObject b)
    {
        Collider2D ca = a.GetComponent<Collider2D>();
        Collider2D cb = b.GetComponent<Collider2D>();

        float ya = ca.bounds.size.y - ca.offset.y;
        float yb = ca.bounds.size.y - ca.offset.y;

        float totA = ya + a.transform.position.y;
        float totB = yb + b.transform.position.y;

        return Math.Sign(totA - totB);
    }
}
