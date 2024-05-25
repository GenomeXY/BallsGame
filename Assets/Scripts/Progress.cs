using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    public int Coins;
    public int Level;
    public Color BackgroudColor;
    public bool IsMusicOn;

    public static Progress Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Load();
    }

    public void SetLevel(int level)
    {
        Level = level;
        Save();
    }

    public void AddCoins(int value)
    {
        Coins += value;
        Save();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        SaveSystem.Save(this);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        ProgressData progressData = SaveSystem.Load();
        if (progressData != null)
        {
            Coins = progressData.Coins;
            Level = progressData.Level;

            Color color = new Color();
            color.r = progressData.BackgroudColor[0];
            color.g = progressData.BackgroudColor[1];
            color.b = progressData.BackgroudColor[2];
            BackgroudColor = color;

            IsMusicOn = progressData.IsMusicOn;
        }
        else
        {
            Coins = 0;
            Level = 1;
            BackgroudColor = Color.blue * 0.5f;
            IsMusicOn = true;
        }
    }
    [ContextMenu("DeleteFile")]
    public void DeleteFile()
    {
        SaveSystem.DeleteFile();
    }
}
