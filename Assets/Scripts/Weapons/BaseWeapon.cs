using System.Collections;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public Transform playerCamera;

    public ParticleSystem muzzleFlash;
    public ParticleSystem enviromentImpactEffect;
    public ParticleSystem enemyImpactEffect;

    public int ammoInClips;
    public int maxAmmo;
    public float fireRate;
    [HideInInspector] public int maxAmmoCap;

    [Space]
    
    public float range;
    public float damage;
    public float force;
    public int ammoInWeapon;

    public LayerMask raycastLayer;

    protected Animator animator;
    
    protected bool isShooting = false;
    protected bool isReloading = false;
    protected bool isOutOfAmmo { get { return ammoInWeapon == 0; } }
    protected bool isOutOfClips { get { return maxAmmo == 0; } }
    protected bool canReload { get { return ammoInWeapon != ammoInClips;  } }

    protected void InitWeapon()
    {
        animator = GetComponent<Animator>();
        
        ammoInWeapon = ammoInClips;
        maxAmmoCap = maxAmmo;
        muzzleFlash.Stop();
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

    public virtual IEnumerator Fire(float duration)
    {
        RaycastHit hit;
        ammoInWeapon--;
        muzzleFlash.Play();
        animator.SetBool("isShooting", true);

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range, raycastLayer))
        {
            Debug.DrawRay(playerCamera.position, playerCamera.forward * hit.distance, Color.red);
            string layerHitted = LayerMask.LayerToName(hit.transform.gameObject.layer);
            Vector3 direction = hit.transform.position - playerCamera.position;
            direction.y = 0;

            switch (layerHitted)
            {
                case "Enemy":
                    hit.collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(damage);

                    AkSoundEngine.PostEvent("bear_impact", gameObject);
                    PlaceImpactEffect(hit, enemyImpactEffect);
                    break;

                case "Tree":
                    AkSoundEngine.PostEvent("shoot_tree", gameObject);
                    PlaceImpactEffect(hit, enviromentImpactEffect);
                    break;

                case "Rock":
                    AkSoundEngine.PostEvent("shoot_rock", gameObject);
                    PlaceImpactEffect(hit, enviromentImpactEffect);
                    break;

                case "Wall":
                    AkSoundEngine.PostEvent("shoot_metal", gameObject);
                    PlaceImpactEffect(hit, enviromentImpactEffect);
                    break;

                default:
                    PlaceImpactEffect(hit, enviromentImpactEffect);
                    break;
            }
           
        }
        else
        {
            Debug.DrawRay(playerCamera.position, playerCamera.forward * range, Color.green);
        }

        muzzleFlash.Play();

        yield return new WaitForSeconds(duration);

        animator.SetBool("isShooting", false);
        muzzleFlash.Stop();
    }

    protected void PlaceImpactEffect(RaycastHit hitPoint, ParticleSystem ps)
    {
        GameObject impactGO = Instantiate(ps.gameObject, hitPoint.point, Quaternion.LookRotation(hitPoint.normal));
        Destroy(impactGO, 2f);
    }

    void OnDisable()
    {
        isReloading = false;
    }
}
