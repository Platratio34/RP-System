using System.Collections.Generic;
using UnityEngine;

/// <summary><c>Json Object</c> is a class used for saving and loading JSON</summary>
public class JsonObj {
    /// <summary>The key value pairs of the JSON object</summary>
    protected Dictionary<string, JsonObj> objs;
    /// <summary>The array value</summary>
    public JsonObj[] array;
    /// <summary>The string value</summary>
    public string val;
    protected JsonParamType type = JsonParamType.STRING;

    /// <summary>Default constructor for <c>JsonObj</c></summary>
    public JsonObj() {
        objs = new Dictionary<string, JsonObj>();
    }
    /// <summary>String constructor for <c>JsonObj</c></summary>
    /// <param name="str">The string to set as the value, if <c>parse</c> is false</param>
    /// <param name="parse">If true, <c>str</c> will be parsed into sub objects, otherwise, <c>str</c> will be set as the value of the object</param>
    public JsonObj(string str, bool parse) {
        objs = new Dictionary<string, JsonObj>();
        if(!parse) {
            val = str;
        } else {
            if(str.Length > 0) {
                if(str[0] == '{') {
                    JsonObj obj = StringParser.ParseObject(str).data;
                    objs = obj.objs;
                } else if(str[0] == '[') {
                    JsonObj obj = StringParser.ParseForArray(str).data;
                    array = obj.array;
                } else {
                    val = str;
                }
            }
        }
    }
    /// <summary>Array constructor for <c>JsonObj</c></summary>
    /// <param name="array">The array to be set as the value of the object</param>
    public JsonObj(JsonObj[] array) {
        objs = new Dictionary<string, JsonObj>();
        this.array = array;
    }
    /// <summary>Type constructor for <c>JsonObj</c> with value</summary>
    /// <param name="value">The value of the object</param>
    /// <param name="type">The type of Json object</param>
    protected JsonObj(string value, JsonParamType type) {
        this.type = type;
        val = value;
    }
    /// <summary>Type constructor for <c>JsonObj</c></summary>
    /// <param name="type">The type of Json object</param>
    protected JsonObj(JsonParamType type) {
        this.type = type;
    }
    
    /// <summary>Adds a Key Value Pair to the object</summary>
    /// <param name="str">The string to parse for the Key Value Pair</param>
    public void Add(string str) {
        if(objs == null) {
            objs = new Dictionary<string, JsonObj>();
        }
        string[] r = StringParser.ParseForKV(str).data;
        if(r[1].Length == 0) {
            // Debug.LogError("Adding value of length 0. key: \"" + r[0] + "\"");
            objs.Add(r[0], new JsonObj(r[1], false));
            return;
        } else if(r[1][0] == '{') {
            objs.Add(r[0], new JsonObj(r[1], true));
            return;
        } else if(r[1][0] == '[') {
            objs.Add(r[0], StringParser.ParseForArray(r[1]).data);
            return;
        }
        objs.Add(r[0], new JsonObj(r[1], false));
    }
    /// <summary>Adds a Key Value Pair to the object</summary>
    /// <param name="key">The the key for the Key Value Pair</param>
    /// <param name="value">The the value for the Key Value Pair, will be parsed for objects or array if possible</param>
    public void AddKey(string key, string value) {
        if(objs == null) {
            objs = new Dictionary<string, JsonObj>();
        }
        if(value.Length > 0) {
            if(value[0] == '{') {
                objs.Add(key, new JsonObj(value, true));
                return;
            } else if(value[0] == '[') {
                objs.Add(key, StringParser.ParseForArray(value).data);
                return;
            }
        }
        objs.Add(key, new JsonObj(value, false));
        return;
    }
    /// <summary>Adds a Key Value Pair to the object</summary>
    /// <param name="key">The the key for the Key Value Pair</param>
    /// <param name="value">The the value for the Key Value Pair, will be parsed for objects or array if possible</param>
    /// <param name="type">The type of value</param>
    public void AddKey(string key, string value, JsonParamType type) {
        if(objs == null) {
            objs = new Dictionary<string, JsonObj>();
        }
        if(value.Length > 0) {
            if(value[0] == '{') {
                objs.Add(key, new JsonObj(value, true));
                return;
            } else if(value[0] == '[') {
                objs.Add(key, StringParser.ParseForArray(value).data);
                return;
            }
        }
        objs.Add(key, new JsonObj(value, type));
        return;
    }
    /// <summary>Adds a Key Value Pair to the object</summary>
    /// <param name="key">The the key for the Key Value Pair</param>
    /// <param name="value">The the value for the Key Value Pair</param>
    public void AddKey(string key, JsonObj value) {
        if(objs == null) {
            objs = new Dictionary<string, JsonObj>();
        }
        objs.Add(key, value);
    }
    /// <summary>Adds a Key Value Pair to the object</summary>
    /// <param name="key">The the key for the Key Value Pair</param>
    /// <param name="value">The the value for the Key Value Pair</param>
    public void AddKey(string key, JsonObj[] array) {
        if(objs == null) {
            objs = new Dictionary<string, JsonObj>();
        }
        objs.Add(key, new JsonObj(array));
    }

