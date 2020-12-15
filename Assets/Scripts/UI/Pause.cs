using UnityEngine;

public class Pause : MonoBehaviour
{
    Player player;
    public GameObject pauseMenu;
    public static bool gameIsPaused = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        if (player.isDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void ResumeGame()
    {
        AkSoundEngine.PostEvent("pause_off", gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void PauseGame()
    {
        AkSoundEngine.PostEvent("pause_on", gameObject);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void StopPauseMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

}