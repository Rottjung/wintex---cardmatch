using System;

[Serializable]
public class SaveFile
{
    public string savefileName;
    public bool isLoaded;
    public SaveData data;
}

[System.Serializable]
public struct SaveData
{
    public int score;
}