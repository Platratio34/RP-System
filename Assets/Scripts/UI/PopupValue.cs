using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupValue : MonoBehaviour {

    public bool shown = false;
    public Text nameText;
    public bool editable;
    public InputField input;
    public string oValue;
    public Dropdown trueFalse;
    public ParamType type;
    public Slider slider;
    public Text sliderText;
    private bool slid;
    // private bool controlled;
    private float max;
    private float min;
    public Popup popup;
    public string key;

    void Start() {
        // trueFalse.options = new List<Dropdown.OptionData>();
        Dropdown.OptionData t = new Dropdown.OptionData();
        t.text = LocalStrings.GetLocalString("ui", "true");
        trueFalse.options.Add(t);
        Dropdown.OptionData f = new Dropdown.OptionData();
        f.text = LocalStrings.GetLocalString("ui", "false");
        trueFalse.options.Add(f);
        trueFalse.RefreshShownValue();
    }

    void Update() {
        sliderText.text = slider.value + "";
    }

    /// <summary>Sets the key, displayed name and value, and the type of the PopupValue. Use for string, float, and int</summary>
    /// <param name="key">The key of the PopupValue, used to check what value it is</param>
    /// <param name="name">The displayed name, should be localized</param>
    /// <param name="value">The string value to be displayed</param>
    /// <param name="editable">Should the value be editable by the user</param>
    /// <param name="type">The type of value it is. (STRING,FLOAT,INT)</param>
    public void SetValue(string key, string name, string value, bool editable, ParamType type) {
        this.key = key;
        // controlled = false;
        nameText.gameObject.SetActive(true);
        shown = true;
        this.nameText.text = key;
        input.gameObject.SetActive(true);
        input.interactable = editable;
        input.text = value;
        oValue = value;
        trueFalse.gameObject.SetActive(false);
        this.type = type;
        this.editable = editable;
        slider.gameObject.SetActive(false);
        sliderText.gameObject.SetActive(false);
        slid = false;
    }
    /// <summary>Sets the key, displayed name and value</summary>
    /// <param name="key">The key of the PopupValue, used to check what value it is</param>
    /// <param name="name">The displayed name, should be localized</param>
    /// <param name="value">The boolean value to be displayed</param>
    public void SetValue(string key, string name, bool value) {
        this.key = key;
        // controlled = false;
        nameText.gameObject.SetActive(true);
        shown = true;
        this.nameText.text = name;
        input.gameObject.SetActive(false);
        trueFalse.gameObject.SetActive(true);
        trueFalse.SetValueWithoutNotify(value?0:1);
        type = ParamType.BOOL;
        slider.gameObject.SetActive(false);
        sliderText.gameObject.SetActive(false);
        slid = false;
    }
    /// <summary>Sets the key, displayed name and value, and the type of the PopupValue. Use for float, and int. Use when a slider may be used</summary>
    /// <param name="key">The key of the PopupValue, used to check what value it is</param>
    /// <param name="name">The displayed name, should be localized</param>
    /// <param name="value">The numeric value to be displayed</param>
    /// <param name="type">The type of value it is. (FLOAT,INT)</param>
    public void SetValue(string key, string name, float value, ParamType type) {
        this.key = key;
        // controlled = false;
        nameText.gameObject.SetActive(true);
        shown = true;
        this.nameText.text = name;
        input.gameObject.SetActive(true);
        input.interactable = editable;
        input.text = value + "";
        oValue = value + "";
        trueFalse.gameObject.SetActive(false);
        this.type = type;
        slider.gameObject.SetActive(false);
        sliderText.gameObject.SetActive(false);
        if(type == ParamType.INT) {
            slider.wholeNumbers = true;
        } else {
            slider.wholeNumbers = false;
        }
        slid = false;
    }
    /// <summary>Sets the string value. Used to update the value</summary>
    /// <param name="value">The new value</param>
    public void SetValue(string value) {
        if(!input.isFocused) {
            input.text = value;
        }
    }
    /// <summary>Sets the boolean value. Used to update the value</summary>
    /// <param name="value">The new value</param>
    public void SetValue(bool value) {
        trueFalse.SetValueWithoutNotify(value ? 0 : 1);
    }
    /// <summary>Sets the numeric value. Used to update the value. Use to update slider position</summary>
    /// <param name="value">The new value</param>
    public void SetValue(float value) {
        if(!input.isFocused) {
            if(slid) {
                slider.value = value;
            } else {
                input.text = value + "";
            }
        }
    }

    /// <summary>Returns the current value as a string</summary>
    public string GetValue() {
        if(type != ParamType.BOOL) {
            return input.text;
        } else {
            return GetValueB() + "";
        }
    }
    /// <summary>Returns the current value as a boolean</summary>
    public bool GetValueB() {
        return trueFalse.value == 0 ? true : false;
    }
    /// <summary>Returns the current value as a float if posible</summary>
    public float GetValueF() {
        if(type == ParamType.FLOAT) {
            Validate();
            return slid ? slider.value : float.Parse(input.text);
        } else {
            return 0;
        }
    }
    /// <summary>Returns the current value as a integer if posible</summary>
    public int GetValueI() {
        if(type == ParamType.INT) {
            Validate();
            return slid ? (int)slider.value : int.Parse(input.text);
        } else {
            return 0;
        }
    }

    /// <summary>Returns the displayed name</summary>
    public string GetName() {
        return nameText.text;
    }

    /// <summary>Hides the popup value</summary>
    public void Hide() {
        shown = false;
        // controlled = false;
        nameText.gameObject.SetActive(false);
        input.gameObject.SetActive(false);
        trueFalse.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        sliderText.gameObject.SetActive(false);
        slid = false;
    }

    /// <summary>Validates the input and resets to last known good if invalid. Used for numeric values</summary>
    public void Validate() {
        if(type == ParamType.STRING) {
            return;
        } else if(type == ParamType.BOOL) {
            return;
        } else if(type == ParamType.INT) {
            int i;
            if(int.TryParse(input.text, out i)) {
                // if(controlled) {
                //     i = (int)Mathf.Clamp(i, min, max);
                // }
                // input.text = i + "";
                oValue = input.text;
            } else {
                input.text = oValue;
            }
        } else if(type == ParamType.FLOAT) {
            float i;
            if(float.TryParse(input.text, out i)) {
                // if(controlled) {
                //     i = Mathf.Clamp(i, min, max);
                // }
                // input.text = i + "";
                oValue = input.text;
            } else {
                input.text = oValue;
            }
        }
    }

    /// <summary>Sets the range for the numeric value</summary>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    public void SetRange(float min, float max) {
        // controlled = true;
        slider.minValue = min;
        slider.maxValue = max;
        this.min = min;
        this.max = max;
    }

    /// <summary>Unsets the numeric value</summary>
    public void UnsetRange() {
        // controlled = false;
    }

    /// <summary>Sets whether to use a slider for the numeric value, or text</summary>
    /// <param name="slid">Wether or not to use the silder</param>
    public void SetSlid(bool slid) {
        this.slid = slid;
        if(type == ParamType.FLOAT || type == ParamType.INT) {
            input.gameObject.SetActive(!slid);
            slider.gameObject.SetActive(slid);
            sliderText.gameObject.SetActive(slid);
        }
    }

    /// <summary>Called from input <c>OnEdit</c>, passes up to popup</summary>
    public void OnEdit() {
        if(popup != null) {
            popup.OnEdit();
        }
    }
}
