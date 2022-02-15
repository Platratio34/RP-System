using System.Collections.Generic;
using UnityEngine;

/// <summary><c>String Parser</c> is a librar of functions for parsing Json type string</summary>
public static class StringParser {
    
    /// <summary><c>Parse For String</c> pareses a Json type string for a string</summary>
    /// <param name="str">The string to parse</param>
    /// <returns>A <c>StringParse</c> object of type <c>string</c>, use <c>".data"</c> for the string</returns>
    public static StringParse<string> ParseForString(string str) {
        string o = "";
        int i = 0;
        int sb = 0;
        int cb = 0;
        bool q = false;
        bool q2 = false;
        for(i = 0; i < str.Length; i++) {
            if(!q) {
                if(str[i] == '}') {
                    cb--;
                } else if(str[i] == '{') {
                    cb++;
                } else if(str[i] == ']') {
                    sb--;
                } else if(str[i] == '[') {
                    sb++;
                }
            }
            if(str[i] == '"') {
                if(!(i > 0 && str[i-1] == '\\')) {
                    q = !q;
                    if(cb == 0 && sb == 0) {
                        q2 = true;
                    }
                }
            }
            if(!q2)
                if(str[i] == ',' && cb == 0 && sb == 0 && !q) {
                    return new StringParse<string>(o, i);
                } else {
                    if(!q) {
                        if(!(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r') ) {
                            o += str[i];
                        }
                    } else {
                        o += str[i];
                    }
                }
            q2 = false;
        }
        return new StringParse<string>(o, i);
    }

    /// <summary><c>Parse For KV</c> pareses a Json type key value par (ex. "key":"value" )</summary>
    /// <param name="str">The string to parse</param>
    /// <returns>A <c>StringParse</c> object of type <c>string[]</c>, use <c>".data"</c> for the string array. Index 0 is the key and index 1 is the value</returns>
    public static StringParse<string[]> ParseForKV(string str) {
        string[] o = new string[] {"",""};
        bool k = true;
        bool v = false;
        int i = 0;
        int sb = 0;
        int cb = 0;
        bool q = false;
        bool q2 = false;
        for(i = 0; i < str.Length; i++) {
            if(!q) {
                if(str[i] == '}') {
                    cb--;
                } else if(str[i] == '{') {
                    cb++;
                } else if(str[i] == ']') {
                    sb--;
                } else if(str[i] == '[') {
                    sb++;
                }
            }
            if(str[i] == '"') {
                if(!(i > 0 && str[i-1] == '\\')) {
                    q = !q;
                    if(cb == 0 && sb == 0) {
                        q2 = true;
                    }
                }
            }
            if(!q2) {
                if((str[i] == '=' || str[i] == ':') && k && !q) {
                    k = false;
                    v = true;
                } else if(str[i] == ',' && cb == 0 && sb == 0 && !q) {
                    Debug.Log(o[0] + ":" + o[1]);
                    return new StringParse<string[]>(o, i);
                } else if(k && !(str[i] == '{' && !q) ) {
                    if(!q && (cb == 0 && sb == 0)) {
                        if(!(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r') ) {
                            o[0] += str[i];
                        }
                    } else {
                        o[0] += str[i];
                    }
                } else if(v) {
                    if(!q && (cb == 0 && sb == 0)) {
                        if(!(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r') ) {
                            o[1] += str[i];
                        }
                    } else {
                        o[1] += str[i];
                    }
                }
            }
            q2 = false;
        }
        return new StringParse<string[]>(o, i);
    }

    /// <summary><c>Parse For Array</c> pareses a Json type key array (ex. ["value","value",...] )</summary>
    /// <param name="str">The string to parse</param>
    /// <returns>A <c>StringParse</c> object of type <c>JsonObject</c>, use <c>".data"</c> for the Json Object, and <c>JsonObj.array</c> for the array</returns>
    public static StringParse<JsonObj> ParseForArray(string str) {
        List<JsonObj> o = new List<JsonObj>();
        string s = "";
        int i = 0;
        int sb = 0;
        int cb = 0;
        bool q = false;
        bool q2 = false;
        for(i = 0; i < str.Length; i++) {
            if(!q) {
                if(str[i] == '}') {
                    cb--;
                } else if(str[i] == '{') {
                    cb++;
                } if(str[i] == ']') {
                    sb--;
                    if(sb <= 0 && cb == 0) {
                        o.Add(new JsonObj(s, true));
                        s = "";
                        return new StringParse<JsonObj>(new JsonObj(o.ToArray()), i);
                    }
                } else if(str[i] == '[') {
                    sb++;
                }
            }
            if(str[i] == '"') {
                if(!(i > 0 && str[i-1] == '\\')) {
                    q = !q;
                    if(cb == 0 && sb == 1) {
                        q2 = true;
                    }
                }
            }
            if(!q2)
                if(str[i] == ',' && sb == 1 && cb == 0 && !q) {
                    o.Add(new JsonObj(s, true));
                    s = "";
                } else if(i > 0) {
                    if(!q && (cb == 0 && sb == 1)) {
                        if(!(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r') ) {
                            s += str[i];
                        }
                    } else {
                        s += str[i];
                    }
                }
            q2 = false;
        }
        return new StringParse<JsonObj>(new JsonObj(o.ToArray()), i);
    }

    /// <summary><c>Parse Object</c> pareses a Json type key Object (ex. {"key":"value","key":"value",...} )</summary>
    /// <param name="str">The string to parse</param>
    /// <returns>A <c>StringParse</c> object of type <c>JsonObj</c>, use <c>".data"</c> for the Json Object, and <c>JsonObj.objs</c> to acces the KV pairs</returns>
    public static StringParse<JsonObj> ParseObject(string str) {
        JsonObj obj = new JsonObj();
        string s = "";
        int i = 0;
        int sb = 0;
        int cb = 0;
        bool q = false;
        // bool q2 = false;
        for(i = 0; i < str.Length; i++) {
            if(!q) {
                if(str[i] == '}') {
                    cb--;
                    if(cb <= 0 && sb <= 0) {
                        obj.Add(s);
                        s = "";
                        return new StringParse<JsonObj>(obj, i);
                    }
                } else if(str[i] == '{') {
                    cb++;
                } if(str[i] == ']') {
                    sb--;
                } else if(str[i] == '[') {
                    sb++;
                }
            }
            if(str[i] == '"') {
                if(!(i > 0 && str[i-1] == '\\')) {
                    q = !q;
                    // if(cb == 1 && sb == 0) {
                    //     q2 = true;
                    // }
                }
            }
            if(str[i] == ',' && cb == 1 && sb == 0 && !q) {
                obj.Add(s);
                s = "";
            } else if(i > 0) {
                if(!q && (cb == 1 && sb == 0)) {
                    if(!(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r') ) {
                        s += str[i];
                    }
                } else {
                    s += str[i];
                }
            }
            // q2 = false;
        }
        return new StringParse<JsonObj>(obj, i);
    }
}
/// <summary><c>String Parse</c> of type <typeparamref name="T"/> is the return type for all of <c>String Parser</c></summary>
/// <typeparam name="T">The type of data returned</typeparam>
public struct StringParse<T> {
    /// <value>The number of charecters that were parsed through</value>
    public int l;
    /// <value>The return data</value>
    public T data;
    
    /// <summary>Constructor for <c>String Parse</c></summary>
    /// <param name="data">The return data</param>
    /// <param name="l">The number of charecters that were parsed through</param>
    public StringParse(T data, int l) {
        this.data = data;
        this.l = l;
    }
}
