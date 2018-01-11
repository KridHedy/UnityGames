using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float velocity = 5f;
    public float turnSpeed = 12f;
    public float jumpPower = 3f;
    public float fallMultiplier = 2.5f;
    public float jumpCoolDown = 1f;
    public LayerMask groundMask;

    Vector2 input;
    float inputDelay = 0.1f;
    float angle;
    float acceleration = 30f;
    float lastJumpTime;
    bool isGrounded = true;

    Rigidbody rBody;
    Quaternion targetRotation;
    Transform cam;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        lastJumpTime = Time.time - jumpCoolDown;
    }

    private void Update()
    {
        GetInput();
        isGrounded = IsOnGround();
        Jump(isGrounded);
        CalculateDirection();
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Input based on Horizontal(a,d,<,>) and Vertical (w,s,^,v) keys
    /// </summary>
    void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// Direction relative to the camera's rotation
    /// </summary>
    void CalculateDirection()
    {
        if (Mathf.Abs(input.x) < inputDelay && Mathf.Abs(input.y) < inputDelay) return;

        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    /// <summary>
    /// Rotate toward the calculated angle
    /// </summary>
    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// This player only moves along its own forward axis
    /// </summary>
    void Move()
    {
        float inputD= Mathf.Abs(input.y) < inputDelay ? input.x : input.y;
        Vector3 desiredVelocity = (Mathf.Sign(inputD) * transform.forward * inputD * velocity) + Vector3.up * rBody.velocity.y;
        rBody.velocity = Vector3.Slerp(rBody.velocity, desiredVelocity, Time.deltaTime * acceleration);
    }

    /// <summary>
    /// Detect ground
    /// </summary>
    bool IsOnGround()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, -Vector3.up, out hit, 1, groundMask);

        //"shoot" a ray from the 1st parameter in the direction of the 2nd parameter and check for maximum the 3rd parameter distance. Objects that are further than the 3rd parameter are not going to be recognized
    }

    /// <summary>
    /// Jump only when we're on ground
    /// </summary>
    void Jump(bool isGrounded)
    {
        if (!isGrounded) {
            if (rBody.velocity.y < 0)
                rBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            else if(rBody.velocity.y > 0)
                rBody.velocity += Vector3.up * Physics.gravity.y * (jumpPower - 1) * Time.deltaTime;
        } else
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastJumpTime + jumpCoolDown) {
            lastJumpTime = Time.time;
            rBody.velocity = Vector3.up * jumpPower * 3f;
        }
    }
}
