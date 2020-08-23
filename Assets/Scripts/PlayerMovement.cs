using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Player body;
    public CharacterController controler;

    public float speed = 12f;
    public float sprintSpeed = 20f;
    public float sprintMaxAmmount = 100f;
    public float sprintUsePerSec = 10f;
    public float sprintRefilPerSec = 10f;

    public float jumpHeight = 3f;
    public float gravity = 9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isWalking;
    bool canSprint = true;
    bool isOnGround;

    // Update is called once per frame
    void Update()
    {
        if (body.isDead)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        isWalking = (x != 0 || z != 0);
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (sprintMaxAmmount > 100)
        {
            sprintMaxAmmount = 100;
        }
        
        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift) && isWalking && canSprint)
        {
            speed = sprintSpeed;
            sprintMaxAmmount -= sprintUsePerSec * Time.deltaTime;
            if (sprintMaxAmmount <= 0)
            {
                canSprint = false;
            }
        }
        else
        {
            speed = 12f;
            if (sprintMaxAmmount < 100f)
            {
                sprintMaxAmmount += sprintRefilPerSec * Time.deltaTime;
            }
            if (sprintMaxAmmount >= 25f)
            {
                canSprint = true;
            }           
        }

        controler.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
        }       

        velocity.y -= gravity * Time.deltaTime;

        controler.Move(velocity * Time.deltaTime);
    }
}
