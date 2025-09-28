using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField][Range(1f, 5f)] private float jumpFallGravityMultiplier;

    [Header("Ground Check Properties")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.05f;
    [SerializeField] private float groundCheckHeight;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float disableGCTime;

    private PlayerActions playerActions;
    private Rigidbody2D rbody;
    private BoxCollider2D collider;
    private Vector2 moveInput;

    private Vector2 boxCenter;
    private Vector2 boxSize;
    private bool jumping;
    private float initialGravityScale;
    private bool groundCheckEnabled = true;
    private WaitForSeconds wait;

    // Called once when the script instance is loaded
    void Awake()
    {
        playerActions = new PlayerActions();
        Debug.Log("PlayerActions initialized: " + (playerActions != null));

        rbody = GetComponent<Rigidbody2D>();
        if ( rbody is null )
        {
            Debug.LogError("Rigidbody2D is NULL!");
        }

        initialGravityScale = rbody.gravityScale;

        collider = GetComponent<BoxCollider2D>();
        if (collider is null)
        {
            Debug.Log("BoxCollider2D is NULL!");
        }

        wait = new WaitForSeconds(disableGCTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start() called: PlayerMovement script is running");
    }

    private void OnEnable()
    {
        Debug.Log("Enabling Player_Map");
        playerActions.Player_Map.Jump.performed += jumpPerformed;
        playerActions.Player_Map.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player_Map.Jump.performed -= jumpPerformed;
        playerActions.Player_Map.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = playerActions.Player_Map.Movement.ReadValue<Vector2>();

        // Deadzone example
        float deadzone = 0.1f;
        float horizontalInput = Mathf.Abs(moveInput.x) < deadzone ? 0f : moveInput.x;

        //movement applied to player Rigidbody2D component
        rbody.velocity = new Vector2(moveInput.x * speed, rbody.velocity.y);

        //gravity
        HandleGravity();
    }

    private void jumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump input received");

        if (!jumping && groundCheckEnabled && IsGrounded())
        {
            Debug.Log("Player is grounded. Jumping now.");

            //force vertical velocity to jump height
            rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
            jumping = true;

            //temporarily disable ground checking to prevent jump spam
            StartCoroutine(EnableGroundCheckAfterJump());
        }
        else
        {
            Debug.Log("Jump input received but player is NOT grounded.");
        }
    }

    private bool IsGrounded()
    {
        var groundBox = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundMask);

        if (groundBox != null)
        {
            Debug.Log("isGrounded is returning true");
            return true;
        }
        Debug.Log("isGrounded is returning false");
        return false;
    }

    private IEnumerator EnableGroundCheckAfterJump()
    {
        groundCheckEnabled = false;
        yield return wait;
        groundCheckEnabled = true;
    }

    private void HandleGravity()
    {
        if(groundCheckEnabled && IsGrounded())
        {
            jumping = false;
        }
        else if (jumping && rbody.linearVelocity.y < 0f) //Jump fall
        {
            rbody.gravityScale = initialGravityScale * jumpFallGravityMultiplier;
        }
        else //Normal fall
        {
            rbody.gravityScale = initialGravityScale;
        }
    }

    private void OnDrawGizmos()
    {
        if (jumping)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
