using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : BaseWeapon
{
    [Range(0f, 0.5f)] public float spread;

    public float timeToFireAgain = 1;
    const int pellets = 8;
    RaycastHit[] hits;

    float timer = 1;
    void Start()
    {
        InitWeapon();
        hits = new RaycastHit[pellets];
    }

    void Update()
    {
        UpdateAmmo();       

        if(timer >= timeToFireAgain)
        {
            timer = timeToFireAgain;
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(timer >= timeToFireAgain)
            {
                if (!isReloading && !isOutOfAmmo)
                {
                    Fire();
                    timer = 0;
                }
            }            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && !isOutOfClips && canReload)
            {
                StartCoroutine(Reload(2.8f));
            }
        }
    }

    public override void Fire()
    {
        ammoInWeapon--;
        muzzleFlash.Play();
        animator.SetBool("isShooting", true);
        shotSound.Play();

        for (int i = 0; i < pellets; i++)
        {
            Vector3 pelletSpread = new Vector3(Random.Range(-spread, spread), Random.Range(-spread / 2, spread / 2), 0);
            Vector3 rayDirection = playerCamera.forward + pelletSpread;

            if (Physics.Raycast(playerCamera.position, rayDirection, out hits[i], range, raycastLayer))
            {
                Debug.Log("Hit!");
                Debug.DrawRay(playerCamera.position, rayDirection * hits[i].distance, Color.red);
                string layerHitted = LayerMask.LayerToName(hits[i].transform.gameObject.layer);
                Vector3 pushDirection = hits[i].transform.position - playerCamera.position;
                pushDirection.y = 0;               

                switch (layerHitted)
                {
                    case "Enemy":
                        hits[i].collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                        hits[i].collider.gameObject.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * force, ForceMode.Impulse);
                        break;
                }
            }
            else
            {
                Debug.DrawRay(playerCamera.position, rayDirection * range, Color.green);
            }
        }

        StartCoroutine(MuzzleFlash());
    }
}
