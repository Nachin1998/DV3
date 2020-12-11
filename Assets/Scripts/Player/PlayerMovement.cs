using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 movement;

    [HideInInspector]public float maxWalkingSpeed;
    [HideInInspector]public float currentSprint;

    [HideInInspector]public bool isOnGround { get { return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); } }
    [HideInInspector] public bool isWalking { get { return (movement.x != 0 || movement.z != 0); } }
    [HideInInspector]public bool canSprint = true;
    [HideInInspector]public bool isSprinting { get { return isWalking && canSprint && Input.GetKey(KeyCode.LeftShift); } }

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

        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");

        SetWeaponAnim("isSprinting", isSprinting);

        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (currentSprint > sprintMaxAmmount)
        {
            currentSprint = sprintMaxAmmount;
        }

        SetWeaponAnim("isWalking", isWalking);

        Vector3 move = transform.right * movement.x + transform.forward * movement.z;
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
        
        StartCoroutine(Sprint());

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
        if (isSprinting)
        {
            SetWeaponAnim("startedSprinting", true);
           
            yield return new WaitForSeconds(0.3f);

            walkingSpeed = sprintSpeed;
            currentSprint -= sprintUsePerSec * Time.deltaTime;
            if (currentSprint <= 0)
            {
                canSprint = false;
            }
        }
        else
        {
            walkingSpeed = maxWalkingSpeed;
            if (currentSprint < sprintMaxAmmount)
            {
                currentSprint += sprintRefilPerSec * Time.deltaTime;
            }
            if (currentSprint >= 25f)
            {
                canSprint = true;
            }

            SetWeaponAnim("startedSprinting", false);
        }
    }

    void SetWeaponAnim(string animParameter, bool state)
    {
        for (int i = 0; i < weaponAnim.Count; i++)
        {
            if (weaponAnim[i].isActiveAndEnabled)
            {
                weaponAnim[i].SetBool(animParameter, state);
            }
        }
    }
}
