using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    public string invName;
    public InvItem[] items;
    public int maxSize = 20;
    void Start() {

    }

    
    void Update() {
        for(int i = 0; i < items.Length; i++) {
            items[i].UpdateDN();
        }
    }

    public void AddItem(string id, int q) {
        InvItem[] i2 = new InvItem[items.Length + 1];
        for(int i = 0; i < items.Length; i++) {
            i2[i] = items[i];
            if(items[i].id.Equals(id)) {
                items[i].q += q;
                return;
            }
        }
        i2[i2.Length - 1] = new InvItem(id, q);
        items = i2;
    }
    public void AddItem(InvItem it) {
        InvItem[] i2 = new InvItem[items.Length + 1];
        for(int i = 0; i < items.Length; i++) {
            i2[i] = items[i];
        }
        i2[i2.Length - 1] = it.Copy();
        items = i2;
    }

    public bool RemoveItem(string id, int q) {
        for(int i = 0; i < items.Length; i++) {
            if(items[i].id == id) {
                items[i].q -= q;
                Clean();
                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(InvItem it) {
        for(int i = 0; i < items.Length; i++) {
            if(items[i].Equals(it)) {
                items[i].q -= it.q;
                Clean();
                return true;
            }
        }
        return false;
    }

    public bool CanAdd() {
        return (items.Length <= maxSize);
    }

    private void Clean() {
        List<InvItem> l = new List<InvItem>();
        for(int i = 0; i < items.Length; i++) {
            if(items[i].q > 0) {
                l.Add(items[i]);
            }
        }
        items = l.ToArray();
        return;
    }

    public JsonObj Save() {
        JsonObj obj = new JsonObj();
        obj.AddKey("name", invName);
        List<JsonObj> li = new List<JsonObj>();
        for(int i = 0; i < items.Length; i++) {
            if(items[i].id != "[Error]" && items[i].q > 0) {
                JsonObj s = items[i].Save();
                li.Add(s);
            }
        }
        obj.AddKey("items", new JsonObj(li.ToArray()));
        return obj;
    }

    public void Load(JsonObj obj) {
        invName = "";
        if(obj.ContainsKey("name")) {
            invName = obj.GetString("name");
        }
        if(obj.ContainsKey("items")) {
            JsonObj[] its = obj.GetArray("items");
            List<InvItem> itemsL = new List<InvItem>();
            for(int i = 0; i < its.Length; i++) {
                InvItem ivI = InvItem.Load(its[i]);
                if(ivI.id != "[Error]" && ivI.q > 0) {
                    itemsL.Add(ivI);
                }
            }
            items = itemsL.ToArray();
        }
        
    }

    public void MoveItems(Inventory inv) {
        for(int i = 0; i < items.Length; i++) {
            inv.AddItem(items[i]);
            items[i].q = 0;
        }
        Clean();
    }
    
    void OnValidate() {
        for(int i = 0; i < items.Length; i++) {
            items[i].UpdateDN();
        }
    }
}

[System.Serializable]
public class InvItem {

    [SerializeField]
    private string dn = "No items";
    public string id = "";
    public int q = -1;
    public CustomData[] customDataArray;
    private Dictionary<string, string> customData;
    public InvItem(string id, int q) {
        this.id = id;
        this.q = q;
    }
    public InvItem(string id, int q, Dictionary<string, string> customData) {
        this.id = id;
        this.q = q;
        this.customData = customData;
    }

    public void AddCustomData(string k, string v) {
        if(customData == null) {
            customData = new Dictionary<string, string>();
        }
        customData.Add(k, v);
    }
    public void SetCustomData(string k, string v) {
        if(customData == null) {
            customData = new Dictionary<string, string>();
        }
        if(customData.ContainsKey(k)) {
            customData[k] = v;
        } else {
            customData.Add(k, v);
        }
    }
    public string GetCustomData(string k) {
        if(customData != null) {
            return customData[k];
        } else {
            return "";
        }
    }
    public void RemoveCustomData(string k) {
        if(customData != null) {
            customData.Remove(k);
        }
    }
    public bool HasCustomData(string k) {
        if(customData != null) {
            return customData.ContainsKey(k);
        } else {
            return false;
        }
    }

    public JsonObj Save() {
        JsonObj obj = new JsonObj();
        obj.AddKey("id", id);
        obj.AddFloat("q", q);
        if(customData == null) {
            customData = new Dictionary<string, string>();
            if(customDataArray != null) {
                for(int i = 0; i < customDataArray.Length; i++) {
                    customData.Add(customDataArray[i].key, customDataArray[i].value);
                }
            }
        }
        if(customData.Count > 0) {
            JsonObj custom = new JsonObj();
            foreach (KeyValuePair<string,string> d in customData) {
                custom.AddKey(d.Key, d.Value);
            }
            obj.AddKey("custom", custom);
        }
        return obj;
    }

    public static InvItem Load(JsonObj obj) {
        string id = "[Error]";
        int q = -1;
        Dictionary<string, string> mD = new Dictionary<string, string>();
        
        if(obj.ContainsKey("id")) id = obj.GetString("id");
        if(obj.ContainsKey("q")) q = (int)obj.GetFloat("q");
        
        if(obj.ContainsKey("custom")) {
            JsonObj custom = obj.GetObj("custom");
            foreach(KeyValuePair<string,JsonObj> p in custom.GetObjs()) {
                mD.Add(p.Key, p.Value.ToString(false));
            }
        }
        return new InvItem(id, q, mD);
    }

    public void UpdateDN() {
        dn = id + " x " + q;
    }

    public InvItem Copy() {
        return new InvItem(id, q, customData);
    }

    
    public bool Equals(InvItem o) {
        return Equals(o, false, false);
    }
    public bool Equals(InvItem o, bool iq, bool im) {
        if(id != o.id) {
            return false;
        }
        if(!iq && q != o.q) {
            return false;
        }
        if(!im && customData != null) {
            foreach(KeyValuePair<string,string> p in customData) {
                if(o.HasCustomData(p.Key)) {
                    if(o.GetCustomData(p.Key) != p.Value) {
                        return false;
                    }
                } else {
                    return false;
                }
            }
        }
        return true;
    }
}
