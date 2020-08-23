using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Camera weaponCamera;
    [Space]
    public GameObject playerStateText;
    public GameObject deathMenu;
    [Space]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI totalEnemies;
    public TextMeshProUGUI waveState;
    [Space]
    public Image aim;
    void Start()
    {
        weaponCamera.gameObject.SetActive(true);

        deathMenu.SetActive(false);

        playerStateText.SetActive(false);
        ammoText.gameObject.SetActive(true);
        totalEnemies.gameObject.SetActive(true);
        waveState.gameObject.SetActive(true);

        aim.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isDead)
        {
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            weaponCamera.gameObject.SetActive(false);

            deathMenu.SetActive(true);

            playerStateText.SetActive(true);
            ammoText.gameObject.SetActive(false);
            totalEnemies.gameObject.SetActive(false);
            waveState.gameObject.SetActive(false);

            aim.gameObject.SetActive(false);
        }
    }
}
