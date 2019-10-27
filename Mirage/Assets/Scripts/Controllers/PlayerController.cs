using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using config; 

// controls player movement
public class PlayerController : MonoBehaviour
{
    // reference to player object rigid body
    Rigidbody2D myRB;

    // reference to object sprite renderer 
    SpriteRenderer myRenderer;

    // reference to player object animator 
    Animator myAnim;

    // reference to player direction [initially = true]
    bool facingRight = true; 

    // public reference to HORIZONTAL movement variable
    public float maxSpeed;

    // reference to jumping capability
    bool grounded = false;

    [SerializeField] 
    private float groundCheckRadius = 0.3f;

    // reference to the ground layer objects
    public LayerMask groundLayer;

    // reference to the the player root transform to check for grounding
    public Transform groundCheck;

    // reference to player jump power 
    public float jumpPower;

    // to the characters gravity scale 
    public float gravityScale;

    [SerializeField]
    AudioController _playerJumpAC;

    /// configuration variables 

    // reference to the width of the mobile screen
    private float screenWidth;

    void Start()
    {
        screenWidth = Screen.width;

        myRB = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
        GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }

    void Update()
    {

        if (grounded && ((Input.GetAxis("Jump") > 0) || (Input.touchCount > 1 || (Input.touchCount == 1 && Input.GetTouch(0).position.x > 0.3 * screenWidth && Input.GetTouch(0).position.x < 0.7 * screenWidth))) && gravityScale > 0f)
        {
           
            if(_playerJumpAC != null)
                _playerJumpAC.PlaySfx();
            myRB.velocity = new Vector2(myRB.velocity.x, 0f);  // make sure out force is the same each jump
            myRB.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);  // using a force to make our character jump
            grounded = false;
        }
        else if(grounded && ((Input.GetAxis("Jump") > 0) || (Input.touchCount > 1 || (Input.touchCount == 1 && Input.GetTouch(0).position.x > 0.3 * screenWidth && Input.GetTouch(0).position.x < 0.7 * screenWidth))) && gravityScale < 0f)
        {
            if (_playerJumpAC != null)
                _playerJumpAC.PlaySfx();
            myRB.velocity = new Vector2(myRB.velocity.x, 0f);  // make sure out force is the same each jump
            myRB.AddForce(new Vector2(0, -jumpPower), ForceMode2D.Impulse);  // using a force to make our character jump
            grounded = false;
        }

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //draw a circle to check for ground
        
        float move = Input.GetAxis("Horizontal"); // gives access to left, right, a, d keys for movement [-1,0,1]

        if(GameConfiguration.Instance.Platform == "IOS" || GameConfiguration.Instance.Platform == "ANDROID")
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).position.x > 0.7*screenWidth )
                {
                    //move right
                    move = 1;

                }
                else if(Input.GetTouch(0).position.x < 0.3*screenWidth)
                {
                    // move left
                    move = -1;
                }
            }
        }
        
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        myRB.velocity = new Vector2(move * maxSpeed, myRB.velocity.y); // velocity used for immediate speed rather than ramp up speed

        // moveSpeed float used in animation decision  
        if(move > 0 || move < 0)
        {
            myAnim.SetBool("isMoving", true);
        } else
            myAnim.SetBool("isMoving", false);

        myAnim.SetFloat("MovingSpeed", Mathf.Abs(move * maxSpeed));

        if (grounded)
            myAnim.SetBool("isJumping", false);
        else
            myAnim.SetBool("isJumping", true);

    }

    // flips direction of player sprite utilising SpriteRenderer X flip
    void Flip()
    {
        facingRight = !facingRight;
        myRenderer.flipX = !myRenderer.flipX;
    }

}