    /// <summary>Formats the object ito a JSON string</summary>
    /// <param name="q">Quote values, set to false if you are getting the value only</param>
    /// <param name="ind">Make string nice</param>
    /// <param name="s">Starting string for each line. (Mostly for internal use)</param>
    public string ToString(bool q, bool ind, string s) {
        string str = "";
        if(objs != null && objs.Count > 0) {
            bool f = true;
            str += "{";
            foreach (KeyValuePair<string,JsonObj> o in objs) {
                if(!f) {
                    str += ",";
                    if(type == JsonParamType.VECTOR3 || type == JsonParamType.QUATERNION) str += ind?" ":"";
                }
                if(type == JsonParamType.VECTOR3 || type == JsonParamType.QUATERNION) {
                    str += "\"" + o.Key + "\":";
                    str += ind?" ":"";
                    str += o.Value.ToString(true, ind, s + "\t");
                } else if(ind) {
                    str += "\n" + s + "\t\"" + o.Key + "\": " + o.Value.ToString(true, ind, s + "\t");
                } else {
                    str += "\"" + o.Key + "\":" + o.Value.ToString(true, ind,s);
                }
                f = false;
            }
            if(ind && !(type == JsonParamType.VECTOR3 || type == JsonParamType.QUATERNION) ) { str += "\n"+s; }
            str += "}";
        } else if(array != null) {
            str = "[";
            for(int i = 0; i < array.Length; i++) {
                if(i > 0) { str += ","; }
                if(ind) {
                    str += "\n" + s + "\t" + array[i].ToString(true, ind,s + "\t");
                } else {
                    str += array[i].ToString(true, ind,s);
                }
            }
            if(ind) { str += "\n"+s; }
            str += "]";
        } else {
            if(q && !(type == JsonParamType.FLOAT || type == JsonParamType.BOOL) ) {
                str = '"' + val+ '"';
            } else {
                str = val.Replace("\\\"", "\"");
            }
        }
        return str;
    }
    /// <summary>Formats the object ito a JSON string, using default parameters (quotes enabled, non-nice string)</summary>
    public override string ToString() {
        return ToString(true, false, "");
    }
    /// <summary>Formats the object ito a JSON string, using default parameter (non-nice string)</summary>
    /// <param name="q">Quote values, set to false if you are getting the value only</param>
    public string ToString(bool q) {
        return ToString(q, false, "");
    }
    /// <summary>Formats the object ito a JSON string, using default parameter (quotes enabled)</summary>
    /// <param name="i">Make string nice</param>
    /// <param name="s">Starting string for each line. (Mostly for internal use)</param>
    public string ToString(bool i, string s) {
        return ToString(true, i, s);
    }

    /// <summary>Returns a Vector3 from keys "x", "y", "z", of parameter <c>key</c></summary>
    /// <param name="key">The parameter to parse</param>
    public Vector3 GetVector3(string key) {
        if(!objs.ContainsKey(key)) {
            return new Vector3();
        }
        return objs[key].GetVector3();
    }
    /// <summary>Returns a Vector3 from keys "x", "y", "z"</summary>
    public Vector3 GetVector3() {
        return new Vector3( GetFloat("x"), GetFloat("y"), GetFloat("z") );
    }

    /// <summary>Adds a Vector3 key named <c>key</c></summary>
    /// <param name="key">The key</param>
    /// <param name="x">The x value</param>
    /// <param name="y">The y value</param>
    /// <param name="z">The z value</param>
    public void AddVector3(string key, float x, float y, float z) {
        JsonObj v = new JsonObj(JsonParamType.VECTOR3);
        v.AddFloat("x", x);
        v.AddFloat("y", y);
        v.AddFloat("z", z);
        AddKey(key, v);
    }
    /// <summary>Adds a Vector3 key named <c>key</c></summary>
    /// <param name="key">The key</param>
    /// <param name="v3">The Vector3 to add</param>
    public void AddVector3(string key, Vector3 v3) {
        AddVector3(key, v3.x, v3.y, v3.z);
    }

