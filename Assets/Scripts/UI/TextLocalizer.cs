using UnityEngine;
using UnityEngine.UI;

/// <summary>Sets a UI.Text's text to a localized string</summary>
public class TextLocalizer : MonoBehaviour {

    /// <summary>The localized string to use</summary>
    public LocalString str;
    /// <summary>The text field to put the string into</summary>
    public Text text;
    
    void Start() {
        text.text = str.ToString();
    }
}
