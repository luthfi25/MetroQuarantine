using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterScript : MonoBehaviour
{
    public Sprite FaceDown;
    public Sprite FaceUp;
    public Sprite FaceLeft;
    public Sprite FaceRight;
    private Sprite currentSprite;

    readonly string[] ORIENTATION = {"down", "up", "left", "right"};
    string orientation;
    float speedHorizon;
    float speedVertical;
    Vector3 moveDir;
    const float SPEED = 2.25f;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody2D;
    private IGameManager gameManagerScript;

    private Vector3 initPosition;
    private bool isFreeze = false;

    [SerializeField] private string name;


    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();

        GameObject gameManager = GameObject.Find("_GAME MANAGER");
        if(!gameManager.TryGetComponent<IGameManager>(out gameManagerScript)){
            Debug.Log("Can't find IGameManager");
        }
    }

    void FixedUpdate()
    {
        if(isFreeze || orientation == "halt"){
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }

        // transform.position += moveDir * SPEED * Time.deltaTime;
        if(orientation != "halt"){
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2D.velocity = moveDir * SPEED;
        }
    }

    public void ChangeOrientation(string orientationVal){
        if(isFreeze){
            return;
        }

        GameObject avatar = GetAvatar();

        if(avatar.TryGetComponent<SpriteRenderer>(out spriteRenderer) && avatar.TryGetComponent<Animator>(out animator)){
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
                    spriteRenderer.sprite = currentSprite;
                    animator.enabled = false;
                    break;
                case "dead":
                    animator.ResetTrigger("dead");
                    speedHorizon = 0f;
                    speedVertical = 0f;
                    spriteRenderer.sprite = currentSprite;
                    animator.SetTrigger("dead");
                    break;
                default:
                    break;
            }

            if(animator.enabled){
                animator.SetFloat("speed-horizon", speedHorizon);
                animator.SetFloat("speed-vert", speedVertical);
            }
            
            moveDir = new Vector3(speedHorizon, speedVertical, 0f);
            orientation = orientationVal;
        } else {
            Debug.Log("error accessing avatar.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Goal")){
            gameManagerScript.GoalTook(collision.gameObject.name);
            Destroy(collision.gameObject);
        } else if(collision.gameObject.CompareTag("Goal-House")){
            ChangeOrientation("halt");
            gameManagerScript.GoalTook(collision.gameObject.name);
            Destroy(collision.gameObject);
        } else if(collision.gameObject.CompareTag("Goal-Market")){
            gameManagerScript.GoalTook(collision.gameObject.name);
            gameManagerScript.RunCustomFunction("validateGoal/"+collision.gameObject.name);
        } else if(collision.gameObject.CompareTag("Door")){
            gameManagerScript.RunCustomFunction("PlayDoorSound");
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Goal-Market")){
            SpriteRenderer sr = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
            if(sr != null){
                sr.color = Color.white;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.CompareTag("Enemy")){
            ChangeOrientation("dead");

            StartCoroutine(gameManagerScript.PlayerDamage(coll.gameObject.name));
            isFreeze = true;
        }
    }

    public void Reset(){
        transform.position = initPosition;
        currentSprite = FaceDown;
        isFreeze = false;
        ChangeOrientation("halt");
    }

    public void ChangeAsset(RuntimeAnimatorController animatorController, List<Sprite> faces, string name){
        GameObject avatar = GetAvatar();
        if(avatar.TryGetComponent<Animator>(out animator)){
            animator.enabled = true;
            animator.runtimeAnimatorController = animatorController;
        } else {
            Debug.Log("Error accessing avatar.");
        }

        FaceUp = faces[0];
        FaceDown = faces[1];
        FaceLeft = faces[2];
        FaceRight = faces[3];

        currentSprite = FaceDown;
        ChangeOrientation("halt");

        this.name = name;

        if(name == "Anna"){
            transform.localScale = new Vector3(transform.localScale.x * 1.125f,transform.localScale.y * 1.125f, 1f);
        } else if (name == "Fritz"){
            transform.localScale = new Vector3(transform.localScale.x * 1.25f,transform.localScale.y * 1.25f, 1f);
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

    public string GetName(){
        return name;
    }
}
