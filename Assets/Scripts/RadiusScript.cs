using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int parentSorting = transform.GetComponentInParent<SpriteRenderer>().sortingOrder;
        GetComponent<SpriteRenderer>().sortingOrder = parentSorting - 1;       
    }
}
