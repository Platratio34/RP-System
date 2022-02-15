using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalString {
    
    public string key;
    public string catagory;
    public string[] dropIns;

    public override bool Equals(object obj) {
        
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }
        
        LocalString ls = (LocalString)obj;
        return ls.key.Equals(key) && ls.catagory.Equals(catagory);
        
        // return base.Equals (obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public override string ToString() {
        return LocalStrings.GetLocalStringD(catagory, key, dropIns);
    }
    public string ToString(bool print) {
        return "Local String: " + catagory + "." + key;
    }
}
