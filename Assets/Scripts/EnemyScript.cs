using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    string mode;
    Animator animator;
    public GameObject vision;
    public string character;

    public GameObject[] spawnPoints;

    float enemySpeed;
    float suddenTurnRate;
    int oldMove;

    Vector3 oldPos;

    Dictionary<String, int> modeToMove = new Dictionary<String, int>();
    Dictionary<int, String> moveToMode = new Dictionary<int, String>();

    float lastCheckTime = 0;
    Vector3 lastCheckPos;
    float xSeconds = 0.25f;
    float yMuch = 0.01f;

    AudioSource audio;
    public AudioClip doorSound;

    // Start is called before the first frame update
    void Start()
    {
        lastCheckPos = transform.position;
        mode = "left";
        calibrateVision(90f);
        animator = GetComponent<Animator>();
        InvokeRepeating("SuddenTurn", 5.0f, 5.0f);

        ResetPosition();
        
        //  { 0, 1, 2, 3, 4}; //up, down, left, right, stop
        modeToMove.Add("up", 0);
        modeToMove.Add("down", 1);
        modeToMove.Add("left", 2);
        modeToMove.Add("right", 3);
        modeToMove.Add("stop", 4);

        moveToMode.Add(0, "up");
        moveToMode.Add(1, "down");
        moveToMode.Add(2, "left");
        moveToMode.Add(3, "right");
        moveToMode.Add(4, "stop");

        audio = GetComponent<AudioSource>();
        audio.loop = false;
    }

    public void ResetPosition(){
        switch (character)
        {
            case ("Emak"):
                mode = "right";
                calibrateVision(180f);
                enemySpeed = MovementScript.speed / 3f;
                suddenTurnRate = 0.5f;
                transform.position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
                break;
            case ("Babeh"):
                mode = "left";
                enemySpeed = MovementScript.speed / 6f;
                suddenTurnRate = 0.5f;
                transform.position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
                break;
            case ("Adek"):
                mode = "down";
                calibrateVision(90f);
                enemySpeed = MovementScript.speed / 1.5f;
                suddenTurnRate = 0.75f;
                transform.position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
                break;
            default:
                // TODO ADEK: movement cepet, lebih sering belok
                enemySpeed = MovementScript.speed / 4f;
                suddenTurnRate = 0.5f;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!MovementScript.isPaused)
        {
            //float horizonTrans = Input.GetAxis("Horizontal");
            //float vertiTrans = Input.GetAxis("Vertical");

            Vector3 moveDir = Vector3.zero;

            if (mode == "up")
            {
                moveDir = new Vector3(0f, 1f).normalized;
            } else if (mode == "right")
            {
                moveDir = new Vector3(1f, 0f).normalized;
            } else if (mode == "down")
            {
                moveDir = new Vector3(0f, -1f).normalized;
            } else if (mode == "left")
            {
                moveDir = new Vector3(-1f, 0f).normalized;
            } else
            {
                // Stop
                moveDir = new Vector3(0f, 0f).normalized;
            }

            animator.SetFloat("speed-horizon", moveDir.x);
            animator.SetFloat("speed-vert", moveDir.y);

            transform.position += moveDir * enemySpeed * Time.deltaTime;
        } else
        {
            CancelInvoke();
            animator.SetFloat("speed-horizon", 0f);
            animator.SetFloat("speed-vert", 0f);
        }

        if((Time.time - lastCheckTime) > xSeconds)
        {
            if((transform.position - lastCheckPos).magnitude < yMuch)
            {
                CollideBorder();
            }

            lastCheckPos = transform.position;
            lastCheckTime = Time.time;
        }
    }

    void calibrateVision(float rotation)
    {
    /*
        Vector3 refPos = transform.position;
        if (mode == "up")
        {
            vision.transform.position = new Vector3(refPos.x,refPos.y + 1.25f);
        }
        else if (mode == "right")
        {
            vision.transform.position = new Vector3(refPos.x + 1f, refPos.y);
        }
        else if (mode == "down")
        {
            vision.transform.position = new Vector3(refPos.x, refPos.y - 1.25f);
        }
        else if (mode == "left")
        {
            // Left
            vision.transform.position = new Vector3(refPos.x - 1f, refPos.y);
        }

        vision.transform.Rotate(0, 0, rotation);
        */

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        /*if(character == "Adek")
        {
            Debug.Log("masup1");
        }

        */
        /*if (coll.gameObject.tag == "Home Border" || coll.gameObject.tag == "Border" || coll.gameObject.tag == "Furniture")
        {
            CollideBorder();
        }*/
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        /*if (character == "Adek")
        {
            Debug.Log("masup2");
        }*/

        /*if (coll.gameObject.tag == "Home Border" || coll.gameObject.tag == "Border" || coll.gameObject.tag == "Furniture")
        {
            CollideBorder();
        }*/
    }

    public void CollideBorder()
    {
        if (mode == "up")
        {
            mode = "right";
        }
        else if (mode == "right")
        {
            mode = "down";
        }
        else if (mode == "down")
        {
            mode = "left";
        }
        else
        {
            // Left
            mode = "up";
        }

        calibrateVision(-90f);
    }

    void SuddenTurn()
    {
        float randProb = UnityEngine.Random.Range(0.0f, 1.0f);
        if (randProb < suddenTurnRate)
        {
            return;
        }

        int[] moves = new int[] { 0, 1, 2, 3, 4}; //up, down, left, right, stop
        float[][] rotationMatrix = new float[][]{new float[] { 0f, 180f, 90f, -90f, 0f }, new float[]{ 180f, 0f, -90f, 90f, 0f } , new float[]{ -90f, 90f, 0f, 180f, 0f }, new float[]{ 90f, -90f, 180f, 0f, 0f }, new float[] { 0f, 0f, 0f, 0f, 0f } };

        int randomMove = moves[UnityEngine.Random.Range(0, moves.Length)];


        if(character == "Adek")
        {
            moves = new int[] { 0, 1, 2, 3, 4, 4};
            randomMove = moves[UnityEngine.Random.Range(0, moves.Length)];
        }

        int curMove = modeToMove[mode];

        if(randomMove == curMove || randomMove == curMove + 1 || randomMove == curMove - 1)
        {
            return;
        }

        mode = moveToMode[randomMove];

        if(curMove == 4)
        {
            curMove = oldMove;
        }

        if (randomMove == 4)
        {
            oldMove = curMove;
        }

        calibrateVision(rotationMatrix[curMove][randomMove]);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            DoorScript ds = collision.gameObject.GetComponent<DoorScript>();
            ds.doAnimateOpen();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            DoorScript ds = collision.gameObject.GetComponent<DoorScript>();
            ds.doAnimateClose();
        }
    }
}
