using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Transform playerCamera;
    public Animator reloadAnimation;
    public bool isSemiautomatic = false;
    public int fullAmmo = 30;
    public int ammoInWeapon;
    public int maxAmmo = 240;
    public float fireRate = 0.1f;
    public float range = 30f;
    public float damage = 10f;
    public TextMeshProUGUI ammoText;

    public LayerMask raycastLayer; 
    bool isShooting = false;
    bool isReloading = false;
    bool isClipEmpty = false;
    bool canReload = false;
    float timer = 0;

    private void Start()
    {
        reloadAnimation = GetComponent<Animator>();
        ammoInWeapon = fullAmmo;
    }
    void Update()
    {
        if(maxAmmo == 0)
        {
            isClipEmpty = true;
        }
        else
        {
            isClipEmpty = false;
        }

        if (ammoInWeapon == fullAmmo)
        {
            canReload = false;
        }
        else
        {
            canReload = true;
        }

        if(!isReloading && ammoInWeapon !=0)
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
                        isShooting = true;
                    }
                    else
                    {
                        isShooting = false;
                    }

                    if (isShooting)
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(!isReloading && !isClipEmpty && canReload)
            {
                StartCoroutine(Reload());
            }
        }

        ammoText.text = ammoInWeapon.ToString() + " / " + maxAmmo.ToString();
    }

    IEnumerator Reload()
    {
        isReloading = true;

        reloadAnimation.Play("Reloading");
        int ammoToLoad = fullAmmo - ammoInWeapon;
        
        yield return new WaitForSeconds(3);

        if (maxAmmo > ammoToLoad)
        {
            ammoInWeapon += ammoToLoad;
            maxAmmo -= ammoToLoad;            
        }
        else
        {
            ammoInWeapon += maxAmmo;
            maxAmmo = 0;
        }

        isReloading = false;
    }

    public void Fire()
    {
        RaycastHit hit;
        ammoInWeapon--;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range, raycastLayer))
        {
            Debug.DrawRay(playerCamera.position, playerCamera.forward * hit.distance, Color.red);
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
            Debug.DrawRay(playerCamera.position, playerCamera.forward * range, Color.green);
        }
        timer = 0f;
    }
}
