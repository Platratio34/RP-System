using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Interactable {
    
    public Inventory inventory;
    public InteractableController interactableController;
    public CustomData[] customDataArray;
    public Dictionary<string, string> customData;
    public string entityId;
    public string entityName;
    public string entityType;
    public string parentEntity;
    public List<string> children;
    public bool staticEntity = false;
    public bool editableEntity = true;

    void Start() {
        if(isNameKey && entityName.Equals("")) {
            entityName = LocalStrings.GetLocalString("entitiyTypeNames", dispName);
        }
        customData = new Dictionary<string, string>();
        for(int i = 0; i < customDataArray.Length; i++) {
            customData.Add(customDataArray[i].key, customDataArray[i].value);
        }
        children = new List<string>();
        OnStart();
    }

    /// <summary>Called right after init of base entity</summary>
    public virtual void OnStart() {}
    /// <summary>Called right before the entity is stringified</summary>
    protected virtual void OnEntitySave() {}
    /// <summary>Called right after the entity is loaded from string</summary>
    protected virtual void OnEntityLoad() {}
    /// <summary>Called right before the entity is destroyed</summary>
    public virtual void OnDestory() {}
    /// <summary>Called when the position is set by an exteranl source</summary>
    public virtual void OnSetPosition() {}

    public override JsonObj Save() {
        OnEntitySave();
        
        JsonObj obj = new JsonObj();

        obj.AddKey("type", entityType);
        obj.AddKey("id", entityId);
        obj.AddKey("name", entityName);
        obj.AddBool("static", staticEntity);
        obj.AddBool("editable", editableEntity);
        obj.AddVector3("pos", transform.position);
        obj.AddQuaternion("rot", transform.rotation);

        if(customData != null && customData.Count > 0) {
            JsonObj custom = new JsonObj();
            foreach (KeyValuePair<string,string> d in customData) {
                custom.AddKey(d.Key, d.Value);
            }
            obj.AddKey("custom", custom);
        }
        if(inventory != null) {
            obj.AddKey("inv", inventory.Save());
        }
        obj.AddKey("parent", parentEntity);
        if(interactableController != null) {
            obj.AddKey("interactables", interactableController.Save());
        }
        return obj;
    }

    public override void Load(JsonObj obj) {
        
        if(obj.ContainsKey("type")) entityType = obj.GetString("type");
        if(obj.ContainsKey("id")) entityId = obj.GetString("id");
        if(obj.ContainsKey("name")) entityName = obj.GetString("name");
        if(obj.ContainsKey("static")) staticEntity = obj.GetBool("static");
        if(obj.ContainsKey("editable")) editableEntity = obj.GetBool("editable");
        
        if(obj.ContainsKey("pos")) {
            transform.position = obj.GetVector3("pos");
        } else {
            Vector3 pos = new Vector3();
            if(obj.ContainsKey("px")) pos.x = obj.GetFloat("px");
            if(obj.ContainsKey("py")) pos.y = obj.GetFloat("py");
            if(obj.ContainsKey("pz")) pos.z = obj.GetFloat("pz");
            transform.position = pos;
        }
        
        if(obj.ContainsKey("rot")) {
            transform.rotation = obj.GetQuaternion("rot");
        } else {
            Quaternion rot = new Quaternion();
            if(obj.ContainsKey("rx")) rot.x = obj.GetFloat("rx");
            if(obj.ContainsKey("ry")) rot.y = obj.GetFloat("ry");
            if(obj.ContainsKey("rz")) rot.z = obj.GetFloat("rz");
            if(obj.ContainsKey("rw")) rot.w = obj.GetFloat("rw");
            transform.rotation = rot;
        }

        if(obj.ContainsKey("inv")) inventory.Load(obj.GetObj("inv") );

        if(customData == null) customData = new Dictionary<string, string>();

        if(obj.ContainsKey("custom")) {
            JsonObj custom = obj.GetObj("custom");
            foreach(KeyValuePair<string,JsonObj> p in custom.GetObjs()) {
                customData.Add(p.Key, p.Value.GetString());
            }
        }
        
        if(obj.ContainsKey("parent")) parentEntity = obj.GetString("parent");
        
        if(obj.ContainsKey("interactables")) interactableController.Load(obj.GetObj("interactables"));

        OnEntityLoad();
    }

    public void SetPosition(Vector3 pos) {
        transform.position = pos;
        OnSetPosition();
    }
    public void SetLocalPosition(Vector3 pos) {
        // print("Test " + pos);
        transform.localPosition = pos;
        OnSetPosition();
    }

}

[System.Serializable]
public class CustomData {
    public string key;
    public string value;
}