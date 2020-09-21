using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Transform playerCamera;
    public ParticleSystem muzzleFlash;
    public Animator animator;
    public AudioSource shotSound;
    public bool isSemiautomatic = false;
    public int ammoInClips = 30;
    public int ammoInWeapon;
    public int maxAmmo = 240;
    public float fireRate = 0.1f;

    public float range = 30f;
    public float damage = 10f;
    public float force = 10f;

    public TextMeshProUGUI ammoText;

    public LayerMask raycastLayer; 

    bool isShooting = false;
    bool isReloading = false;
    bool isOutOfAmmo = false;
    bool canReload = false;
    float timer = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ammoInWeapon = ammoInClips;
        muzzleFlash.Stop();
    }
    void Update()
    {
        if (Pause.gameIsPaused)
        {
            return;
        }

        if (maxAmmo == 0)
        {
            isOutOfAmmo = true;
        }
        else
        {
            isOutOfAmmo = false;
        }

        if (ammoInWeapon == ammoInClips)
        {
            canReload = false;
        }
        else
        {
            canReload = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && !isOutOfAmmo && canReload)
            {
                StartCoroutine(Reload(2.8f));
            }
        }

        ammoText.text = ammoInWeapon.ToString() + " / " + maxAmmo.ToString();

        if (!isReloading && ammoInWeapon != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }

            if (!isSemiautomatic)
            {
                return;
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
    }

    public virtual void Shoot()
    {

    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("isReloading", false);
    }

    IEnumerator Reload(float reloadDuration)
    {
        isReloading = true;

        animator.SetBool("isReloading", true);
        int ammoToLoad = ammoInClips - ammoInWeapon;
        
        yield return new WaitForSeconds(reloadDuration);

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

        animator.SetBool("isReloading", false);
        isReloading = false;
    }

    public void Fire()
    {
        RaycastHit hit;
        ammoInWeapon--;
        muzzleFlash.Play();
        animator.SetBool("isShooting", true);
        shotSound.Play();
        
        
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range, raycastLayer))
        {
            Debug.Log("Hit!");
            Debug.DrawRay(playerCamera.position, playerCamera.forward * hit.distance, Color.red);
            string layerHitted = LayerMask.LayerToName(hit.transform.gameObject.layer);
            Vector3 direction = hit.transform.position - playerCamera.position;
            direction.y = 0;

            switch (layerHitted)
            {
                case "Enemy":
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                    hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * force, ForceMode.Impulse);
                    break;
            }
        }
        else
        {
            Debug.DrawRay(playerCamera.position, playerCamera.forward * range, Color.green);
        }
        timer = 0f;
        StartCoroutine(MuzzleFlash());
    }

    public IEnumerator MuzzleFlash()
    {
        muzzleFlash.Play();
        yield return new WaitForSeconds(muzzleFlash.main.duration * 3);
        animator.SetBool("isShooting", false);
        muzzleFlash.Stop();
    }
}
