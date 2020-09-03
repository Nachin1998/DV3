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

        ammoText.gameObject.SetActive(true);
        totalEnemies.gameObject.SetActive(true);
        waveState.gameObject.SetActive(true);

        aim.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!player.isDead)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        weaponCamera.gameObject.SetActive(false);

        deathMenu.SetActive(true);

        ammoText.gameObject.SetActive(false);
        totalEnemies.gameObject.SetActive(false);
        waveState.gameObject.SetActive(false);

        aim.gameObject.SetActive(false);
    }
}
