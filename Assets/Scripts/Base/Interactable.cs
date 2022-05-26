using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public Vector3 selBoxOffset;
    public Vector3 selBoxScale;
    public EditabelParams eParams;
    public string id;
    public string dispName;
    public LocalString localName;
    public bool isNameKey = false;

    public SelectionBox[] selBoxes;

    /// <summary>Called when the the interact key is used on the entity</summary>
    public void OnInteract(bool gm) {
        OnInteract(gm, 0);
    }
    /// <summary>Called when the the interact key is used on the entity</summary>
    public virtual void OnInteract(bool gm, int b) {}
    /// <summary>Called right after an editable paramter is changed</summary>
    protected virtual void OnEdit() {}
    /// <summary>Called when the interactable is saved</summary>
    public virtual JsonObj Save() {
        return new JsonObj();
    }
    /// <summary>Called when the interactable is loaded</summary>
    public virtual void Load(JsonObj obj) {}

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

    public string[] GetParams() {
        string[] s = new string[eParams.parameters.Length];
        for(int i = 0; i < s.Length; i++) {
            s[i] = eParams.parameters[i].name;
        }
        return s;
    }

    public bool IsParameterRanged(string name) {
        return eParams.IsParameterRanged(name);
    }
    public Vector2 GetParamRange(string name) {
        return eParams.GetParamRange(name);
    }

    public bool IsParamSlid(string name) {
        return eParams.IsParamSlid(name);
    }

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
    public string GetParamS(string name) {
        if(eParams.HasParam(name)) {
            EditableParam e = eParams.GetParam(name);
            if(e.type == ParamType.STRING) {
                return e.valueS;
            }
        }
        return "";
    }
    public float GetParamF(string name) {
        if(eParams.HasParam(name)) {
            EditableParam e = eParams.GetParam(name);
            if(e.type == ParamType.FLOAT) {
                return e.valueF;
            }
        }
        return 0f;
    }
    public int GetParamI(string name) {
        if(eParams.HasParam(name)) {
            EditableParam e = eParams.GetParam(name);
            if(e.type == ParamType.INT) {
                return e.valueI;
            }
        }
        return 0;
    }
    public bool GetParamB(string name) {
        if(eParams.HasParam(name)) {
            EditableParam e = eParams.GetParam(name);
            if(e.type == ParamType.BOOL) {
                return e.valueB;
            }
        }
        return false;
    }

    public ParamType GetParamType(string name) {
        if(eParams.HasParam(name)) {
            return eParams.GetParam(name).type;
        }
        return ParamType.NULL;
    }

    public void Select() {
        for(int i = 0; i < selBoxes.Length; i++) {
            selBoxes[i].show = true;
        }
    }
    public void UnSelect() {
        for(int i = 0; i < selBoxes.Length; i++) {
            selBoxes[i].show = false;
        }
    }
    public void Selected(bool sel) {
        for(int i = 0; i < selBoxes.Length; i++) {
            selBoxes[i].show = sel;
        }
    }

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
