using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Localized string
/// </summary>
[System.Serializable]
public class LocalString {

    /// <summary>
    /// Localization key
    /// </summary>
    public string key;
    /// <summary>
    /// Localization category
    /// </summary>
    public string catagory;
    /// <summary>
    /// Drop in replacement strings
    /// </summary>
    public string[] dropIns;

    public override bool Equals(object obj) {

        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }

        LocalString ls = (LocalString)obj;
        return ls.key.Equals(key) && ls.catagory.Equals(catagory);

        // return base.Equals (obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    /// <summary>
    /// Returns the localized string from key, category, and drop-ins
    /// </summary>
    /// <returns>Localized string</returns>
    public override string ToString() {
        return LocalStrings.GetLocalStringD(catagory, key, dropIns);
    }
    /// <summary>
    /// Returns a string representation of the object for debug printing
    /// </summary>
    /// <param name="print">Any value</param>
    /// <returns>Un-localized debug string</returns>
    public string ToString(bool print) {
        return "Local String: " + catagory + "." + key;
    }
    
    public JsonObj toJson() {
        JsonObj obj = new JsonObj();
        obj.AddKey("cat", catagory);
        obj.AddKey("key", key);
        JsonObj[] arr = new JsonObj[dropIns.Length];
        for (int i = 0; i < dropIns.Length; i++) {
            arr[i] = new JsonObj(dropIns[i], false);
        }
        obj.AddKey("dropIns", arr);
        return obj;
    }

    public LocalString(JsonObj obj) {
        key = obj.GetString("key");
        catagory = obj.GetString("cat");
        JsonObj[] arr = obj.GetArray("dropIns");
        dropIns = new string[arr.Length];
        for (int i = 0; i < arr.Length; i++) {
            dropIns[i] = arr[i].val;
        }
    }
}
