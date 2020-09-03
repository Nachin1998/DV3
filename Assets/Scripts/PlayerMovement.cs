using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Player body;
    public CharacterController controler;

    public float speed = 12f;
    public float sprintSpeed = 20f;
    public float sprintMaxAmmount = 100f;
    public float currentSprint = 100f;
    public float sprintUsePerSec = 10f;
    public float sprintRefilPerSec = 10f;

    public Image sprintBar;

    public Animator weaponAnim;

    public float jumpHeight = 3f;
    public float gravity = 9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isWalking;
    bool canSprint = true;
    bool isSprinting = false;
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

        sprintBar.fillAmount = currentSprint / 100;

        weaponAnim.SetBool("isSprinting", isSprinting);

        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (currentSprint > sprintMaxAmmount)
        {
            currentSprint = sprintMaxAmmount;
        }

        if (canSprint)
        {
            sprintBar.color = Color.blue;
        }
        else
        {
            sprintBar.color = Color.red;
        }

        weaponAnim.SetBool("isWalking", isWalking);

        Vector3 move = transform.right * x + transform.forward * z;

        StartCoroutine(Sprint());

        controler.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
        }       

        velocity.y -= gravity * Time.deltaTime;

        controler.Move(velocity * Time.deltaTime);
    }

    public IEnumerator Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isWalking && canSprint)
        {
            weaponAnim.SetBool("startedSprinting", true);
            yield return new WaitForSeconds(0.3f);

            isSprinting = true;
            speed = sprintSpeed;
            currentSprint -= sprintUsePerSec * Time.deltaTime;
            if (currentSprint <= 0)
            {
                canSprint = false;
                weaponAnim.SetBool("isSprinting", canSprint);
            }
        }
        else
        {
            isSprinting = false;
            speed = 12f;
            if (currentSprint < sprintMaxAmmount)
            {
                currentSprint += sprintRefilPerSec * Time.deltaTime;
            }
            if (currentSprint >= 25f)
            {
                canSprint = true;
            }
            weaponAnim.SetBool("startedSprinting", false);
        }
    }
}
