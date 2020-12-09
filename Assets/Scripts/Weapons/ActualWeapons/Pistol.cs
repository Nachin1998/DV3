using UnityEngine;

public class Pistol : BaseWeapon
{
    void Start()
    {
        InitWeapon();        
    }

    void Update()
    {
        if (Pause.gameIsPaused)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isReloading && !isOutOfAmmo)
            {
                Fire();
                AkSoundEngine.PostEvent("player_shoot", gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && !isOutOfClips && canReload)
            {
                AkSoundEngine.PostEvent("player_reload", gameObject);
                StartCoroutine(Reload(3f));
            }
        }
    }
}
