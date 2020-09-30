using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SurvivalMode()
    {
        GameManager.Instance.gameMode = GameManager.GameMode.Survival;
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }

    public void HoldZoneMode()
    {
        GameManager.Instance.gameMode = GameManager.GameMode.HoldZone;
        Cursor.visible = false;
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
