using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    private float runSpeedNow;
    private Vector2 movementInput;
    private BoxCollider2D myFeetCollider;
    [SerializeField] private AudioSource stepAudioSource;
    [SerializeField] private AudioSource jumpAudioSource1;
    [SerializeField] private AudioSource fallAudioSource1;
    // [SerializeField] private AudioClip runOnMetalSFX;


    private bool isGround, isJump;
    
    private bool jumpPressed;
    private int jumpCount;

    private bool jumped;




    void Jump()
    {
        Debug.Log(jumped);
        
        if (myRigidbody2D.velocity.y == 0 && jumped)
        {
            fallAudioSource1.Play();
            jumped = false;
        }
        
        if (isGround)
        {
            jumpCount = 1;

            isJump = false;
            
        }

        if (jumpPressed && isGround)
        {
            isJump = true;
            
            myRigidbody2D.velocity += new Vector2(0f,jumpSpeed);
            jumpAudioSource1.Play();
            jumpCount--;
            jumpPressed = false;
            
            jumped = true;
        }else if (jumpPressed&& jumpCount>0&& isJump)
        {
            myRigidbody2D.velocity += new Vector2(0f,jumpSpeed);
            jumpCount--;
            jumpPressed = false;
            
            jumped = true;
        }
        
    }

    private void FixedUpdate()
    {
        isGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        // isGround = Physics2D.OverlapCircle(groundC)
       

        Jump();
        SwitchAnimation();
    }


    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
        // Debug.Log(moveInput);
    }

    // Start is called before the first frame update
    void Start()
    {
        runSpeedNow = runSpeed;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();

        if (Input.GetButtonDown("Jump") && jumpCount>0)
        {
            jumpPressed = true;
        }
    }

    
    void Run()
    {
        Vector2 playerVelocity = new Vector2(movementInput.x * runSpeedNow,myRigidbody2D.velocity.y);
        // Debug.Log(playerVelocity);
        myRigidbody2D.velocity = playerVelocity;
        
        bool isPlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning",isPlayerHasHorizontalSpeed);

        if (isPlayerHasHorizontalSpeed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if(!stepAudioSource.isPlaying){
                stepAudioSource.Play();
            }
            // AudioSource.PlayClipAtPoint(runOnMetalSFX, Camera.main.transform.position, 0.1f);
            
        }
        else
        {
            stepAudioSource.Stop();
        }
        
    }
    
    private void FlipSprite()
    {
        bool isPlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;

        if (isPlayerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }

    // void OnJump(InputValue value)
    // {
    //     
    //     if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
    //     {
    //         return;
    //     }
    //
    //     if (value.isPressed)
    //     {
    //         
    //
    //     }
    // }

    void SwitchAnimation()
    {
        // myAnimator.SetFloat("isRunning",Math.Abs(myRigidbody2D.velocity.x));
        if (isGround)
        {
            myAnimator.SetBool("falling",false);
        } else if (!isGround&& myRigidbody2D.velocity.y > 0)
        {
            myAnimator.SetBool("isJumping",true);
        }else if (myRigidbody2D.velocity.y < 0)
        {
            myAnimator.SetBool("isJumping",false);
            myAnimator.SetBool("falling",true);
        }
    }
}
