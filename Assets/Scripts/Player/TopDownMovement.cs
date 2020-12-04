using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float movementSpeed;
    public float sprintSpeed;
    float auxBaseSpeed;
    bool isWalking { get { return movement.x != 0 || movement.z != 0; } }
    bool isSprinting { get { return Input.GetKey(KeyCode.LeftShift); } }

    Rigidbody rb;
    Vector3 movement;
    Camera cam;
    Vector3 mousePos;

    void Start()
    {
        auxBaseSpeed = movementSpeed;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        mousePos = cam.WorldToScreenPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        transform.position += movement * movementSpeed * Time.deltaTime;

        Vector3 mousePos = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200))
        {
            transform.LookAt(hit.point);
        }

        if (isSprinting)
        {
            movementSpeed = sprintSpeed;
        }
        else
        {
            movementSpeed = auxBaseSpeed;
        }
    }
}
