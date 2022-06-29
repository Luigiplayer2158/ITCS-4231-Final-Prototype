using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=YHC-6I_LSos This tutorial assisted me with setting up the new input manager, so I could easily use Keyboard and Mouse along with Controller input.
    //https://www.youtube.com/watch?v=we4CGmkPQ6Q using the cinemachine plugin with the new input system
    private PlayerInputActions playerInputActions;
    private InputAction movement;
    public Animator chuck_anims;
    
    public float speed = 20f;
    private Vector3 playerMove;

    public Rigidbody rb;
    //https://docs.unity3d.com/ScriptReference/Rigidbody-velocity.html help with rigidbody velocity
    public MeshRenderer mR;

    public CharacterController controller;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    InputAction jump;
    InputAction boost;

    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public float boostSpeed = 200;
    float targetAngle;

    private Vector3 jumpVelocity;
    private Vector3 boostVelocity;
    private Vector3 wallJump;
    private Vector3 moveDir;
    private Vector3 damageDir;
    private Vector3 lastMove;

    bool jumpAllowed = false;
    bool boostAllowed = false;
    bool grounded = false;
    bool wallJumpAllowed = true;
    bool hasBeenHit = false;

    int wallJumpWait = 0;
    public int health = 3;
    public int collectableNum = 0;
    public int invTimeMax = 60;
    int invTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        mR = GetComponent<MeshRenderer>();
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }



    private void OnEnable()
    {
        movement = playerInputActions.Player.Move;
        movement.Enable();

        jump = playerInputActions.Player.Jump;
        playerInputActions.Player.Jump.Enable();

        boost = playerInputActions.Player.Boost;
        playerInputActions.Player.Boost.Enable();
    }

    //https://answers.unity.com/questions/1642153/how-to-make-a-3d-character-move.html help with setting position
    //https://www.youtube.com/watch?v=4HpC--2iowE help with camera relative movement and rotate towards movement direction
    //https://www.youtube.com/watch?v=7KiK0Aqtmzc better jump assistance
    //https://docs.unity3d.com/ScriptReference/CharacterController.Move.html character controller assistance / jump
    //https://docs.unity3d.com/ScriptReference/CharacterController-isGrounded.html is grounded

    void Update()
    {
        if (!MenuHandler.isPaused)
        {
            playerMove = movement.ReadValue<Vector2>();

            Vector3 direction = new Vector3(playerMove.x, 0f, playerMove.y).normalized;
            grounded = controller.isGrounded;

            if (grounded)
            {
                // Debug.Log("Grounded!");
                jumpVelocity = new Vector3(0f, 0f, 0f);
                jumpAllowed = true;
                boostAllowed = true;
                chuck_anims.SetBool("Boosting", false);
            }
            if (jump.triggered & jumpAllowed)
            {
                chuck_anims.SetBool("Moving", false);
                chuck_anims.SetBool("Jumping", true);
                // Debug.Log("Jump2!!");
                jumpAllowed = false;
                jumpVelocity = new Vector3(0f, 0f, 0f);
                jumpVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }

            if (jumpAllowed == true)
            {
                chuck_anims.SetBool("Jumping", false);
            }

            if (direction.magnitude >= 0.2f)
            {
                if (grounded)
                {
                    chuck_anims.SetBool("Moving", true);
                }
                
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); // rotate towards movement

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            } else
            {
                chuck_anims.SetBool("Moving", false);
            }

            lastMove = moveDir;

            //https://answers.unity.com/questions/1725562/how-to-launch-the-player-into-the-direction-its-fa.html help with boosting forward
            if (boost.triggered & boostAllowed)
            {
                jumpVelocity = new Vector3(0f, 0f, 0f);
                jumpVelocity.y += Mathf.Sqrt(jumpHeight * 0.25f * -3.0f * gravity);
                boostAllowed = false;
                Debug.Log("Boost");
                jumpVelocity += transform.forward * boostSpeed;
                chuck_anims.SetBool("Boosting", true);
            }

            jumpVelocity.y += gravity * Time.deltaTime;
            controller.Move(jumpVelocity * Time.deltaTime);
            
            //Debug.Log(wallJumpWait);
            // Mesh renderer flashing did exist for invuln time, but it does not work the same way for a blender imported model, so it is disabled for now
            if (hasBeenHit == true)
            {
                invTime++;
                if (invTime % 4 == 1)
                {
                  //  mR.enabled = true;
                } else
                {
                  //  mR.enabled = false;
                }
                if (invTime >= invTimeMax)
                {
                    movement.Enable();
                    invTime = 0;
                    hasBeenHit = false;
                  //  mR.enabled = true;
                }
                if (invTime >= 20)
                {
                    movement.Enable();
                }
            }
            //Debug.Log(invTime);
        }
    }

    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.Jump.Disable();
    }
    //https://answers.unity.com/questions/838210/increase-gravity.html help with gravity manipulation
    private void FixedUpdate()
    {
        //Debug.Log("Movement Values " + movement.ReadValue<Vector2>());
        //This code used to prevent fast wall jumps, broke when the game was fully compiled. Not sure why.
        if (wallJumpAllowed == false)
        {
            wallJumpWait++;
            if (wallJumpWait >= 10)
            {
                wallJumpAllowed = true;
                wallJumpWait = 0;
                movement.Enable();
            }
        }
    }
    // https://www.youtube.com/watch?v=EOSjfRuh7x4 wall jump basis
    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        
       if (other.gameObject.tag == "JumpableWall")
        {
            Debug.DrawRay(other.point, other.normal, Color.red, 1.25f);
            if (!grounded & jump.triggered & wallJumpAllowed)
            {
                jumpVelocity.y = 0;
                wallJump = other.normal;
                jumpVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                jumpVelocity.x = wallJump.x * speed;
                jumpVelocity.z = wallJump.z * speed;
                if (jumpVelocity.x == 0 && jumpVelocity.z ==0)
                {
                    movement.Enable();
                } else
                {
                    movement.Disable();
                }
               // controller.Move(wallJump * speed * Time.deltaTime);
                wallJumpAllowed = false;
                Debug.Log(jumpVelocity);
            }
       }

       if (other.gameObject.tag == "Damage")
        {
            damageDir = other.normal;
            Debug.Log(damageDir);
            if (hasBeenHit == false)
            {
               movement.Disable();
               Debug.DrawRay(other.point, other.normal, Color.red, 1.25f);
               jumpVelocity.y += 10;
               jumpVelocity.x = 0;
               jumpVelocity.z = 0;
               jumpVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
               jumpVelocity.x = damageDir.x * speed;
               jumpVelocity.z = damageDir.z * speed;
               health = health - 1;
            }
            hasBeenHit = true;
            Debug.Log("HIT!!!!");
            // Debug.Log(health);
            // Debug.Log(jumpVelocity);

            if (health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
        }

       if (other.gameObject.tag == "Enemy")
        {
            damageDir = other.normal;
            Debug.Log(damageDir);
            if (boostAllowed == false) 
            {
                Destroy(other.gameObject);
                jumpVelocity.y = 0;
                jumpVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            } else
            {
                if (hasBeenHit == false)
                {
                    movement.Disable();
                    Debug.DrawRay(other.point, other.normal, Color.red, 1.25f);
                    jumpVelocity.y = 0;
                    jumpVelocity.x = 0;
                    jumpVelocity.z = 0;
                    jumpVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                    jumpVelocity.x = damageDir.x * speed;
                    jumpVelocity.z = damageDir.z * speed;
                    if (jumpVelocity.x <= 0.01f)
                    {
                        jumpVelocity.x = lastMove.x * speed;
                    }
                    if (jumpVelocity.z <= 0.01f)
                    {
                        jumpVelocity.z = lastMove.z * speed;
                    }
                    health = health - 1;
                }
                hasBeenHit = true;
                Debug.Log("HIT!!!!");
               // Debug.Log(health);
               // Debug.Log(jumpVelocity);
            }

            if (health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
        }
    }
}
