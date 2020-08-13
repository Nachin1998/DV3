using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0.5f;
    public float weaponRange = 10f;
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
    }

    public void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange, raycastLayer))
        {
            Debug.DrawRay(transform.position, transform.forward * weaponRange, Color.red);
            string layerHitted = LayerMask.LayerToName(hit.transform.gameObject.layer);

            switch (layerHitted)
            {
                case "Enemy":
                    hit.collider.gameObject.GetComponent<Enemy>().health -= damage;
                    Debug.Log(hit.collider.gameObject.GetComponent<Enemy>().health);
                    break;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * weaponRange, Color.green);
        }
        timer = 0f;
    }
}
