using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum PickUpType
    {
        HealthPickUp,
        AmmoPickUp
    }
    public PickUpType pickUpType;

    public GameObject interactableQuad;
    public float healthRecovery;
    public int ammoRecovery;

    Player player;
    BaseWeapon bw;
    GameObject auxUI;

    private void Start()
    {
        if(interactableQuad != null)
        {
            auxUI = Instantiate(interactableQuad, transform.position, Quaternion.Euler(90, 0, 0), transform.parent);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            switch (pickUpType)
            {
                case PickUpType.HealthPickUp:
                    player = col.GetComponent<Player>();
                    if (player.currentHealth == player.maxHealth)
                    {
                        return;
                    }

                    if (player.currentHealth + healthRecovery > player.maxHealth)
                    {
                        float newHealthRecovery = player.maxHealth - player.currentHealth;
                        player.currentHealth += newHealthRecovery;
                    }
                    else
                    {
                        player.currentHealth += healthRecovery;
                    }
                    AkSoundEngine.PostEvent("player_sandwich", gameObject);
                    if (auxUI != null)
                    {
                        Destroy(auxUI);
                    }
                    Destroy(gameObject);
                    break;

                case PickUpType.AmmoPickUp:
                    bw = col.GetComponentInChildren<BaseWeapon>();
                    if (bw != null)
                    {
                        if (bw.maxAmmo == bw.maxAmmoCap)
                        {
                            return;
                        }

                        if (bw.maxAmmo + ammoRecovery > bw.maxAmmoCap)
                        {
                            int newAmmoRecovery = bw.maxAmmoCap - bw.maxAmmo;
                            bw.maxAmmo += newAmmoRecovery;
                        }
                        else
                        {
                            bw.maxAmmo += ammoRecovery;
                        }

                        AkSoundEngine.PostEvent("player_bullets", gameObject);
                        if(auxUI != null)
                        {
                            Destroy(auxUI);
                        }
                        Destroy(gameObject);
                    }

                    break;
            }
        }
    }
}
