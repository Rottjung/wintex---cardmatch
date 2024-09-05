using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public static class DeleteSaveFile
{
    [MenuItem("Tools/Delete Save File")] 
    public static void DeleteSaveFromMenu()
    {
        string filePath = SerializationManager<SaveFile>.FILE_PATH;

        if (ConfirmDelete(filePath))
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log("Save file deleted successfully!");
                }
                else
                {
                    Debug.LogError("Save file not found: " + filePath);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error deleting save file: " + e.Message);
            }
        }
    }

    private static bool ConfirmDelete(string filePath)
    {
        return EditorUtility.DisplayDialog("Delete Save File",
            "Are you sure you want to delete the save file at: " + filePath + "?",
            "Yes", "No"); 
    }
}
#endif
