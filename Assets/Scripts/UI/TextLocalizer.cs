using UnityEngine;
using UnityEngine.UI;

/// <summary>Sets a UI.Text's text to a localized string</summary>
public class TextLocalizer : MonoBehaviour {

    /// <value>The localized string to use</value>
    public LocalString str;
    /// <value>The text field to put the string into</value>
    public Text text;
    
    void Start() {
        text.text = str.ToString();
    }
}
