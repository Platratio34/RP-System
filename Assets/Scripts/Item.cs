using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public string itemID;
    public Sprite icon;
    public Ico3d icon3d;
    
    public override string ToString() {
        return "Item {id:\"" + itemID + "\", icon:[" + (icon==null?".":"X") + "], i3d:[" + (icon3d==null?".":"X") + "]";
    }
}
[System.Serializable]
public class Ico3d {
    public GameObject icon;
    public Vector3 posOffset;
    public Vector3 rotOffset;
}