    /// <summary>Returns a Quaternion from keys "x", "y", "z", "w", of parameter <c>key</c></summary>
    /// <param name="key">The parameter to parse</param>
    public Quaternion GetQuaternion(string key) {
        if(!objs.ContainsKey(key)) {
            return new Quaternion();
        }
        return objs[key].GetQuaternion();
    }
    /// <summary>Returns a Quaternion from keys "x", "y", "z", "w"</summary>
    public Quaternion GetQuaternion() {
        return new Quaternion(GetFloat("x"), GetFloat("y"), GetFloat("z"), GetFloat("w"));
    }

    /// <summary>Adds a Quaternion key named <c>key</c></summary>
    /// <param name="key">The key</param>
    /// <param name="x">The x value</param>
    /// <param name="y">The y value</param>
    /// <param name="z">The z value</param>
    /// <param name="w">The z value</param>
    public void AddQuaternion(string key, float x, float y, float z, float w) {
        JsonObj v = new JsonObj(JsonParamType.QUATERNION);
        v.AddFloat("x", x);
        v.AddFloat("y", y);
        v.AddFloat("z", z);
        v.AddFloat("w", w);
        AddKey(key, v);
    }
    /// <summary>Adds a Quaternion key named <c>key</c></summary>
    /// <param name="key">The key</param>
    /// <param name="1">The Quaternion to add</param>
    public void AddQuaternion(string key, Quaternion q) {
        AddQuaternion(key, q.x, q.y, q.z, q.w);
    }
    
    /// <summary>Returns a float from parameter <c>key</c></summary>
    /// <param name="key">The parameter to parse</param>
    public float GetFloat(string key) {
        if(!objs.ContainsKey(key)) {
            return 0;
        }
        return objs[key].GetFloat();
    }
    /// <summary>Returns a float from value</summary>
    public float GetFloat() {
        float v = 0;
        float.TryParse(val, out v);
        return v;
    }
    
    /// <summary>Adds a float key named <c>key</c></summary>
    /// <param name="key">The key</param>
    /// <param name="v">The float to add</param>
    public void AddFloat(string key, float v) {
        AddKey(key, v + "", JsonParamType.FLOAT);
    }

    /// <summary>Returns the boolean from parameter <c>key</c></summary>
    /// <param name="key">The parameter to parse</param>
    public bool GetBool(string key) {
        if(!objs.ContainsKey(key)) {
            return false;
        }
        return objs[key].GetBool();
    }
    /// <summary>Returns the boolean from value</summary>
    public bool GetBool() {
        bool v = false;
        bool.TryParse(val, out v);
        return v;
    }

    /// <summary>Adds a boolean key named <c>key</c></summary>
    /// <param name="key">The key</param>
    /// <param name="v">The boolean to add</param>
    public void AddBool(string key, bool v) {
        AddKey(key, v + "", JsonParamType.BOOL);
    }

    /// <summary>Returns the string from parameter <c>key</c></summary>
    /// <param name="key">The parameter to get</param>
    public string GetString(string key) {
        if(!objs.ContainsKey(key)) {
            return "";
        }
        return objs[key].GetString();
    }
    /// <summary>Returns the string from value</summary>
    public string GetString() {
        return val;
    }

    /// <summary>Returns the Json Object from parameter<c>key</c></summary>
    /// <param name="key">The parameter to get</param>
    public JsonObj GetObj(string key) {
        if(!objs.ContainsKey(key)) {
            return null;
        }
        return objs[key];
    }

    /// <summary>Returns the object dictionary for iterating over from parameter<c>key</c></summary>
    /// <param name="key">The parameter to get</param>
    public Dictionary<string,JsonObj> GetObjs(string key) {
        return objs[key].GetObjs();
    }
    /// <summary>Returns the object dictionary for iterating over</summary>
    public Dictionary<string,JsonObj> GetObjs() {
        return objs;
    }

    /// <summary>Returns the object array from parameter<c>key</c></summary>
    /// <param name="key">The parameter to get</param>
    public JsonObj[] GetArray(string key) {
        return objs[key].GetArray();
    }
    /// <summary>Returns the object array</summary>
    public JsonObj[] GetArray() {
        return array;
    }

    /// <summary>Returns if the object has a parameter <c>key</c></summary>
    /// <param name="key">The key to check for</param>
    public bool ContainsKey(string key) {
        return objs.ContainsKey(key);
    }

    /// <summary>Removes parameter <c>key</c></summary>
    /// <param name="key">The parameter to remove</param>
    public void Remove(string key) {
        objs.Remove(key);
    }

    /// <summary>Returns the number of keys in the object dictionary</summary>
    public int NumberOfKeys() {
        return objs.Count;
    }

    public enum JsonParamType {
        STRING,
        FLOAT,
        VECTOR3,
        OBJ,
        QUATERNION,
        BOOL
    }
}
