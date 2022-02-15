using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Popup : MonoBehaviour {
    
    public bool shown = false;
    public GameObject panel;
    public Text title;
    public PopupValue[] values;
    public UnityEvent onEdit;

    void Start() {
        for(int i = 0; i < values.Length; i++) {
            values[i].Hide();
            values[i].popup = this;
        }
    }

    void Update() {
        panel.SetActive(shown);
    }

    /// <summary>Showes the popup</summary>
    /// <param name="title">The displayed title of the popup. Should be localized if not name</param>
    public void PopUp(string title) {
        shown = true;
        this.title.text = title;
    }

    /// <summary>Resets and hide the popup</summary>
    public void Reset() {
        Hide();
        for(int i = 0; i < values.Length; i++) {
            values[i].Hide();
        }
    }

    /// <summary>Hides the popup</summary>
    public void Hide() {
        shown = false;
    }

    /// <summary>Addes a displayed value to the popup of type <c>STRING</c></summary>
    /// <param name="key">The identification key for the value</param>
    /// <param name="name">The displayed name of the value. Should be localized</param>
    /// <param name="value">The string value to be displayed</param>
    /// <param name="editable">Whether the value should be edtible by the User</param>
    public void AddValue(string key, string name, string value, bool editable) {
        for(int i = 0; i < values.Length; i++) {
            if(!values[i].shown) {
                values[i].SetValue(key, name, value, editable, ParamType.STRING);
                return;
            }
        }
    }
    /// <summary>Addes a displayed value to the popup of type <c>BOOL</c></summary>
    /// <param name="key">The identification key for the value</param>
    /// <param name="name">The displayed name of the value. Should be localized</param>
    /// <param name="value">The boolean value to be displayed</param>
    public void AddValue(string key, string name, bool value) {
        for(int i = 0; i < values.Length; i++) {
            if(!values[i].shown) {
                values[i].SetValue(key, name, value);
                return;
            }
        }
    }
    /// <summary>Addes a displayed value to the popup of type <c>FLOAT</c></summary>
    /// <param name="key">The identification key for the value</param>
    /// <param name="name">The displayed name of the value. Should be localized</param>
    /// <param name="value">The float value to be displayed</param>
    /// <param name="editable">Whether the value should be edtible by the User</param>
    public void AddValue(string key, string name, float value, bool editable) {
        for(int i = 0; i < values.Length; i++) {
            if(!values[i].shown) {
                values[i].SetValue(key, name, value, ParamType.FLOAT);
                return;
            }
        }
    }
    /// <summary>Addes a displayed value to the popup of type <c>INT</c></summary>
    /// <param name="key">The identification key for the value</param>
    /// <param name="name">The displayed name of the value. Should be localized</param>
    /// <param name="value">The integer value to be displayed</param>
    /// <param name="editable">Whether the value should be edtible by the User</param>
    public void AddValue(string key, string name, int value, bool editable) {
        for(int i = 0; i < values.Length; i++) {
            if(!values[i].shown) {
                values[i].SetValue(key, name, value, ParamType.INT);
                return;
            }
        }
    }

    /// <summary>Sets the string value of the PopupValue with key</summary>
    /// <param name="key">The key of the PopupValue to update</param>
    /// <param name="value">The new string value</param>
    public void SetValue(string key, string value) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(key)) {
                values[i].SetValue(value);
                return;
            }
        }
    }
    /// <summary>Sets the boolean value of the PopupValue with key</summary>
    /// <param name="key">The key of the PopupValue to update</param>
    /// <param name="value">The new boolean value</param>
    public void SetValue(string name, bool value) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(name)) {
                values[i].SetValue(value);
                return;
            }
        }
    }
    /// <summary>Sets the numeric value of the PopupValue with key</summary>
    /// <param name="key">The key of the PopupValue to update</param>
    /// <param name="value">The new value</param>
    public void SetValue(string name, float value) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(name)) {
                values[i].SetValue(value);
                return;
            }
        }
    }

    /// <summary>Sets the PopupValue with key to have a range</summary>
    /// <param name="key">The key of the PopupValue to make ranged</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum vakue</param>
    public void SetValueRange(string key, float min, float max) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(key)) {
                values[i].SetRange(min, max);
                return;
            }
        }
    }
    /// <summary>Sets the PopupValue with key to have a range</summary>
    /// <param name="key">The key of the PopupValue to make ranged</param>
    /// <param name="range">A Vector 2 repesenting the range. <c>x</c> is minimum, and <c>y</c> is maximum</param>
    public void SetValueRange(string key, Vector2 range) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(key)) {
                values[i].SetRange(range.x, range.y);
                return;
            }
        }
    }

    /// <summary>Makes the PopupValue with key not have a range</summary>
    /// <param name="key">The key of the PopupValue to make not ranged</param>
    public void UnsetValueRange(string key) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(key)) {
                values[i].UnsetRange();
                return;
            }
        }
    }

    /// <summary>Sets the PopupValue with key to be slid or not</summary>
    /// <param name="key">The key of the PopupValue to make slid</param>
    /// <param name="slid">Wether it should be slid or not</param>
    public void SetValueSlid(string key, bool slid) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(key)) {
                values[i].SetSlid(slid);
                return;
            }
        }
    }

    /// <summary>Returns the string value of the PopupValue with key</summary>
    /// <param name="key">The PopupValue to get the value of</param>
    public string GetValue(string key) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(key)) {
                return values[i].GetValue();
            }
        }
        Debug.LogError("Tried to get value \"" + key + "\" on popup, no such value");
        return "";
    }

    /// <summary>Called from PopupValue when it is edited. Invokes <c>onEdit</c></summary>
    public void OnEdit() {
        for(int i = 0; i < values.Length; i++) {
            values[i].Validate();
        }
        onEdit.Invoke();
    }

    /// <summary>Checks it the popup has a PopupValue with key</summary>
    /// <param name="key">The key to check for</param>
    public bool HasValue(string key) {
        for(int i = 0; i < values.Length; i++) {
            if(values[i].key.Equals(key)) {
                return true;
            }
        }
        Debug.LogError("Tried to get value \"" + key + "\" on popup, no such value");
        return false;
    }
}
