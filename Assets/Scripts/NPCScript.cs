using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPCScript : MonoBehaviour
{
    public Sprite FaceDown;
    public Sprite FaceUp;
    public Sprite FaceLeft;
    public Sprite FaceRight;
    private Sprite currentSprite;

    readonly string[] ORIENTATION = {"down", "up", "left", "right", "halt"};
    [SerializeField] string curOrientation;
    float speedHorizon;
    float speedVertical;
    Vector3 moveDir;
    [SerializeField] private float SPEED = 0.75f;

    float bumpTime;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody2D;
    GameObject avatar;

    [SerializeField] private float suddenTurnRate = 0f;
    [SerializeField] private string name = "";

    private bool isFreeze = false;

    [SerializeField] private AIPath aIPath;

    // Start is called before the first frame update
    void Start()
    {
        avatar = GetAvatar();

        if(aIPath != null){
            aIPath.enabled = false;
        }

        initOrientation();

        if(suddenTurnRate > 0f){
            InvokeRepeating("SuddenTurn", 5.0f, 5.0f);
        }
    }

    void FixedUpdate()
    {
        if(isFreeze) {
            if(aIPath != null){
                aIPath.enabled = false;
            }

            if(TryGetComponent<Rigidbody2D>(out rigidbody2D)){
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            return;
        }

        if(aIPath != null && aIPath.enabled){            
            if(avatar.TryGetComponent<Animator>(out animator)){
                animator.SetBool("halt", false);
                if(Mathf.Abs(aIPath.desiredVelocity.x) >= Mathf.Abs(aIPath.desiredVelocity.y)){
                    if(aIPath.desiredVelocity.x >= 0f){
                        animator.SetFloat("Speed-Horizon", 1f);
                    } else {
                        animator.SetFloat("Speed-Horizon", -1f);
                    }                 
                    animator.SetFloat("Speed-Vertical", 0f);
                } else {
                    if(aIPath.desiredVelocity.y >= 0f){
                        animator.SetFloat("Speed-Vertical", 1f);
                    } else {
                        animator.SetFloat("Speed-Vertical", -1f);
                    }   
                    animator.SetFloat("Speed-Horizon", 0f);
                }
            }
            return;
        }

        if(bumpTime > 0f){
            transform.position += moveDir * SPEED * 2f * Time.deltaTime;
            bumpTime -= Time.deltaTime;
        } else {
            transform.position += moveDir * SPEED * Time.deltaTime;
        }
    }

    void ChangeOrientation(string orientationVal){
        if(isFreeze){
            return;
        }

        if(avatar.TryGetComponent<SpriteRenderer>(out spriteRenderer) && avatar.TryGetComponent<Animator>(out animator)){
            spriteRenderer.enabled = true;
            animator.enabled = true;
            
            if(aIPath != null){
                aIPath.enabled = false;
            }

            switch(orientationVal){
                case "down":
                    speedHorizon = 0f;
                    speedVertical = -1f;
                    currentSprite = FaceDown;
                    break;
                case "up":
                    speedHorizon = 0f;
                    speedVertical = 1f;
                    currentSprite = FaceUp;
                    break;
                case "left":
                    speedHorizon = -1f;
                    speedVertical = 0f;
                    currentSprite = FaceLeft;
                    break;
                case "right":
                    speedHorizon = 1f;
                    speedVertical = 0f;
                    currentSprite = FaceRight;
                    break;
                case "halt":
                    speedHorizon = 0f;
                    speedVertical = 0f;
                    // spriteRenderer.sprite = currentSprite;
                    if(name == ""){
                        animator.enabled = false;
                    }
                    break;
                case "A*":
                    speedHorizon = 0f;
                    speedVertical = 0f;
                    if(aIPath != null){
                        aIPath.enabled = true;
                    }
                    break;
                default:
                    break;
            }

            if(animator.enabled){
                if(orientationVal == "halt"){
                    animator.SetBool("halt", true);
                } else {
                    animator.SetBool("halt", false);
                }

                animator.SetFloat("Speed-Horizon", speedHorizon);
                animator.SetFloat("Speed-Vertical", speedVertical);
            }

            moveDir = new Vector3(speedHorizon, speedVertical, 0f);
            curOrientation = orientationVal;
        } else {
            Debug.Log("Error accessing avatar.");
        }
    }

    void OnCollisionEnter2D(Collision2D coll){

        if(coll.gameObject.CompareTag("Player")){
            ChangeOrientation("halt");
            isFreeze = true;
        } else if(!coll.gameObject.CompareTag("Door")){
            if(aIPath != null && aIPath.enabled){
                return;
            }

            string bumpOrientation;
            string newOrientation;

            switch(curOrientation){
                case "down":
                    bumpOrientation = "up";
                    newOrientation = "left";
                    break;
                case "up":
                    bumpOrientation = "down";
                    newOrientation = "right";
                    break;
                case "left":
                    bumpOrientation = "right";
                    newOrientation = "up";
                    break;
                case "right":
                    bumpOrientation = "left";
                    newOrientation = "down";
                    break;
                default:
                    bumpOrientation = "left";
                    newOrientation = "down";
                    break;
            }

            bumpTime = 0.15f;
            ChangeOrientation(bumpOrientation);
            StartCoroutine(ChangeOrientationDelay(0.15f, newOrientation));
        }
    }

    IEnumerator ChangeOrientationDelay(float time, string orientationVal){
        yield return new WaitForSeconds(time);
        ChangeOrientation(orientationVal);
    }

    public void InitAnimator(RuntimeAnimatorController anim){
        avatar = GetAvatar();
        if(avatar.TryGetComponent<Animator>(out animator)){
            animator.runtimeAnimatorController = anim;
        } else {
            Debug.Log("error accessing avatar");
        }
    }

    GameObject GetAvatar(){
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.name == "Avatar") {
                return child.gameObject;
            }
        }

        Debug.Log("Can't find avatar.");
        return null;
    }

    void SuddenTurn(){
        if(isFreeze){
            return;
        }

        float randProb = Random.Range(0f, 1f);
        if(!(aIPath != null && aIPath.enabled) && randProb < suddenTurnRate && curOrientation != "halt"){
            return;
        }

        int newOrientationIndex = -1;
        // int newOrientationIndex = Random.Range(0, ORIENTATION.Length-1);
        if(aIPath != null){
            newOrientationIndex = Random.Range(0, ORIENTATION.Length+1);
        } else {
            newOrientationIndex = Random.Range(0, ORIENTATION.Length);
        }

        // string newOrientation = ORIENTATION[newOrientationIndex];
        string newOrientation = "";
        if(newOrientationIndex >= ORIENTATION.Length){
            newOrientation = "A*";
        } else {
            newOrientation = ORIENTATION[newOrientationIndex];
        }
        
        if(TryGetComponent<Rigidbody2D>(out rigidbody2D)){
            rigidbody2D.velocity = Vector3.zero;
        }
        ChangeOrientation(newOrientation);
    }

    public void Reset(){
        isFreeze = false;  
        initOrientation();
    }

    void initOrientation(){
        int initOrientationIndex = Random.Range(0, ORIENTATION.Length-1);
        if(aIPath != null){
            initOrientationIndex = Random.Range(0, ORIENTATION.Length+1);
        }

        string initOrientation = "";
        if(initOrientationIndex >= ORIENTATION.Length){
            initOrientation = "A*";
        } else {
            initOrientation = ORIENTATION[initOrientationIndex];
        }

        ChangeOrientation(initOrientation);
    }

    public void SetFreeze(bool freezeVal){
        isFreeze = freezeVal;
    }
}
