using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CharacterMovementv2 : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float movementSpeed;
    [SerializeField] float dafaultMaxSpeed;
    [SerializeField] float crouchMaxSpeed;
    [SerializeField] float sprintMaxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float jumpForce;

    [SerializeField] float jumpAmount;
    float jumpCount;

    [Header("Contisions")]
    bool isGrounded;

    [Header("Parameters")]
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public Transform orientation;
    [SerializeField] public GameObject playerBody;
    [SerializeField] public PhysicMaterial playerPhysicsMaterial;
    [SerializeField] public PhysicMaterial playerPhysicsCrouch;
    
    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    Vector3 moveDirection;
    float horizontalInput;
    float verticalInput;
    Rigidbody rb;
    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpCount = jumpAmount;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f, whatIsGround);
        if (isGrounded){
            jumpCount = jumpAmount;
        }
        MyInput();
        SpeedControl();
        StateHandler();
        
//        SpeedController();
    }
    private void FixedUpdate() {
        MovePlayer();
    }
    void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetButtonDown("Jump") && jumpCount > 0){
        Jump();}
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f){
            rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y * 0.5f,rb.velocity.z);
        }
    }
        private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetButton("Crouch"))
        {
            state = MovementState.crouching;
            movementSpeed = crouchMaxSpeed;
        }

        // Mode - Sprinting
        else if(isGrounded && Input.GetAxis("Accelerate") >= 0.5f)
        {
            state = MovementState.sprinting;
            movementSpeed = sprintMaxSpeed;
        }

        // Mode - Walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            movementSpeed = dafaultMaxSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }
    void MovePlayer(){
        // Movement Direction
        moveDirection = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");
        if(OnSlope()){
            rb.AddForce(GetSlopeMoveDirection() * acceleration * 20f, ForceMode.Force);
        }

        else if(isGrounded){
            rb.AddForce(moveDirection.normalized * acceleration * 10f, ForceMode.Force);
        }

        else if(!isGrounded){
            rb.AddForce(moveDirection.normalized * acceleration * 10f * 2 ,ForceMode.Force);
        }
        Vector3 PlayerSpeed = new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z);
        
        print(PlayerSpeed.magnitude);

        if(Input.GetButton("Crouch")){
            print("Kucanie");
            playerBody.GetComponent<BoxCollider>().material = playerPhysicsCrouch;
        }else{
            playerBody.GetComponent<BoxCollider>().material = playerPhysicsMaterial;
        }
    }
    private void SpeedControl(){

        Vector3 flatVel = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        if (flatVel.magnitude > movementSpeed){
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x , rb.velocity.y, limitedVel.z);
        }
    }
    public void Jump(){
        print("player jumped");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForce, rb.velocity.z);
        jumpCount--;
    }
    
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
