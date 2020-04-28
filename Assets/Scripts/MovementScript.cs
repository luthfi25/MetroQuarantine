using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    public static float speed = 2.25f;
    Rigidbody2D rb;

    public GameObject healthManager;
    HealthManager healthManagerInstance;

    public GameObject stopWatch;
    StopWatchScript stopWatchInstance;

    public static bool isPaused = false;
    public Animator animator;

    float horizonTrans;
    float vertiTrans;

    AudioSource audioData;
    public AudioClip goalSound;
    public AudioClip doorSound;
    public AudioClip screamSound;

    public GoalScript goalScript;
    bool isInAction;
    Dictionary<string, int> goalToID = new Dictionary<string, int>();
    public GameObject[] actionMiniGames;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        speed = 2.25f;
        rb = GetComponent<Rigidbody2D>();
        healthManagerInstance = healthManager.GetComponent<HealthManager>();
        stopWatchInstance = stopWatch.GetComponent<StopWatchScript>();
        audioData = GetComponent<AudioSource>();
        audioData.loop = false;
        isInAction = false;

        goalToID.Add("Buku", 0);
        goalToID.Add("Sabun", 1);
        goalToID.Add("Makan", 2);
        goalToID.Add("Sapu", 3);
        goalToID.Add("Semprot", 4);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetHorizon(float val)
    {
        horizonTrans = val;
        vertiTrans = 0f;
    }

    public void SetVerti(float val)
    {
        vertiTrans = val;
        horizonTrans = 0f;
    }

    public void ResetMove()
    {
        horizonTrans = 0f;
        vertiTrans = 0f;
    }


    void FixedUpdate()
    {

        if (!isPaused)
        {
            Vector3 moveDir = Vector3.zero;

            // horizonTrans = Input.GetAxis("Horizontal");
            // vertiTrans = Input.GetAxis("Vertical");

            if (isInAction)
            {
                horizonTrans = 0f;
                vertiTrans = 0f;
            }

            if (horizonTrans > 0.5f || horizonTrans < -0.5f)
            {
                horizonTrans = Mathf.Round(horizonTrans);
                moveDir = new Vector3(horizonTrans, 0f).normalized;
            } else if (vertiTrans > 0.5f || vertiTrans < -0.5f)
            {
                vertiTrans = Mathf.Round(vertiTrans);
                moveDir = new Vector3(0f, vertiTrans).normalized;
            }

            animator.SetFloat("speed-horizon", horizonTrans);
            animator.SetFloat("speed-vert", vertiTrans);
            transform.position += moveDir * speed * Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        { 
            if (isPaused || isInAction)
            {
                return;
            }

            audioData.volume = 1f;
            audioData.clip = screamSound;
            audioData.Play();

            animator.SetFloat("speed-horizon", 0f);
            animator.SetFloat("speed-vert", 0f);

            isPaused = true;
            GameObject parent = coll.gameObject.transform.parent.gameObject;
            StartCoroutine(playerDead(0.25f, parent));
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Border")
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = false;
        } else if (coll.gameObject.tag == "Enemy")
        {
            if (isInAction)
            {
                return;
            }

            audioData.volume = 1f;
            audioData.clip = screamSound;
            audioData.Play();

            isPaused = true;
            animator.SetFloat("speed-horizon", 0f);
            animator.SetFloat("speed-vert", 0f);

            StartCoroutine(playerDead(0.0f, coll.gameObject));
        } else if (coll.gameObject.tag == "Furniture")
        {
            int sortingLayer = coll.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sortingOrder = sortingLayer + 1;
        }
    }

    
    IEnumerator playerDead(float waitTime, GameObject enemy)
    {
        yield return new WaitForSeconds(waitTime);

        if (enemy.name == "Babeh")
        {
            stopWatchInstance.AddTime(20);
            healthManagerInstance.DestroyHealth(2);
        }
        else
        {
            stopWatchInstance.AddTime(10);
            healthManagerInstance.DestroyHealth(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            isInAction = true;
            goalScript.DestroyGoal(collision.gameObject);
            audioData.clip = goalSound;
            audioData.Play();

            actionMiniGames[goalToID[collision.gameObject.name]].SetActive(true);
            isPaused = true;

            animator.SetInteger("Action", goalToID[collision.gameObject.name]);
            StartCoroutine(StopActionAnim());

        } else if (collision.gameObject.tag == "Door")
        {
            audioData.clip = doorSound;
            audioData.Play();
            DoorScript ds = collision.gameObject.GetComponent<DoorScript>();
            ds.doAnimateOpen();
        }
    }

    IEnumerator StopActionAnim()
    {
        yield return new WaitForSeconds(3f);
        animator.SetInteger("Action", -1);
        isInAction = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            DoorScript ds = collision.gameObject.GetComponent<DoorScript>();
            ds.doAnimateClose();
        }
    }

    public void TriggerDead()
    {
        animator.SetTrigger("dead");
    }

    public void Unpause()
    {
        isPaused = false;
    }


}
