using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject playerGO;
    public BaseWeapon bw;

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

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(player, pm, bw);
    }

    public void LoadPlayer()
    {
        Pause.gameIsPaused = false;
        PlayerData data = SaveSystem.LoadPlayer();

        Debug.Log(new Vector3(data.position[0], data.position[1], data.position[2]));
        Debug.Log(Application.persistentDataPath);
        
        player.currentHealth = data.currentHealth;
        playerGO.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        pm.currentSprint = data.currentSprint;

        bw.ammoInWeapon = data.ammoInWeapon;
        bw.maxAmmo = data.maxAmmo;
    }
}
