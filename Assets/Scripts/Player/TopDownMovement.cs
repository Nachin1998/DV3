using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float walkingSpeed = 12f;
    public float sprintSpeed = 20f;

    [Space]

    public float sprintMaxAmmount = 100f;
    public float sprintUsePerSec = 7.5f;
    public float sprintRefilPerSec = 10f;

    [Space]

    public float yCamOffset;

    float auxBaseSpeed;
    [HideInInspector]public float currentSprint;
    [HideInInspector]public bool canSprint = true;

    bool isWalking { get { return movement.x != 0 || movement.z != 0; } }
    bool isSprinting { get { return Input.GetKey(KeyCode.LeftShift) && canSprint; } }

    Vector3 movement;
    Camera cam;
    Vector3 mousePos;

    void Start()
    {
        auxBaseSpeed = walkingSpeed;
        cam = Camera.main;
        currentSprint = sprintMaxAmmount;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + yCamOffset, transform.position.z);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        transform.position += movement * walkingSpeed * Time.deltaTime;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float length;
        if(plane.Raycast(ray, out length))
        {
            Vector3 pointToLook = ray.GetPoint(length);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        if (currentSprint > sprintMaxAmmount)
        {
            currentSprint = sprintMaxAmmount;
        }

        if (isSprinting)
        {
            walkingSpeed = sprintSpeed;
            currentSprint -= sprintUsePerSec * Time.deltaTime;
        }
        else
        {
            if(currentSprint < sprintMaxAmmount)
            {
                currentSprint += sprintUsePerSec * Time.deltaTime;
            }
            walkingSpeed = auxBaseSpeed;
        }

        if (currentSprint <= 0)
        {
            canSprint = false;
        }

        if (currentSprint >= 25f)
        {
            canSprint = true;
        }
    }
}
