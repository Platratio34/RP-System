using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Saves and loads strings to the data folder.
/// </summary>
public static class SaveLoadData {
   
    /// <summary>
    /// The base path for all saving and loading, <c>Assets/[basePath]</c>. Default is <c>"Data/"</c>
    /// </summary>
    public static string basePath = "Data/";

    /// <summary>
    /// Saves <c>[str]</c> to <c>Assets/[basePath]/[filename]</c>.Default for <c> base path</c> is <c>"Data/"</c></summary>
    /// <param name="fileName">The sub folders and file name with extension to save to</param>
    /// <param name="str">The string to save to <c>[filename]</c></param>
    public static bool SaveString(string fileName, string str) {
        string path = Application.dataPath + "/" + basePath + fileName;
        File.WriteAllText(path, str);
        return true;
    }

    /// <summary>
    /// Retune the contended of "<c>Assets/[basePath]/[filename]</c>". Default for <c>base path</c> is "<c>Data/</c>"
    /// </summary>
    /// <param name="fileName">The sub folders and file name with extension to load from</param>
    public static string LoadString(string fileName) {
        if(Exists(fileName)) {
            string path = Application.dataPath + "/" + basePath + fileName;
            return File.ReadAllText(path);
        } else {
            Debug.LogError("File does not exist, can not load");
            return "";
        }
    }

    /// <summary>
    /// Retune true if "<c>Assets/[basePath]/[filename]</c>' exists. Default for <c>base path</c> is "<c>Data/</c>"
    /// </summary>
    /// <param name="fileName">The sub folders and file name with extension to check</param>
    public static bool Exists(string fileName) {
        string path = Application.dataPath + "/" + basePath + fileName;
        return File.Exists(path);
    }
}
