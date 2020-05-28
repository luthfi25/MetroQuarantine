using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
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
    const float SPEED = 0.75f;

    Vector3 bumpDir;
    float bumpTime;

    SpriteRenderer spriteRenderer;
    Animator animator;

    private bool isFreeze = false;
    // Start is called before the first frame update
    void Start()
    {
        int initOrientationIndex = Random.Range(0, ORIENTATION.Length);
        string initOrientation = ORIENTATION[initOrientationIndex];
        ChangeOrientation(initOrientation);
    }

    void FixedUpdate()
    {
        if(isFreeze) {
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
        if(TryGetComponent<SpriteRenderer>(out spriteRenderer) && TryGetComponent<Animator>(out animator)){
            spriteRenderer.enabled = true;
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

            animator.SetFloat("Speed-Horizon", speedHorizon);
            animator.SetFloat("Speed-Vertical", speedVertical);
            moveDir = new Vector3(speedHorizon, speedVertical, 0f);
            orientation = orientationVal;
        }
    }

    void OnCollisionEnter2D(Collision2D coll){

        if(coll.gameObject.CompareTag("Player")){
            ChangeOrientation("halt");
            isFreeze = true;
        } else {
            string bumpOrientation;
            string newOrientation;

            switch(orientation){
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
}
