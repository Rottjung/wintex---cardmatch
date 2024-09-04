using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System;

public static class SerializationManager<T> where T : SaveFile, new()
{
    private static T m_SaveFile;
    private static string name = Application.persistentDataPath.TrimEnd(Path.DirectorySeparatorChar);
    public static string FILE_PATH => Path.Combine(Application.persistentDataPath, Path.GetFileName(name) + ".sav");

    public static Action<T> OnGameSaveLoaded;

    public static bool DeleteSave()
    {
        if(File.Exists(FILE_PATH))
        {
            File.Delete(FILE_PATH);
            m_SaveFile = null;
            return true;
        }
        return false;
    }

    public static bool Save(T data)
    {
        m_SaveFile = data;
        
        BinaryFormatter bf = new BinaryFormatter();
        return SerializeData(bf);
    }

   
    private static bool SerializeData(BinaryFormatter bf)
    {
        if (m_SaveFile != default(T))
        {
            using (FileStream stream = new FileStream(FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
				try
				{
					bf.Serialize(stream, m_SaveFile);
					return true;
				}
				catch (SerializationException e)
				{
                    CorruptedSave();
                    Debug.Log("Failed To Save: "+e.ToString());
					return false;
				}
			}
        }
        return false;
    }

    public static bool Load(out T saveFile)
    {
        saveFile = m_SaveFile;
        if (!File.Exists(FILE_PATH))
            return false;
        
        BinaryFormatter bf = new BinaryFormatter();
        if (DeserializeData(bf))
            saveFile = m_SaveFile;
       
        return m_SaveFile != null;
    }


    private static bool DeserializeData(BinaryFormatter bf)
    {
        try
        {
            using (FileStream stream = new FileStream(FILE_PATH, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    stream.Position = 0;
                    m_SaveFile = bf.Deserialize(stream) as T;
                    return true;
                }
                catch (SerializationException)
                {
                    m_SaveFile = null;
                    CorruptedSave();
                    return false;
                }
            }
        }
        catch
        {
            m_SaveFile = null;
            CorruptedSave();
            return false;
        }
    }

    public static void CorruptedSave()
    {

    }
    
    public static byte[] ToBinairy<U>(U file) where U : new()
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, file);
        return ms.ToArray();
    }

    public static byte[] StructToBinairy<U>(U file) where U : struct
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, file);
        return ms.ToArray();
    }

    public static U StructFromBinairy<U>(TextAsset file) where U : struct
    {
        MemoryStream ms = new MemoryStream(file.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        return (U)bf.Deserialize(ms);
    }
}
