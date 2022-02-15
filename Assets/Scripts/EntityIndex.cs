using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIndex : MonoBehaviour {
    
    private static Dictionary<string,DefaultEntity> entities;
    public string[] entitylists;

    void Awake() {
        entities = new Dictionary<string, DefaultEntity>();
        entities.Add("missing", new DefaultEntity(Resources.Load<GameObject>("Prefabs/Entities/Missing"), "missing", Vector3.zero, Vector3.one, false) );
        // for(int i = 0; i < entitiesArr.Length; i++) {
        //     entities.Add(entitiesArr[i].type, entitiesArr[i]);
        // }

        for(int i = 0; i < entitylists.Length; i++) {
            JsonObj list = StringParser.ParseObject(SaveLoadData.LoadString("EntityLists/" + entitylists[i] + ".json")).data;
            foreach(KeyValuePair<string, JsonObj> p in list.GetObjs()) {
                DefaultEntity de = new DefaultEntity();
                JsonObj o = p.Value;
                de.type = p.Key;
                if(o.ContainsKey("spawable")) {
                    de.spawnable = o.GetBool("spawable");
                }
                if(o.ContainsKey("selbox")) {
                    JsonObj sb = o.GetObj("selbox");
                    de.selBoxOffset = sb.GetVector3("offset");
                    de.selBoxScale = sb.GetVector3("scale");
                }
                if(o.ContainsKey("prefab")) {
                    de.prefab = Resources.Load<GameObject>("Prefabs/Entities/" + o.GetString("prefab"));
                }

                entities.Add(p.Key, de);
            }
        }

    }

    void Update() {
        
    }

    public static DefaultEntity GetEntity(string type) {
        if(entities != null && entities.ContainsKey(type)) {
            return entities[type];
        } else {
            Debug.LogError("Atemted to get default entity of type: \"" + type + "\", no entity found, returning MissingEntity");
            return entities["missing"];
        }
    }
    public static GameObject GetPrefab(string type) {
        if(entities != null && entities.ContainsKey(type)) {
            return entities[type].prefab;
        } else {
            Debug.LogError("Atemted to get prefab of type: \"" + type + "\", no entity found, returning MissingEntity prefab");
            return entities["missing"].prefab;
        }
    }

    public static bool HasEntity(string type) {
        return entities.ContainsKey(type);
    }

    public static string[] GetEntityTypes() {
        string[] sa = new string[entities.Count];
        entities.Keys.CopyTo(sa, 0);
        return sa;
    }

    public static bool IsSpawnable(string type) {
        if(entities.ContainsKey(type)) {
            return entities[type].spawnable;
        }
        Debug.LogError("Atemted to check if entity of type: \"" + type + "\" is spawnable, no entity found");
        return false;
    }


}

[System.Serializable]
public class DefaultEntity {
    public GameObject prefab;
    public string type;
    public Vector3 selBoxOffset;
    public Vector3 selBoxScale;
    public bool spawnable;

    public DefaultEntity() {}
    public DefaultEntity(GameObject prefab, string type, Vector3 selBoxOffset, Vector3 selBoxScale, bool spawnable) {
        this.prefab = prefab;
        this.type = type;
        this.selBoxOffset = selBoxOffset;
        this.selBoxScale = selBoxScale;
        this.spawnable = spawnable;
    }
}