using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Player body;
    CharacterController controler;

    public float walkingSpeed = 12f;
    public float sprintSpeed = 20f;
    public float sprintMaxAmmount = 100f;
    public float sprintUsePerSec = 10f;
    public float sprintRefilPerSec = 10f;

    public List <Animator> weaponAnim = new List<Animator>();

    public float jumpHeight = 3f;
    public float gravity = 9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [HideInInspector]public float maxWalkingSpeed;
    [HideInInspector]public float currentSprint;
    [HideInInspector]public bool isWalking;
    [HideInInspector]public bool canSprint = true;
    [HideInInspector]public bool isSprinting = false;
    [HideInInspector]public bool isOnGround;

    Vector3 velocity;
    float stepSoundOffset = 1f;
    float movingTimer;

    // Update is called once per frame
    private void Start()
    {
        body = GetComponent<Player>();
        controler = GetComponent<CharacterController>();

        maxWalkingSpeed = walkingSpeed;
        currentSprint = sprintMaxAmmount;

        movingTimer = stepSoundOffset;
    }

    void Update()
    {
        if (body.isDead || GameManager.Instance.won)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        isWalking = (x != 0 || z != 0);
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        for (int i = 0; i < weaponAnim.Count; i++)
        {
            if (weaponAnim[i].isActiveAndEnabled)
            {
                weaponAnim[i].SetBool("isSprinting", isSprinting);
            }
        }

        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (currentSprint > sprintMaxAmmount)
        {
            currentSprint = sprintMaxAmmount;
        }

        for (int i = 0; i < weaponAnim.Count; i++)
        {
            if (weaponAnim[i].isActiveAndEnabled)
            {
                weaponAnim[i].SetBool("isWalking", isWalking);
            }
        }

        Vector3 move = transform.right * x + transform.forward * z;

        StartCoroutine(Sprint());

        controler.Move(move * walkingSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
        }       

        velocity.y -= gravity * Time.deltaTime;

        controler.Move(velocity * Time.deltaTime);

        if (isWalking)
        {
            movingTimer += Time.deltaTime;
            if(movingTimer >= stepSoundOffset)
            {
                AkSoundEngine.PostEvent("player_footstep", gameObject);
                movingTimer = 0;
            }            
        }

        if (isSprinting)
        {
            stepSoundOffset = 0.6f;
        }
        else
        {
            stepSoundOffset = 1f;
        }
    }

    public IEnumerator Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isWalking && canSprint)
        {
            for (int i = 0; i < weaponAnim.Count; i++)
            {
                if (weaponAnim[i].isActiveAndEnabled)
                {
                    weaponAnim[i].SetBool("startedSprinting", true);
                }                
            }
           
            yield return new WaitForSeconds(0.3f);

            isSprinting = true;
            walkingSpeed = sprintSpeed;
            currentSprint -= sprintUsePerSec * Time.deltaTime;
            if (currentSprint <= 0)
            {
                canSprint = false;
            }
        }
        else
        {
            isSprinting = false;
            walkingSpeed = maxWalkingSpeed;
            if (currentSprint < sprintMaxAmmount)
            {
                currentSprint += sprintRefilPerSec * Time.deltaTime;
            }
            if (currentSprint >= 25f)
            {
                canSprint = true;
            }

            for (int i = 0; i < weaponAnim.Count; i++)
            {
                if (weaponAnim[i].isActiveAndEnabled)
                {
                    weaponAnim[i].SetBool("startedSprinting", false);
                }
            }
        }
    }
}
