using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameMaster : MonoBehaviour {
    
    public Entity[] entitiesArr;
    public Dictionary<string, Entity> entities;
    public InputField saveTo;
    public bool showDebugMessages = false;

    void Start() {
        entities = new Dictionary<string, Entity>();
        if(entitiesArr != null) {
            for(int i = 0; i < entitiesArr.Length; i++) {
                if(entitiesArr[i] != null) {
                    entities.Add(entitiesArr[i].entityId, entitiesArr[i]);
                }
            }
        }
        
        string s = SaveLoadData.LoadString("Saves/test.json");
        JsonObj o = StringParser.ParseObject(s).data;

        // LocalStrings.GetLocalStringD("test", "test", new string[]{"a"});
        // Load(o);

        // for(int i = 0; i < 8; i++) {
        //     print(GenerateUUID());
        // }
        
        // JsonObj obj = new JsonObj();
        // obj.AddVector3("test", 1, 2, 3);
        // obj.AddQuaternion("test2", 1, 2, 3, 4);
        // obj.AddFloat("test3", 123);
        // print(obj.ToString(true,""));
    }

    public void OnApplicationQuit() {
        SaveLoadData.SaveString("Saves/test3.json", Save().ToString(true,""));
    }

    void Update() {
        
    }

    public JsonObj Save() {
        JsonObj obj = new JsonObj();

        if(entities.Count > 0) {
            JsonObj ent = new JsonObj();
            foreach(KeyValuePair<string, Entity> e in entities) {
                ent.AddKey(e.Key, e.Value.Save());
            }
            obj.AddKey("entities", ent);
        }
        return obj;
    }

    public void Load(JsonObj obj) {
        if(obj.ContainsKey("entities")) {
            Queue<Entity> needParents = new Queue<Entity>();
            JsonObj ent = obj.GetObj("entities");
            
            foreach (KeyValuePair<string, Entity> e in entities) {
                if(ent.ContainsKey(e.Key)) {
                    e.Value.Load(ent.GetObj(e.Key) );
                    ent.Remove(e.Key);
                    Log("Reloaded entity id \"" + e.Value.entityId + "\", of type \"" + e.Value.entityType + "\"");
                }
            }
            foreach(KeyValuePair<string, JsonObj> p in ent.GetObjs()) {
                JsonObj o = p.Value;
                GameObject prefab = EntityIndex.GetPrefab(o.GetString("type"));
                if(prefab != null) {
                    GameObject e = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    Entity en = e.GetComponent<Entity>();
                    en.Load(o);
                    entities.Add(en.entityId, en);
                    if(en.parentEntity != "") {
                        needParents.Enqueue(en);
                        // print("Enti");
                    }
                    Log("Loaded entity id \"" + o.GetString("id") + "\", of type \"" + o.GetString("type") + "\"");
                } else {
                    Debug.LogError("Failed to load entiy id \"" + o.GetString("id") + "\", of type \"" + o.GetString("type") + "\". Type or prefab was missing");
                }
            }

            while(needParents.Count > 0) {
                Entity e = needParents.Dequeue();
                if(entities.ContainsKey(e.parentEntity)) {
                    entities[e.parentEntity].children.Add(e.entityId);
                    // e.parentEntity = entities
                    e.transform.SetParent(entities[e.parentEntity].transform);
                } else {
                    Debug.LogError("No entity to parent to (" + e.entityId + " => " + e.parentEntity + ")");
                }
            }
        }
    }

    public void LoadSave() {
        string filePath = "Saves/" + saveTo.text + ".json";
        JsonObj o = StringParser.ParseObject( SaveLoadData.LoadString(filePath) ).data;
        Load(o);
        Log("Loaded game from " + filePath);
    }
    public void SaveSave() {
        string filePath = "Saves/" + saveTo.text + ".json";
        SaveLoadData.SaveString(filePath, Save().ToString(true,""));
        Log("Saved game to " + filePath);
    }

    public void DestroyEntity(string id) {
        Entity e = entities[id];
        e.OnDestory();
        entities.Remove(id);
        Destroy(e.gameObject);
    }

    public void SpawnEntity(string type, string id) {
        if(!entities.ContainsKey(id)) {
            GameObject prefab = EntityIndex.GetPrefab(type);
            if(prefab != null) {
                GameObject e = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                Entity en = e.GetComponent<Entity>();
                en.entityId = id;
                en.entityType = type;
                en.entityName = id;
                en.OnStart();
                entities.Add(en.entityId, en);
                Log("Spawned entity id \"" + id + "\", of type \"" + type + "\"");
            } else {
                Debug.LogError("Failed to spawn entiy id \"" + id + "\", of type \"" + type + "\". Type or prefab was missing");
                return;
            }
        } else {
            Debug.LogError("Failed to spawn entiy id \"" + id + "\", of type \"" + type + "\". Entity id already exited");
            return;
        }
    }
    public string SpawnEntity(string type) {
        if(EntityIndex.HasEntity(type)) {
            string id = GenerateUUID();
            SpawnEntity(type, id);
            return id;
        } else {
            return "";
        }
    }

    public Entity GetEntity(string id) {
        if(entities.ContainsKey(id)) {
            return entities[id];
        } else {
            return null;
        }
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void Log(string s) {
        if(showDebugMessages) {
            print(s);
        }
    }

    public string GenerateUUID() {
        string uUID = "";
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        do {
            for(int i = 0; i < 8; i++) {
                uUID += glyphs[Random.Range(0, glyphs.Length)];
            }
        } while( entities.ContainsKey(uUID) );

        return uUID;
    }
}
