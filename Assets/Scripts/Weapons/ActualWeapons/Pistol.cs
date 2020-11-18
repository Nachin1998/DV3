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

        UpdateAmmo();
        if (Input.GetMouseButtonDown(0))
        {
            if (!isReloading && !isOutOfAmmo)
            {
                Fire();
                //AkSoundEngine.PostEvent("shoot_tree", gameObject);
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
}
