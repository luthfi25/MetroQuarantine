using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureScript : MonoBehaviour
{
    Vector3 enterPos;
    Vector3 enterLocalPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            /*Transform mask = coll.gameObject.transform.GetChild(0);
            enterPos = mask.position;
            enterLocalPos = mask.localPosition;
            mask.gameObject.SetActive(true);*/

            // EnemyScript es = coll.gameObject.GetComponentInParent<EnemyScript>();
            // StartCoroutine(doCollide(0.75f, es));
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            /*Transform mask = coll.gameObject.transform.GetChild(0);
            mask.position = enterPos;*/

            // EnemyScript es = coll.gameObject.GetComponentInParent<EnemyScript>();
            // StartCoroutine(doCollide(0.0f, es));
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
       if(coll.gameObject.tag == "Enemy")
        {
            /*Transform mask = coll.gameObject.transform.GetChild(0);
            mask.localPosition = enterLocalPos; 
            mask.gameObject.SetActive(false);*/
        } 
    }

    IEnumerator doCollide(float seconds, EnemyScript es)
    {
        yield return new WaitForSeconds(seconds);
        es.CollideBorder();
    }
}
