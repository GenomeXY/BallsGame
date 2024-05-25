[System.Serializable]
public class ProgressData 
{
    public int Coins;
    public int Level;
    public float[] BackgroudColor;
    public bool IsMusicOn; 

    public ProgressData(Progress progress)
    {
        Coins = progress.Coins;
        Level = progress.Level; 

        BackgroudColor = new float[3];
        BackgroudColor[0] = progress.BackgroudColor.r;
        BackgroudColor[1] = progress.BackgroudColor.g;
        BackgroudColor[2] = progress.BackgroudColor.b;

        IsMusicOn = progress.IsMusicOn;
    }
}
