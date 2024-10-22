using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CharacterMovementV3 : MonoBehaviour
{
    [AddComponentMenu("Smash")]

    [Header("Movement Parameters")]
    //Speed prarameters
    [Tooltip("Maximum speed player can achive while walking")]
        [SerializeField] float player_dafault_speed;

    [Tooltip("Maximum speed player can achive while sprinting")]
        [SerializeField] float player_sprint_speed;

    [Tooltip("Maximum speed player can achive while crouching")]
        [SerializeField] float player_crouch_speed;

    //Acceleration parameters
    [Space(10)]

    [Tooltip("Default player acceleration")]
        [SerializeField] float player_default_acceleration;

    [Tooltip("Detirmins if player sprint acceleration should be used")]
        [SerializeField] bool use_sprint_acceleration;

    [Tooltip("Player sprint acceleration")]
        [SerializeField] float player_sprint_acceleration;


    //Jumping parameters
    [Space(10)]

    [Tooltip("Defines how many jumps player can make")]
        [SerializeField] float player_jump_amount;

    [Tooltip("Defines how much force does a jump get")]
        [SerializeField] float player_jump_force;
    [Tooltip("")]
        [SerializeField] bool is_grounded;
    [Tooltip("")]
        [SerializeField] public LayerMask define_ground;
    [Tooltip("")]
        [SerializeField] public Transform player_orientation;
    [Tooltip("")]
        [SerializeField] public GameObject player_object;
    
    [Space(10)]
        [SerializeField] float maximum_angle_slope;
        [SerializeField] RaycastHit slope_was_hit;

    //Private values
    private float jumpCount = 0;
    private float horizontal_input;
    private float vertical_input;
    private Vector3 moveDirection;

    private float player_top_speed;
    private float player_current_acceleration;
    Vector3 player_current_speed;

    public player_movement_state state;
    public enum player_movement_state{
        walking,
        sprinting,
        crouching,
        air,
    }

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpCount = player_jump_amount;
    }

    // Update is called once per frame
    void Update()
    {
        is_grounded = Physics.Raycast(transform.position, Vector3.down,0.5f,define_ground);
        if (is_grounded){jumpCount = player_jump_amount;}

        speedControl();
        PlayerInput();
        playerStateHandler();

        print(player_current_speed.magnitude);
    }

    private void FixedUpdate() {
        movePlayer();
    }
    public void playerStateHandler(){

        if(Input.GetButton("Crouch"))
        {
            state = player_movement_state.crouching;
            player_top_speed = player_crouch_speed;
            player_current_acceleration = player_default_acceleration;

        }else if(is_grounded && Input.GetAxis("Accelerate") >= 0.5f){
            state = player_movement_state.sprinting;
            player_top_speed = player_sprint_speed;
            player_current_acceleration = player_sprint_acceleration;

        }
        else if(is_grounded)
        {
            state = player_movement_state.walking;
            player_top_speed = player_dafault_speed;
            player_current_acceleration = player_default_acceleration;

        }
        else
        {
            state = player_movement_state.air;
            player_top_speed = player_dafault_speed;
            player_current_acceleration = player_default_acceleration;
            
        }
    }

    void PlayerInput()
    {
        horizontal_input = Input.GetAxisRaw("Horizontal");
        vertical_input = Input.GetAxisRaw("Vertical");

        //Full and half jump mechanic  
        if(Input.GetButtonDown("Jump") && jumpCount > 0){Jump();}
        
        if(Input.GetButtonDown("Jump") && rb.velocity.y > 0f){rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y * 0.5f,rb.velocity.z);}
    }

    void movePlayer()
    {
        moveDirection = player_orientation.forward * vertical_input + player_orientation.right * horizontal_input;
        rb.AddForce(moveDirection.normalized * player_current_acceleration * 10f, ForceMode.Force);

        player_current_speed = new Vector3(rb.velocity.x, 0f,rb.velocity.z);

    }

    private void speedControl()
    {
        if(player_current_speed.magnitude > player_top_speed){
            Vector3 limitingVelocity = player_current_speed.normalized * player_top_speed;
            rb.velocity = new Vector3(limitingVelocity.x , rb.velocity.y, limitingVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + player_jump_force,rb.velocity.z);
        jumpCount--;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slope_was_hit, 0.5f))
        {
             Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * slope_was_hit.distance, Color.yellow);
            float angle = Vector3.Angle(Vector3.up, slope_was_hit.normal);
            return angle < maximum_angle_slope && angle != 0;
        }

        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slope_was_hit.normal).normalized;
    }
}
