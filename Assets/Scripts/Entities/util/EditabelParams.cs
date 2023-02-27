using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EditabelParams {
    
    /// <summary>
    /// The array of parameter editable in game
    /// </summary>
    public EditableParam[] parameters;
    private Dictionary<string, EditableParam> paramDict;

    /// <summary>
    /// If a parameter with the name exists
    /// </summary>
    /// <param name="name">The name to look for</param>
    /// <returns>If there is a parameter by that name</returns>
    public bool HasParam(string name) {
        if(paramDict == null) paramDict = new Dictionary<string, EditableParam>();
        if(paramDict.ContainsKey(name)) return true;
        for(int i = 0; i < parameters.Length; i++) {
            if(parameters[i].name.Equals(name)) {
                paramDict.Add(name, parameters[i]);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Returns the parameter with the name
    /// </summary>
    /// <param name="name">The parameter name</param>
    /// <returns>The EditableParam, or null if no parameter with name exists</returns>
    public EditableParam GetParam(string name) {
        if(!HasParam(name)) {
            return null;
        }
        return paramDict[name];
        // for(int i = 0; i < parameters.Length; i++) {
        //     if(parameters[i].name.Equals(name)) {
        //         return parameters[i];
        //     }
        // }
        // return null;
    }

    /// <summary>
    /// Checks if a parameter with name has a range
    /// </summary>
    /// <param name="name">The parameter name</param>
    /// <returns>If the parameter is ranged, or false if the parameter does not exist</returns>
    public bool IsParameterRanged(string name) {
        if(!HasParam(name)) {
            return false;
        }
        return paramDict[name].range.x != paramDict[name].range.y;
        // for(int i = 0; i < parameters.Length; i++) {
        //     if(parameters[i].name.Equals(name)) {
        //         return parameters[i].range.x != parameters[i].range.y;
        //     }
        // }
        // return false;
    }

    /// <summary>
    /// Gets the range of a parameter
    /// </summary>
    /// <param name="name">The parameter name</param>
    /// <returns>The range of the parametr, or 0 if the parameter does not exist</returns>
    public Vector2 GetParamRange(string name) {
        if(!HasParam(name)) {
            return Vector2.zero;
        }
        return paramDict[name].range;
        // for(int i = 0; i < parameters.Length; i++) {
        //     if(parameters[i].name.Equals(name)) {
        //         return parameters[i].range;
        //     }
        // }
        // return Vector2.zero;
    }
    /// <summary>
    /// If the parameter should have a slider
    /// </summary>
    /// <param name="name"><The parameter name/param>
    /// <returns>If the parameter should have a slider, or false if the parameter does not exist</returns>
    public bool IsParamSlid(string name) {
        if(!HasParam(name)) {
            return false;
        }
        return paramDict[name].slid;
        // for(int i = 0; i < parameters.Length; i++) {
        //     if(parameters[i].name.Equals(name)) {
        //         return parameters[i].slid;
        //     }
        // }
        // return false;
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

/// <summary>
/// An editable parameter that is serialized in the save process
/// </summary>
[System.Serializable]
public class EditableParam {
    /// <summary>
    /// The name of the parameter. Must be unique to the object
    /// </summary>
    public string name;
    /// <summary>
    /// The type of parameter
    /// </summary>
    public ParamType type;
    /// <summary>
    /// The string value of the parameter
    /// </summary>
    public string valueS;
    /// <summary>
    /// The float value of the parameter
    /// </summary>
    public float valueF;
    /// <summary>
    /// The integer value of the parameter
    /// </summary>
    public int valueI;
    /// <summary>
    /// The boolean value of the parameter
    /// </summary>
    public bool valueB;
    /// <summary>
    /// The range of the parameter
    /// </summary>
    public Vector2 range;
    /// <summary>
    /// If a slider should be used for the parameter
    /// </summary>
    public bool slid;

    /// <summary>
    /// Validates the state of the parameter
    /// </summary>
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

/// <summary>
/// Type of editable parameter
/// </summary>
[System.Serializable]
public enum ParamType {
    NULL,
    STRING,
    FLOAT,
    INT,
    BOOL
}
