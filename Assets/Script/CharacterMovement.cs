using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float movementSpeed;
    [SerializeField] float movementSpeedAcceleration;
    [SerializeField] float movementSpeedThreshold;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpMultiplayer;

    bool canJump;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode jumpButton;
    float horizontalInput;
    float verticalInput;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }
    void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
