using UnityEngine;
using TMPro;

public class WavesUI : MonoBehaviour
{
    public WaveSpawner ws;

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
        if (ws.state == WaveSpawner.SpawnState.ActiveWave)
        {
            waveStateText.gameObject.SetActive(false);
            enemyAmmount.SetActive(true);
            enemyAmmountText.text = "x " + ws.totalEnemies.ToString();
            return;
        }

        if (ws.waveCountdown <= 0)
        {
            waveStateText.text = "Spawning Nightmares...";
        }
        else
        {
            waveStateText.gameObject.SetActive(true);
            enemyAmmount.gameObject.SetActive(false);
            waveStateText.text = "Next wave in: " + ws.waveCountdown.ToString("F2");
        }
    }
}
