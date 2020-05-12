using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusMaskScript : MonoBehaviour
{
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
        if (coll.gameObject.tag == "Enemy" && coll.gameObject.name == "Radius")
        {
            /*Transform mask = coll.gameObject.transform.GetChild(0);
            enterPos = mask.position;
            enterLocalPos = mask.localPosition;
            mask.gameObject.SetActive(true);*/

            // EnemyScript es = coll.gameObject.GetComponentInParent<EnemyScript>();
            // StartCoroutine(doCollide(0.75f, es));

            float thisY = this.gameObject.transform.position.y;
            float radY = coll.gameObject.transform.position.y;

            if(thisY > radY){
                coll.gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            } else {
                coll.gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && coll.gameObject.name == "Radius")
        {
            /*Transform mask = coll.gameObject.transform.GetChild(0);
            enterPos = mask.position;
            enterLocalPos = mask.localPosition;
            mask.gameObject.SetActive(true);*/

            // EnemyScript es = coll.gameObject.GetComponentInParent<EnemyScript>();
            // StartCoroutine(doCollide(0.75f, es));

            float thisY = this.gameObject.transform.position.y;
            float radY = coll.gameObject.transform.position.y;

            if(thisY > radY){
                coll.gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            } else {
                coll.gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && coll.gameObject.name == "Radius")
        {
            /*Transform mask = coll.gameObject.transform.GetChild(0);
            enterPos = mask.position;
            enterLocalPos = mask.localPosition;
            mask.gameObject.SetActive(true);*/

            // EnemyScript es = coll.gameObject.GetComponentInParent<EnemyScript>();
            // StartCoroutine(doCollide(0.75f, es));

            float thisY = this.gameObject.transform.position.y;
            float radY = coll.gameObject.transform.position.y;

            if(thisY > radY){
                coll.gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            } else {
                coll.gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            }
        }
    }
}
