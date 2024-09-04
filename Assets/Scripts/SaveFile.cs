using System;

[Serializable]
public class SaveFile
{
    public string savefileName;
    public bool isLoaded;
    public SaveData data;
}

public struct SaveData
{

}