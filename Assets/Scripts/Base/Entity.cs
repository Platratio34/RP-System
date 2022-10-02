using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic moving object in the game world
/// </summary>
public class Entity : Interactable {
    
    /// <summary>
    /// The inventory associated with the entity
    /// </summary>
    public Inventory inventory;
    /// <summary>
    /// InteractableController for child intractable
    /// </summary>
    public InteractableController interactableController;
    /// <summary>
    /// The custom data of each instance of the entity, used for loading and saving
    /// </summary>
    public CustomData[] customDataArray;
    /// <summary>
    /// Instance custom data, not editable by GM
    /// </summary>
    public Dictionary<string, string> customData;
    /// <summary>
    /// The unique ID of the instance
    /// </summary>
    public string entityId;
    /// <summary>
    /// The displayed name of the entity, may be localized soon
    /// </summary>
    public string entityName;
    /// <summary>
    /// The type of entity, unique to class, but not instance
    /// </summary>
    public string entityType;
    /// <summary>
    /// The parent entity
    /// </summary>
    public string parentEntity;
    /// <summary>
    /// List of child entities
    /// </summary>
    public List<string> children;
    /// <summary>
    /// If the entity is static, IE can not be moved by players or GM
    /// </summary>
    public bool staticEntity = false;
    /// <summary>
    /// If the entity is editable by GM
    /// </summary>
    public bool editableEntity = true;

    void Start() {
        // if(isNameKey && entityName.Equals("")) {
        //     entityName = LocalStrings.GetLocalString("entitiyTypeNames", dispName);
        // }
        customData = new Dictionary<string, string>();
        for(int i = 0; i < customDataArray.Length; i++) {
            customData.Add(customDataArray[i].key, customDataArray[i].value);
        }
        children = new List<string>();
        OnStart();
    }

    /// <summary>
    /// Called right after init of base entity
    /// </summary>
    public virtual void OnStart() {}
    /// <summary>
    /// Called right before the entity is stringified
    /// </summary>
    protected virtual void OnEntitySave() {}
    /// <summary>
    /// Called right after the entity is loaded from string
    /// </summary>
    protected virtual void OnEntityLoad() {}
    /// <summary>
    /// Called right before the entity is destroyed
    /// </summary>
    public virtual void OnDestory() {}
    /// <summary>
    /// Called when the position is set by an external source
    /// </summary>
    public virtual void OnSetPosition() {}

    /// <summary>
    /// Saves the entity to a JSON object
    /// </summary>
    /// <returns>JSON object representing the entity</returns>
    public override JsonObj Save() {
        OnEntitySave();
        
        JsonObj obj = new JsonObj();

        obj.AddKey("type", entityType);
        obj.AddKey("id", entityId);
        if(isNameKey) {
            obj.AddKey("localName", localName.toJson());
        } else {
            obj.AddKey("name", dispName);
        }
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

    /// <summary>
    /// Loads the entity from a JSON object
    /// </summary>
    /// <param name="obj">JSON object representing the entity</param>
    public override void Load(JsonObj obj) {
        
        if(obj.ContainsKey("type")) entityType = obj.GetString("type");
        if(obj.ContainsKey("id")) entityId = obj.GetString("id");
        if(obj.ContainsKey("name")) dispName = obj.GetString("name");
        if(obj.ContainsKey("localName")) {
            isNameKey = true;
            localName = new LocalString(obj.GetObj("localName"));
        }
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

    /// <summary>
    /// Sets the position of the entity, and calluses OnSetPosition()
    /// </summary>
    /// <param name="pos">The new position</param>
    public void SetPosition(Vector3 pos) {
        transform.position = pos;
        OnSetPosition();
    }
    /// <summary>
    /// Sets the position of the entity relative to it's parent
    /// </summary>
    /// <param name="pos">The new local position</param>
    public void SetLocalPosition(Vector3 pos) {
        // print("Test " + pos);
        transform.localPosition = pos;
        OnSetPosition();
    }

}

/// <summary>
/// Used to load a dictionary from JSON array
/// </summary>
[System.Serializable]
public class CustomData {
    public string key;
    public string value;
}