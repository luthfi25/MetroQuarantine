using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public Sprite FaceDown;
    public Sprite FaceUp;
    public Sprite FaceLeft;
    public Sprite FaceRight;
    private Sprite currentSprite;

    readonly string[] ORIENTATION = {"down", "up", "left", "right", "halt"};
    string curOrientation;
    float speedHorizon;
    float speedVertical;
    Vector3 moveDir;
    [SerializeField] private float SPEED = 0.75f;

    float bumpTime;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody2D;

    [SerializeField] private float suddenTurnRate = 0f;
    [SerializeField] private string name = "";

    private bool isFreeze = false;
    // Start is called before the first frame update
    void Start()
    {
        initOrientation();

        if(suddenTurnRate > 0f){
            InvokeRepeating("SuddenTurn", 5.0f, 5.0f);
        }
    }

    void FixedUpdate()
    {
        if(isFreeze) {
            if(TryGetComponent<Rigidbody2D>(out rigidbody2D)){
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
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

        GameObject avatar = GetAvatar();

        if(avatar.TryGetComponent<SpriteRenderer>(out spriteRenderer) && avatar.TryGetComponent<Animator>(out animator)){
            spriteRenderer.enabled = true;
            animator.enabled = true;

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
                    animator.enabled = false;
                    break;
                default:
                    break;
            }

            if(animator.enabled){
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

            bumpTime = 0.3f;
            ChangeOrientation(bumpOrientation);
            StartCoroutine(ChangeOrientationDelay(0.3f, newOrientation));
        }
    }

    IEnumerator ChangeOrientationDelay(float time, string orientationVal){
        yield return new WaitForSeconds(time);
        ChangeOrientation(orientationVal);
    }

    public void InitAnimator(RuntimeAnimatorController anim){
        GameObject avatar = GetAvatar();
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
        if(randProb < suddenTurnRate){
            return;
        }

        int newOrientationIndex = -1;
        if(name == "Adek"){
            newOrientationIndex = Random.Range(0, ORIENTATION.Length+1);
        } else {
            newOrientationIndex = Random.Range(0, ORIENTATION.Length);
        }

        string newOrientation = "";
        if(newOrientationIndex >= ORIENTATION.Length){
            newOrientation = "halt";
        } else {
            newOrientation = ORIENTATION[newOrientationIndex];
        }

        ChangeOrientation(newOrientation);
    }

    public void Reset(){
        isFreeze = false;  
        initOrientation();
    }

    void initOrientation(){
        int initOrientationIndex = Random.Range(0, ORIENTATION.Length-1); //exclude halt
        string initOrientation = ORIENTATION[initOrientationIndex];
        ChangeOrientation(initOrientation);
    }

    public void SetFreeze(bool freezeVal){
        isFreeze = freezeVal;
    }
}
