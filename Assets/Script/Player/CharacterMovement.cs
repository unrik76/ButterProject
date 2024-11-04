using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float movementSpeed;
    [SerializeField] float movementAcceleration;
    [SerializeField] float movementThreshold;
    [SerializeField] float movementBoost;
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
    Vector3 moveDirection;
    float horizontalInput;
    float verticalInput;
    Rigidbody rb;


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
    void MovePlayer(){
        moveDirection = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");
        Vector3 PlayerSpeed = new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z);
        print(PlayerSpeed.magnitude);
        if(Input.GetAxis("Accelerate") <= 0.5f){
                SpeedController(1, PlayerSpeed, movementThreshold);
            }else{
                SpeedController(movementBoost, PlayerSpeed, movementThreshold*2);
            }
        if(Input.GetButton("Crouch")){
            print("Kucanie");
            playerBody.GetComponent<BoxCollider>().material = playerPhysicsCrouch;
        }else{
            playerBody.GetComponent<BoxCollider>().material = playerPhysicsMaterial;
        }
    }
    void SpeedController(float speedMult, Vector3 Speed,float max){
        if(Speed.magnitude < max){
            rb.AddForce(moveDirection.normalized * movementAcceleration * 10f * speedMult, ForceMode.Force);
        }     
    }
    public void Jump(){
        print("player jumped");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForce, rb.velocity.z);
        jumpCount--;
    }

}
