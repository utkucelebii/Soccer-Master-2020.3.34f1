using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputManager inputManager;
    private Rigidbody rb;
    private CharacterController cc;
    private Animator anim;

    //Character Controller
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    public float speed = 6f;
    public float turnSmoothTime = .1f;
    float turnSmoothVelocity;

    //Ball
    public GameObject Ball;
    private Rigidbody ballRB;
    private bool ballOnFoot = false, canHoldBall = true;
    public float shootPower = 10f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        anim = GetComponent<Animator>();
        ballRB = Ball.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        setGravity();

        if (LevelManager.Instance.isGameActive)
        {
            if (ballOnFoot == true)
            {
                Ball.transform.parent = transform;
                ballRB.velocity = Vector3.zero;
            }

            if (canHoldBall == true && ballOnFoot == false && Vector3.Distance(transform.position, Ball.transform.position) <= 1.5f)
                ballOnFoot = true;

            if (!inputManager.isSwipe)
            {
                if (anim.GetBool("Run"))
                    anim.SetBool("Run", false);
            }

            if (inputManager.isThrow)
            {
                inputManager.isThrow = false;
                anim.SetTrigger("Shoot");
                ballOnFoot = false;
                Ball.transform.parent = null;
                ShootBall(inputManager.direction);
                StartCoroutine("HoldBall");
            }
            else if (inputManager.isSwipe && !inputManager.isThrow)
            {
                if (!anim.GetBool("Run"))
                    anim.SetBool("Run", true);

                Movement(inputManager.direction);
            }
        }


    }

    private void ShootBall(Vector3 direction)
    {
        if(!LevelManager.Instance.PowerUP)
        {
            ballRB.velocity = (direction.normalized + transform.forward) * shootPower;
        }
        else
        {
            Vector3 shootWithoutFlyIt = (direction.normalized + transform.forward);
            shootWithoutFlyIt.y /= 1.25f;
            ballRB.velocity = shootWithoutFlyIt * shootPower;
        }
    }

    IEnumerator HoldBall()
    {
        canHoldBall = false;
        yield return new WaitForSeconds(1.0f);
        canHoldBall = true;
    }

    private void Movement(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        if (!inputManager.isThrow)
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }
    
    private void setGravity()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "block")
        {
            LevelManager.Instance.isGameActive = false;
            LevelManager.Instance.DeathPanel.SetActive(true);
        }
    }
}
