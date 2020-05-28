using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterScript : MonoBehaviour
{
    public Sprite FaceDown;
    public Sprite FaceUp;
    public Sprite FaceLeft;
    public Sprite FaceRight;

    readonly string[] ORIENTATION = {"down", "up", "left", "right"};
    string orientation;
    float speedHorizon;
    float speedVertical;
    Vector3 moveDir;
    const float SPEED = 2.25f;

    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField] GameManagerScript gameManager;

    private Vector3 initPosition;
    private bool isFreeze = false;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    void FixedUpdate()
    {
        if(isFreeze){
            return;
        }

        transform.position += moveDir * SPEED * Time.deltaTime;
    }

    public void ChangeOrientation(string orientationVal){
        if(isFreeze){
            return;
        }

        if(TryGetComponent<SpriteRenderer>(out spriteRenderer) && TryGetComponent<Animator>(out animator)){
            animator.enabled = true;

            switch(orientationVal){
                case "down":
                    speedHorizon = 0f;
                    speedVertical = -1f;
                    spriteRenderer.sprite = FaceDown;
                    break;
                case "up":
                    speedHorizon = 0f;
                    speedVertical = 1f;
                    spriteRenderer.sprite = FaceUp;
                    break;
                case "left":
                    speedHorizon = -1f;
                    speedVertical = 0f;
                    spriteRenderer.sprite = FaceLeft;
                    break;
                case "right":
                    speedHorizon = 1f;
                    speedVertical = 0f;
                    spriteRenderer.sprite = FaceRight;
                    break;
                case "halt":
                    speedHorizon = 0f;
                    speedVertical = 0f;
                    animator.enabled = false;
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Goal")){
            gameManager.GoalTook();
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.CompareTag("Enemy")){
            ChangeOrientation("halt");

            if(TryGetComponent<Animator>(out animator)){
                animator.enabled = true;
                animator.SetTrigger("dead");
            }

            StartCoroutine(gameManager.PlayerDamage());
            isFreeze = true;
        }
    }

    public void Reset(){
        if(TryGetComponent<Animator>(out animator)){
            animator.enabled = true;
            animator.ResetTrigger("dead");
        }

        transform.position = initPosition;
        ChangeOrientation("down");
        ChangeOrientation("halt");
        isFreeze = false;
    }

    public void ChangeAsset(RuntimeAnimatorController animatorController, List<Sprite> faces){
        if(TryGetComponent<Animator>(out animator)){
            animator.runtimeAnimatorController = animatorController;
        }

        FaceUp = faces[0];
        FaceDown = faces[1];
        FaceLeft = faces[2];
        FaceRight = faces[3];

        ChangeOrientation("down");
        ChangeOrientation("halt");
    }
}
