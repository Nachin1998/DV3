using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum PickUpType
    {
        HealthPickUp,
        AmmoPickUp
    }
    public PickUpType pickUpType;

    public float healthRecovery;
    public int ammoRecovery;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            switch (pickUpType)
            {
                case PickUpType.HealthPickUp:
                    if (col.GetComponent<Player>().currentHealth == col.GetComponent<Player>().maxHealth)
                    {
                        return;
                    }

                    if (col.GetComponent<Player>().currentHealth + healthRecovery > col.GetComponent<Player>().maxHealth)
                    {
                        float newHealthRecovery = col.GetComponent<Player>().maxHealth - col.GetComponent<Player>().currentHealth;
                        col.GetComponent<Player>().currentHealth += newHealthRecovery;
                    }
                    else
                    {
                        col.GetComponent<Player>().currentHealth += healthRecovery;
                    }
                    AkSoundEngine.PostEvent("player_sandwich", gameObject);
                    Destroy(gameObject);
                    break;

                case PickUpType.AmmoPickUp:
                    if (col.GetComponentInChildren<BaseWeapon>() != null)
                    {
                        if (col.GetComponentInChildren<BaseWeapon>().maxAmmo == col.GetComponentInChildren<BaseWeapon>().maxAmmoCap)
                        {
                            return;
                        }

                        if (col.GetComponentInChildren<BaseWeapon>().maxAmmo + ammoRecovery > col.GetComponentInChildren<BaseWeapon>().maxAmmoCap)
                        {
                            int newAmmoRecovery = col.GetComponentInChildren<BaseWeapon>().maxAmmoCap - col.GetComponentInChildren<BaseWeapon>().maxAmmo;
                            col.GetComponentInChildren<BaseWeapon>().maxAmmo += newAmmoRecovery;
                        }
                        else
                        {
                            col.GetComponentInChildren<BaseWeapon>().maxAmmo += ammoRecovery;
                        }
                        AkSoundEngine.PostEvent("player_reload", gameObject);
                        Destroy(gameObject);
                    }

                    break;
            }
        }
    }
}
