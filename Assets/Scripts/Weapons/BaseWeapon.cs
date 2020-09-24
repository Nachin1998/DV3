using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseWeapon : MonoBehaviour
{
    public Transform playerCamera;
    public ParticleSystem muzzleFlash;
    public int ammoInClips;
    public int ammoInWeapon;
    public int maxAmmo;
    public float fireRate;

    public float range;
    public float damage;
    public float force;

    public TextMeshProUGUI ammoText;

    public LayerMask raycastLayer;

    protected Animator animator;
    protected AudioSource shotSound;
    
    protected bool isShooting = false;
    protected bool isReloading = false;
    protected bool isOutOfAmmo = false;
    protected bool isOutOfClips = false;
    protected bool canReload = false;

    protected void InitWeapon()
    {
        animator = GetComponent<Animator>();
        shotSound = GetComponent<AudioSource>();
        
        ammoInWeapon = ammoInClips;
        muzzleFlash.Stop();
    }
    protected void UpdateAmmo()
    {
        if (Pause.gameIsPaused)
        {
            return;
        }

        if (maxAmmo == 0)
        {
            isOutOfClips = true;
        }
        else
        {
            isOutOfClips = false;
        }

        if (ammoInWeapon == ammoInClips)
        {
            canReload = false;
        }
        else
        {
            canReload = true;
        }

        if (ammoInWeapon == 0)
        {
            isOutOfAmmo = true;
        }
        else
        {
            isOutOfAmmo = false;
        }

        ammoText.text = ammoInWeapon.ToString() + " / " + maxAmmo.ToString();
    }

    void OnDisable()
    {
        isReloading = false;
        //animator.SetBool("isReloading", false);
    }

    public virtual IEnumerator Reload(float reloadDuration)
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

    public virtual void Fire()
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
