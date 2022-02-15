using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalStrings : MonoBehaviour {
    private static Dictionary<string,LocalLang> local;
    /// <value>The currently selected language. Default: "en"</value>
    public static string lang = "en";
    /// <value>The array of language files to load. The value of <c>lang</c> should be in this array</value>
    public static string[] langs;
    public string[] langs2;

    void Awake() {
        langs = langs2;
        LoadDict();
    }

    private static void LoadDict() {
        local = new Dictionary<string, LocalLang>();
        if(langs == null) {
            Debug.LogError("Language array not defined");
            langs = new string[]{lang};
        }
        for(int i = 0; i < langs.Length; i++) {
            string f = "Localizaton/" + langs[i] + ".json";
            if(SaveLoadData.Exists(f)) {
                JsonObj l = StringParser.ParseObject(SaveLoadData.LoadString(f)).data;
                local.Add(langs[i], new LocalLang(l));
            }
        }
    }

    
    /// <summary>Retuns a localized string based on the key from the current language. For string from a catagory, use the method with 2 paramaters</summary>
    /// <param name="key">The localization key</param>
    public static string GetLocalString(string key) {
        string r = "";
        if(local == null) {
            LoadDict();
        }
        if(local.ContainsKey(lang)) {
            if(local[lang].strings.ContainsKey(key)) {
                return local[lang].strings[key];
            }
            r = GetLocalStringD("lang", "missingString_keyInLang", new string[]{key,lang});
        } else {
            r = GetLocalStringD("lang", "missingLanguage", new string[]{lang});
        }
        print(r);
        return r;
    }

    /// <summary>Retuns a localized string based on the catagory and key from the current language</summary>
    /// <param name="catagory">The first level catagory of key</param>
    /// <param name="key">The localization key. May include '.' to mark sub-catagory, ex: "file.save"</param>
    public static string GetLocalString(string catagory, string key) {
        string r = "";
        if(local == null) {
            LoadDict();
        }
        if(local.ContainsKey(lang)) {
            if(local[lang].catagories.ContainsKey(catagory)) {
                string[] keys = key.Split('.');
                string str = local[lang].catagories[catagory].GetString(keys);

                if(str != null) {
                    return str;
                } else {
                    r = GetLocalStringD("lang", "missingString_keyInCatOfLang", new string[]{key,catagory,lang} );
                }
            } else {
                r = GetLocalStringD("lang", "missingCatagory", new string[]{catagory,lang} );
            }
        } else {
            r = GetLocalStringD("lang", "missingLanguage", new string[]{lang} );
        }
        Debug.LogError(r);
        return r;
    }
    /// <summary>Returns a localized string based on the catagory and key from the current language, with varible replacment</summary>
    /// <param name="cat">The first level catagory of key</param>
    /// <param name="key">The localization key. May include '.' to mark sub-catagory, ex: "file.save"</param>
    /// <param name="drops">The array of varibles to drop into the string</param>
    public static string GetLocalStringD(string cat, string key, string[] drops) {
        string str = GetLocalString(cat,key);
        if(str.Contains("[")) {
            string[] parts = str.Split('[');
            string str2 = parts[0];
            for(int i = 1; i < parts.Length; i++) {
                string[] parts2 = parts[i].Split(']');
                int di = 0;
                if(int.TryParse(parts2[0], out di)) {
                    if(di < drops.Length) {
                        str2 += drops[di];
                        str2 += parts2[1];
                    } else {
                        Debug.LogError("Failed to drop in variable #" + di + ", Index out of range");
                    }
                } else {
                    Debug.LogError("Failed to drop in variable \"" + parts2[0] + "\", Id not a number");
                }
                // Debug.Log(parts[i]);
            }
            return str2;
        }
        return str;
    }

    private class LocalLang {
        public Dictionary<string, string> strings;
        public Dictionary<string, LocalLang> catagories;

        public LocalLang(JsonObj o) {
            strings = new Dictionary<string, string>();
            catagories = new Dictionary<string, LocalLang>();
            foreach(KeyValuePair<string, JsonObj> p in o.GetObjs()) {
                if(p.Value.NumberOfKeys() == 0) {
                    strings.Add(p.Key, p.Value.ToString(false));
                } else {
                    catagories.Add(p.Key, new LocalLang(p.Value));
                }
            }
        }

        /// <summary>Returns a string from the catagory based on the keys provided</summary>
        /// <param name="keys">Array of keys, each element except the last is a sub-catagory</param>
        public string GetString(string[] keys) {
            return GetString(keys, 0);
        }
        /// <summary>Returns a string from the catagory based on the keys provided started at index <c>l</c></summary>
        /// <param name="keys">Array of keys, each element except the last is a sub-catagory</param>
        /// <param name="l">The index to start looking for a  key or catagory at</param>
        private string GetString(string[] keys, int l) {
            if(strings.ContainsKey(keys[l])) {
                return strings[keys[l]];
            } else {
                if(catagories.ContainsKey(keys[l])) {
                    return catagories[keys[l]].GetString(keys, 1);
                } else {
                    return null;
                }
            }
        }
    }
}
