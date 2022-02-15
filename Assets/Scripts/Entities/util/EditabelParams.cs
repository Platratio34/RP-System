using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EditabelParams {
    
    public EditableParam[] parameters;

    public bool HasParam(string name) {
        for(int i = 0; i < parameters.Length; i++) {
            if(parameters[i].name.Equals(name)) {
                return true;
            }
        }
        return false;
    }
    public EditableParam GetParam(string name) {
        if(!HasParam(name)) {
            return null;
        }
        for(int i = 0; i < parameters.Length; i++) {
            if(parameters[i].name.Equals(name)) {
                return parameters[i];
            }
        }
        return null;
    }

    public bool IsParameterRanged(string name) {
        if(!HasParam(name)) {
            return false;
        }
        for(int i = 0; i < parameters.Length; i++) {
            if(parameters[i].name.Equals(name)) {
                return parameters[i].range.x != parameters[i].range.y;
            }
        }
        return false;
    }

    public Vector2 GetParamRange(string name) {
        if(!HasParam(name)) {
            return Vector2.zero;
        }
        for(int i = 0; i < parameters.Length; i++) {
            if(parameters[i].name.Equals(name)) {
                return parameters[i].range;
            }
        }
        return Vector2.zero;
    }
    public bool IsParamSlid(string name) {
        if(!HasParam(name)) {
            return false;
        }
        for(int i = 0; i < parameters.Length; i++) {
            if(parameters[i].name.Equals(name)) {
                return parameters[i].slid;
            }
        }
        return false;
    }

    // public JsonObj Save() {
    //     JsonObj o = new JsonObj();
    //     for(int i = 0; i < parameters.Length; i++) {
    //         if(parameters[i].type == ParamType.STRING) o.AddKey(parameters[i].name, parameters[i].valueS);
    //         else if(parameters[i].type == ParamType.FLOAT) o.AddFloat(parameters[i].name, parameters[i].valueF);
    //         else if(parameters[i].type == ParamType.INT) o.AddFloat(parameters[i].name, parameters[i].valueI);
    //         else if(parameters[i].type == ParamType.BOOL) o.AddBool(parameters[i].name, parameters[i].valueB);
    //     }
    //     return o;
    // }

    // public void Load(JsonObj o) {
    //     for(int i = 0; i < parameters.Length; i++) {
    //         if(o.ContainsKey(parameters[i].name)) {
    //             if(parameters[i].type == ParamType.STRING) parameters[i].valueS = o.GetString(parameters[i].name);
    //             else if(parameters[i].type == ParamType.FLOAT) parameters[i].valueF = o.GetFloat(parameters[i].name);
    //             else if(parameters[i].type == ParamType.INT) parameters[i].valueI = (int)o.GetFloat(parameters[i].name);
    //             else if(parameters[i].type == ParamType.BOOL) parameters[i].valueB = o.GetBool(parameters[i].name);
    //         }
    //     }
    // }
}

[System.Serializable]
public class EditableParam {
    public string name;
    public ParamType type;
    public string valueS;
    public float valueF;
    public int valueI;
    public bool valueB;
    public Vector2 range;
    public bool slid;

    public void Validate() {
        if(range.x != range.y) {
            if(type == ParamType.FLOAT) {
                valueF = Mathf.Clamp(valueF, range.x, range.y);
            } else if(type == ParamType.INT) {
                valueI = (int)Mathf.Clamp(valueI, range.x, range.y);
            }
        }
    }
}

[System.Serializable]
public enum ParamType {
    NULL,
    STRING,
    FLOAT,
    INT,
    BOOL
}
