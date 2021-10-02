using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    public float jumpHoldForce;
    public float jumpHoldDuration;
    public Transform groundCheckPoint;
    public Transform[] wallCheckPoints;
    public LayerMask groundLayer;
    public LayerMask standableLayer;
    public float groundCheckRadius;
    public float wallCheckRadius;
    public float landingCheckRadius;
    public float turningSpeed;
    public Vector3 maxVelocity;

    [Space]
    public AudioClip jumpClip;

    [SerializeField] bool isOnGround;
    [SerializeField] bool isAgainstWall;
    
    Rigidbody rb;
    Animator animator;
    AudioSource audioSource;
    PlayerInput input;

    bool isJumping;
    bool isLanding;
    float jumpCounter;
    Vector3 targetDirection;

    float lockZ;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();

        targetDirection = transform.forward;
        // lockZ = transform.position.z;

        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }

    void Update()
    {
        GroundAndWallCheck();
        ApplyAnimation();
    }

    private void FixedUpdate() 
    {
        // Horizontal
        float velocityX;
        if (isAgainstWall)
            velocityX = 0;
        else
            velocityX = input.horizontal * speed;
        velocityX = Mathf.Clamp(velocityX, -maxVelocity.x, maxVelocity.x);

        // Vertical
        if (input.jump && isOnGround && !isJumping)
        {
            input.jump = false;
            isJumping = true;
            jumpCounter = Time.time + jumpHoldDuration;

            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
            
            audioSource.clip = jumpClip;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }
        else if (isJumping)
        {
            if (input.jumpHeld)
            {
                rb.AddForce(new Vector3(0, jumpHoldForce, 0), ForceMode.Acceleration);
            }
            if (jumpCounter < Time.time)
                isJumping = false;
        }

        if (!isOnGround)
            input.jump = false;

        var velocityY = Mathf.Clamp(rb.velocity.y, -maxVelocity.y, maxVelocity.y);

        // Apply velocity
        rb.velocity = new Vector3(velocityX, velocityY, 0);
        // transform.position.Set(transform.position.x, transform.position.y, lockZ);

        // Facing direction
        if (input.horizontal > 0)
            targetDirection = Vector3.right;
        else if (input.horizontal < 0)
            targetDirection = Vector3.left;
        if (targetDirection != transform.forward)
        {
            var rot = Quaternion.LookRotation(targetDirection, Vector3.up);
            rot = Quaternion.Slerp(transform.rotation, rot, turningSpeed);
            transform.rotation = rot;
        }

        LandingCheck();

    }

    void ApplyAnimation()
    {
        animator.SetFloat("VelocityX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetBool("IsOnGround", isOnGround);
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsLanding", isLanding);
    }

    void GroundAndWallCheck()
    {
        var cols = Physics.OverlapSphere(groundCheckPoint.position, groundCheckRadius, groundLayer | standableLayer);
        isOnGround = cols.Length != 0;
        isAgainstWall = false;
        foreach (var item in wallCheckPoints)
        {
            cols = Physics.OverlapSphere(item.position, wallCheckRadius, groundLayer);
            if (cols.Length != 0)
            {
                isAgainstWall = true;
                break;
            }
        }
    }

    void LandingCheck()
    {
        if (isOnGround || isJumping)
        {
            isLanding = false;
            return;
        }
        // isLanding = Physics.Raycast(groundCheckPoint.position, Vector3.down, landingCheckRadius, groundLayer);
        isLanding = Physics.OverlapSphere(groundCheckPoint.position, landingCheckRadius, groundLayer | standableLayer).Length != 0;
    }

    public void Teleported(TweenCallback onComplete = null)
    {
        transform.DOScale(Vector3.zero, .5f).OnComplete(() => 
        {
            if (onComplete != null)
                onComplete();
            Destroy(gameObject);
        });
    }

}
