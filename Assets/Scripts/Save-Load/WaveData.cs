[System.Serializable]
public class WaveData
{
    public int currentWave;
    public int waveState;
    public float waveCountdown;

    public WaveData(WaveManager wm)
    {
        currentWave = wm.currentWave;
        waveState = (int)wm.state;
        waveCountdown = wm.waveCountdown;
    }
}
