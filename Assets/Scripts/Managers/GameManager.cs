using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject playerGO;
    public BaseWeapon bw;
    public WaveManager wm;

    Player player;
    PlayerMovement pm;
    public bool won = false;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = playerGO.GetComponent<Player>();
        pm = playerGO.GetComponent<PlayerMovement>();
    }

    public void SaveWave()
    {
        SaveSystem.SaveWave(wm);
    }
    public void LoadWave()
    {
        WaveData data = SaveSystem.LoadWave();

        wm.waveCountdown = data.waveCountdown;
        wm.currentWave = data.currentWave;
        wm.state = (WaveManager.WaveState)data.currentWave;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(player, pm, bw);
    }

    public void LoadPlayer()
    {
        Pause.gameIsPaused = false;
        PlayerData data = SaveSystem.LoadPlayer();

        Debug.Log(new Vector3(data.position[0], data.position[1], data.position[2]));
        
        player.currentHealth = data.currentHealth;
        playerGO.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        pm.currentSprint = data.currentSprint;

        bw.ammoInWeapon = data.ammoInWeapon;
        bw.maxAmmo = data.maxAmmo;
    }
}
