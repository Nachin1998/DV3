using UnityEngine;
using TMPro;

public class WavesUI : MonoBehaviour
{
    public WaveManager wm;

    [Space]

    public GameObject enemyAmmount;
    public TextMeshProUGUI waveStateText;
    TextMeshProUGUI enemyAmmountText;

    void Start()
    {
        enemyAmmountText = enemyAmmount.GetComponentInChildren<TextMeshProUGUI>();
        enemyAmmount.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (wm.state == WaveManager.WaveState.ActiveWave)
        {
            waveStateText.gameObject.SetActive(false);
            enemyAmmount.SetActive(true);
            enemyAmmountText.text = "x " + wm.totalEnemies.ToString();
            return;
        }

        if (wm.waveCountdown <= 0)
        {
            waveStateText.text = "Spawning Nightmares...";
        }
        else
        {
            waveStateText.gameObject.SetActive(true);
            enemyAmmount.gameObject.SetActive(false);
            waveStateText.text = "Next wave in: " + wm.waveCountdown.ToString("F2");
        }
    }
}
