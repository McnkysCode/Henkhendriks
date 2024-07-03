using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Assingables
    public Transform playerCam;

    public Transform orientation;

    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;

    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Movement
    public float moveSpeed = 4500;

    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);

    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    //Jumping
    private bool readyToJump = true;

    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //Input
    private float x, y;

    private bool jumping, sprinting, crouching;

    //Sliding
    private Vector3 normalVector = Vector3.up;

    private Vector3 wallNormalVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
    }

    private void Start()
    {
        playerScale = transform.localScale; // Set player scale
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false; // Hide cursor
    }

    private void FixedUpdate()
    {
        Movement(); // Handle movement physics
    }

    private void Update()
    {
        MyInput(); // Get user input
        Look(); // Handle player looking direction
    }

    private void MyInput()
    {
        // Get input axes
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump"); // Check if jump button is pressed
        crouching = Input.GetKey(KeyCode.LeftControl); // Check if crouch button is pressed

        // Check for crouch input
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch(); // Start crouching
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch(); // Stop crouching
    }

    private void StartCrouch()
    {
        // Adjust player scale and position for crouching
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        // Apply slide force if moving and grounded while crouching
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        // Restore player scale and position after crouching
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        // Apply extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        // Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        // Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        // Handle jumping
        if (readyToJump && jumping) Jump();

        float maxSpeed = this.maxSpeed;

        // Apply movement forces
        if (crouching && grounded && readyToJump)
        {
            // Apply downward force for crouching
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        // Clamp input based on maximum speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        float multiplier = 1f, multiplierV = 1f;

        // Apply multiplier for movement depending on grounded and crouching state
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        if (grounded && crouching) multiplierV = 0f;

        // Apply movement forces
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        // Check if grounded and ready to jump
        if (grounded && readyToJump)
        {
            readyToJump = false;

            // Apply upward and forward jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            Vector3 vel = rb.velocity;
            // Adjust velocity for smoother jumping
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown); // Reset jump after cooldown
        }
    }

    private void ResetJump()
    {
        readyToJump = true; // Set ready to jump flag
    }

    private float desiredX;

    private void Look()
    {
        // Get mouse input for looking direction
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0); // Rotate camera vertically
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0); // Rotate player horizontally
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        // Ignore if not grounded or jumping
        if (!grounded || jumping) return;

        // Handle crouching counter movement
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        // Apply counter movement to prevent sliding
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        // Clamp velocity to maximum speed
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        // Calculate velocity relative to player's look direction
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        // Check if given vector represents a floor based on angle
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    private void OnCollisionStay(Collision other)
    {
        // Handle collision with ground
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        // Start grounding delay
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        // Stop grounding after delay
        grounded = false;
    }
}