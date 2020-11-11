using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI progressText;

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
        StartCoroutine(LoadAsync(1));
        //SceneManager.LoadSceneAsync(1);
        //SceneManager.LoadScene(1);
    }

    /*public void HoldZoneMode()
    {
        GameManager.Instance.gameMode = GameManager.GameMode.HoldZone;
        Cursor.visible = false;
        SceneManager.LoadScene(2);
    }*/

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressText.text = (int)(progress * 100) + "%";
            slider.value = progress;
            yield return null;
        }
    }
}
