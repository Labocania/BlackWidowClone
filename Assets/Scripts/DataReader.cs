using UnityEngine;

public class DataReader 
{
    static DataReader dataReader;
    private static int waveNumber = 1;
    const int WAVE_LIMIT = 27;

    public static DataReader instance
    {
        get
        {
            if (dataReader == null)
            {
                dataReader = new DataReader();
            }

            return dataReader;
        }
    }

    public static int WaveNumber 
    { 
        get => waveNumber; 

        set
        {
            if (value > WAVE_LIMIT)
            {
                waveNumber = 1;
            }
            else
            {
                waveNumber = value;
            }
        }
    }

    public WaveData LoadWaveData()
    {
        Debug.Log($"Wave number {WaveNumber} loaded.");
        return Resources.Load<WaveData>($"Wave{WaveNumber}");
    }

    public void NextWave()
    {
        if (waveNumber > WAVE_LIMIT)
        {
            waveNumber = 1;
            return;
        }

        WaveNumber++;
    }

}
