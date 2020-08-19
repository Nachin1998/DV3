using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform playerBody;
    public bool isSemiautomatic = false;
    public float fireRate = 0.1f;
    public float weaponRange = 30f;
    public float damage = 10f;

    public LayerMask raycastLayer; 
    bool shooting = false;
    float timer = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (isSemiautomatic)
        {
            if (Input.GetMouseButton(0))
            {
                timer += Time.deltaTime;
                if (timer >= fireRate)
                {
                    shooting = true;
                }
                else
                {
                    shooting = false;
                }

                if (shooting)
                {
                    Fire();
                }
            }
            else
            {
                timer = 0;
            }
        }
    }

    public void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerBody.position, transform.forward, out hit, weaponRange, raycastLayer))
        {
            Debug.DrawRay(playerBody.position, transform.forward * hit.distance, Color.red);
            string layerHitted = LayerMask.LayerToName(hit.transform.gameObject.layer);

            switch (layerHitted)
            {
                case "Enemy":
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                    break;
            }
        }
        else
        {
            Debug.DrawRay(playerBody.position, transform.forward * weaponRange, Color.green);
        }
        timer = 0f;
    }
}
