using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : BaseWeapon
{
    [Range(0f, 0.5f)] public float spread;

    const int maxPellets = 8;
    RaycastHit[] hits;

    float timer = 1;
    void Start()
    {
        InitWeapon();
        hits = new RaycastHit[maxPellets];
        fireRate = 1;
    }

    void Update()
    {
        if (Pause.gameIsPaused)
        {
            return;
        }

        if (timer >= fireRate)
        {
            timer = fireRate;
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (timer >= fireRate)
            {
                if (!isReloading && !isOutOfAmmo)
                {
                    Fire();
                    AkSoundEngine.PostEvent("shotgun_shot", gameObject);
                    timer = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && !isOutOfClips && canReload)
            {
                StartCoroutine(Reload(3.5f));
            }
        }
    }

    public override void Fire()
    {
        ammoInWeapon--;
        muzzleFlash.Play();
        animator.SetBool("isShooting", true);

        for (int i = 0; i < maxPellets; i++)
        {
            Vector3 pelletSpread = new Vector3(Random.Range(-spread, spread), Random.Range(-spread / 2, spread / 2), 0);
            Vector3 rayDirection = playerCamera.forward + pelletSpread;

            if (Physics.Raycast(playerCamera.position, rayDirection, out hits[i], range, raycastLayer))
            {
                Debug.DrawRay(playerCamera.position, rayDirection * hits[i].distance, Color.red);
                string layerHitted = LayerMask.LayerToName(hits[i].transform.gameObject.layer);
                Vector3 pushDirection = hits[i].transform.position - playerCamera.position;
                pushDirection.y = 0;

                switch (layerHitted)
                {
                    case "Enemy":
                        hits[i].collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(damage);
                        PlaceImpactEffect(hits[i], enemyImpactEffect);
                        break;

                    default:
                        PlaceImpactEffect(hits[i], enviromentImpactEffect);
                        break;
                }
            }
            else
            {
                Debug.DrawRay(playerCamera.position, rayDirection * range, Color.green);
            }
        }

        StartCoroutine(MuzzleFlash(1f));
    }
}