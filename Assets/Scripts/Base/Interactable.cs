using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Basic intractable object in the game world
/// </summary>
public class Interactable : MonoBehaviour {

    /// <summary>
    /// Replaced by selBoxes
    /// </summary>
    [Obsolete("No longer used, use selBoxes instead")]
    public Vector3 selBoxOffset;
    /// <summary>
    /// Replaced by selBoxes
    /// </summary>
    [Obsolete("No longer used, use selBoxes instead")]
    public Vector3 selBoxScale;
    /// <summary>
    /// Collection of parameters editable by GM
    /// </summary>
    public EditabelParams eParams;
    /// <summary>
    /// Unique ID of the intractable
    /// </summary>
    public string id;
    /// <summary>
    /// Display name of the intractable, can be override by localName
    /// </summary>
    public string dispName;
    /// <summary>
    /// Localized display name of the entity
    /// </summary>
    public LocalString localName;
    /// <summary>
    /// If the localized name should be used for display
    /// </summary>
    public bool isNameKey = false;

    /// <summary>
    /// Array of boxes to use to indicate selection
    /// </summary>
    public SelectionBox[] selBoxes;

    /// <summary>
    /// Called when the the interact key is used on the entity
    /// </summary>
    public void OnInteract(bool gm) {
        OnInteract(gm, 0);
    }
    /// <summary>
    /// Called when the the interact key is used on the entity
    /// </summary>
    public virtual void OnInteract(bool gm, int b) {}
    /// <summary>
    /// Called right after an editable parameter is changed
    /// </summary>
    protected virtual void OnEdit() {}
    /// <summary>
    /// Called when the intractable is saved
    /// </summary>
    public virtual JsonObj Save() {
        return new JsonObj();
    }
    /// <summary>
    /// Called when the intractable is loaded
    /// </summary>
    public virtual void Load(JsonObj obj) {}

    /// <summary>
    /// Sets an editable parameter by name
    /// </summary>
    /// <param name="name">The name of the parameter</param>
    /// <param name="value">Teh new value</param>
    /// <returns>If the parameter was set</returns>
    public bool SetParameter(string name, string value) {
        EditableParam e = eParams.GetParam(name);
        if(e != null) {
            if(e.type == ParamType.STRING) {
                e.valueS = value;
                e.Validate();
                OnEdit();
                return true;
            } else if(e.type == ParamType.FLOAT) {
                e.valueF = float.Parse(value);
                e.Validate();
                OnEdit();
                return true;
            } else if(e.type == ParamType.INT) {
                e.valueI = int.Parse(value);
                e.Validate();
                OnEdit();
                return true;
            } else if(e.type == ParamType.BOOL) {
                e.valueB = bool.Parse(value);
                e.Validate();
                OnEdit();
                return true;
            }
            return false;
        }
        return false;
    }
    /// <summary>
    /// Sets an editable parameter by name
    /// </summary>
    /// <param name="name">The name of the parameter</param>
    /// <param name="value">Teh new value</param>
    /// <returns>If the parameter was set</returns>
    public bool SetParameter(string name, float value) {
        EditableParam e = eParams.GetParam(name);
        if(e != null) {
            if(e.type == ParamType.FLOAT) {
                e.valueF = value;
                OnEdit();
                return true;
            }
            return false;
        }
        return false;
    }
    /// <summary>
    /// Sets an editable parameter by name
    /// </summary>
    /// <param name="name">The name of the parameter</param>
    /// <param name="value">Teh new value</param>
    /// <returns>If the parameter was set</returns>
    public bool SetParameter(string name, int value) {
        EditableParam e = eParams.GetParam(name);
        if(e != null) {
            if(e.type == ParamType.INT) {
                e.valueI = value;
                OnEdit();
                return true;
            }
            return false;
        }
        return false;
    }
    /// <summary>
    /// Sets an editable parameter by name
    /// </summary>
    /// <param name="name">The name of the parameter</param>
    /// <param name="value">The new value</param>
    /// <returns>If the parameter was set</returns>
    public bool SetParameter(string name, bool value) {
        EditableParam e = eParams.GetParam(name);
        if(e != null) {
            if(e.type == ParamType.BOOL) {
                e.valueB = value;
                OnEdit();
                return true;
            }
            return false;
        }
        return false;
    }

    /// <summary>
    /// Gets an array of the editable parameter names
    /// </summary>
    /// <returns></returns>
    public string[] GetParams() {
        string[] s = new string[eParams.parameters.Length];
        for(int i = 0; i < s.Length; i++) {
            s[i] = eParams.parameters[i].name;
        }
        return s;
    }

