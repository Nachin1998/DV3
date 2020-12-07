[System.Serializable]
public class WaveData
{
    public int currentWave;
    public int waveState;

    public WaveData(WaveManager wm)
    {
        currentWave = wm.currentWave;
        waveState = (int)wm.state;
    }
}
