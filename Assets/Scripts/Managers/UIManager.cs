﻿using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Player player;
    public GameObject weaponHolder;
    public GameObject minimap;
    public GameObject winMenu;
    public GameObject deathMenu;

    void Start()
    {
        weaponHolder.SetActive(true);
        minimap.SetActive(true);
        winMenu.SetActive(false);
        deathMenu.SetActive(false);
    }

    void Update()
    {
        if (player.isDead)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            minimap.SetActive(false);

            weaponHolder.SetActive(false);

            deathMenu.SetActive(true);
        }

        if(GameManager.Instance.won)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            winMenu.SetActive(true);
        }
    }
}