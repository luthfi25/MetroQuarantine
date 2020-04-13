using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosScript : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public Camera cameraObj;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        alignPos(x, y, z);
    }

    void alignPos(float x, float y, float z)
    {
        Vector3 desiredPos = cameraObj.ViewportToWorldPoint(new Vector3(x, y, z));
        transform.position = desiredPos;
    }
}
