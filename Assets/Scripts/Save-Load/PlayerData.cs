[System.Serializable]
public class PlayerData 
{
    public float currentHealth;
    public float currentSprint;
    public float[] position;

    public int ammoInWeapon;
    public int maxAmmo;

    public PlayerData(Player player, PlayerMovement pm, BaseWeapon bw)
    {
        currentHealth = player.currentHealth;
        currentSprint = pm.currentSprint;

        position = new float[3];
        position[0] = player.gameObject.transform.position.x;
        position[1] = player.gameObject.transform.position.y;
        position[2] = player.gameObject.transform.position.z;

        ammoInWeapon = bw.ammoInWeapon;
        maxAmmo = bw.maxAmmo;
    }
}