    /// <summary>
    /// Checks if a parameter with name has a range
    /// </summary>
    /// <param name="name">The parameter name</param>
    /// <returns>If the parameter is ranged, or false if the parameter does not exist</returns>
    public bool IsParameterRanged(string name) {
        return eParams.IsParameterRanged(name);
    }
    /// <summary>
    /// Gets the range of a parameter
    /// </summary>
    /// <param name="name">The parameter name</param>
    /// <returns>The range of the parametr, or 0 if the parameter does not exist</returns>
    public Vector2 GetParamRange(string name) {
        return eParams.GetParamRange(name);
    }

    /// <summary>
    /// If the parameter should have a slider
    /// </summary>
    /// <param name="name"><The parameter name/param>
    /// <returns>If the parameter should have a slider, or false if the parameter does not exist</returns>
    public bool IsParamSlid(string name) {
        return eParams.IsParamSlid(name);
    }

    /// <summary>
    /// Use specific type getter instead. Gets any parameter as a string by name
    /// </summary>
    /// <param name="name">The parameter name</param>
    /// <param name="type">The parameter type</param>
    /// <returns>The value as a string</returns>
    [Obsolete("Use type specific getter instep")]
    public string GetParam(string name, ParamType type) {
        if(type == ParamType.STRING) {
            return GetParamS(name);
        } else if(type == ParamType.FLOAT) {
            return GetParamF(name) + "";
        } else if(type == ParamType.INT) {
            return GetParamI(name) + "";
        } else if(type == ParamType.BOOL) {
            return GetParamB(name) + "";
        }
        return "";
    }
    /// <summary>
    /// Returns the string value of a parameter
    /// </summary>
    /// <param name="name">Parameter name</param>
    /// <returns>The value, or "" if the parameter does not exist, or is not a string</returns>
    public string GetParamS(string name) {
        if(eParams.HasParam(name)) {
            EditableParam e = eParams.GetParam(name);
            if(e.type == ParamType.STRING) {
                return e.valueS;
            }
        }
        return "";
    }
    /// <summary>
    /// Returns the float value of a parameter
    /// </summary>
    /// <param name="name">Parameter name</param>
    /// <returns>The value, or 0f if the parameter does not exist, or is not a float</returns>
    public float GetParamF(string name) {
        if(eParams.HasParam(name)) {
            EditableParam e = eParams.GetParam(name);
            if(e.type == ParamType.FLOAT) {
                return e.valueF;
            }
        }
        return 0f;
    }
    /// <summary>
    /// Returns the integer value of a parameter
    /// </summary>
    /// <param name="name">Parameter name</param>
    /// <returns>The value, or 0 if the parameter does not exist, or is not a integer</returns>
    public int GetParamI(string name) {
        if(eParams.HasParam(name)) {
            EditableParam e = eParams.GetParam(name);
            if(e.type == ParamType.INT) {
                return e.valueI;
            }
        }
        return 0;
    }
    /// <summary>
    /// Returns the boolean value of a parameter
    /// </summary>
    /// <param name="name">Parameter name</param>
    /// <returns>The value, or false if the parameter does not exist, or is not a boolean</returns>
    public bool GetParamB(string name) {
        if(eParams.HasParam(name)) {
            EditableParam e = eParams.GetParam(name);
            if(e.type == ParamType.BOOL) {
                return e.valueB;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets the type of a named parametr
    /// </summary>
    /// <param name="name">Parameter name</param>
    /// <returns>Parameter type</returns>
    public ParamType GetParamType(string name) {
        if(eParams.HasParam(name)) {
            return eParams.GetParam(name).type;
        }
        return ParamType.NULL;
    }

    /// <summary>
    /// Selects the intractable, and makes the selection box visible
    /// </summary>
    public void Select() {
        for(int i = 0; i < selBoxes.Length; i++) {
            selBoxes[i].show = true;
        }
    }
    /// <summary>
    /// Unselects the intractable, and makes the selection box invisible
    /// </summary>
    public void UnSelect() {
        for(int i = 0; i < selBoxes.Length; i++) {
            selBoxes[i].show = false;
        }
    }
    /// <summary>
    /// Sets the selection state of the intractable, change selection box visibility to match
    /// </summary>
    /// <param name="sel">If it is selected</param>
    public void Selected(bool sel) {
        for(int i = 0; i < selBoxes.Length; i++) {
            selBoxes[i].show = sel;
        }
    }

    /// <summary>
    /// Gets an intractable from a GameObject, through a ChildCollider
    /// </summary>
    /// <param name="gO">The GameObject to look at</param>
    /// <returns>The Intractable, or null if none could be found</returns>
    public static Interactable GetInteractable(GameObject gO) {
        Interactable ot = gO.GetComponent<Interactable>();
        if(ot == null) {
            ChildCollider cC = gO.GetComponent<ChildCollider>();
            if(cC != null) {
                ot = cC.interactable;
            }
        }
        return ot;
    }

}
