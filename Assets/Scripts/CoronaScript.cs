using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoronaScript : MonoBehaviour
{
    public float speed;
    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(2.5f, 3.5f);
        moveDir = new Vector3(0f, -1f).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        transform.position += moveDir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Corona-Border")){
            Destroy(this.gameObject);
        }
    }
}
