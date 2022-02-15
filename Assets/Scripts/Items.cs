using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

    public static Sprite missingSprite;
    private static Dictionary<string, Item> items;
    public static Item[] itemArray;
    public string[] itemLists;

    public void Awake() {
        items = new Dictionary<string, Item>();
        missingSprite = Resources.Load<Sprite>("Sprites/Items/Missing");
        // for(int i = 0; i < itemArray.Length; i++) {
        //     items.Add(itemArray[i].itemID, itemArray[i]);
        // }

        for(int l = 0; l < itemLists.Length; l++) {
            JsonObj obj = StringParser.ParseObject( SaveLoadData.LoadString("ItemLists/" + itemLists[l] + ".json") ).data;
            foreach (KeyValuePair<string, JsonObj> p in obj.GetObjs()) {
                JsonObj o = p.Value;
                Item i = new Item();
                i.itemID = p.Key;

                if(o.ContainsKey("icon")) {
                    string s = o.GetString("icon");
                    if(!s.Equals("n/a") ) {
                        i.icon = Resources.Load<Sprite>("Sprites/Items/" + s);
                    }
                }
                if(o.ContainsKey("icon3d")) {
                    JsonObj o2 = o.GetObj("icon3d");
                    i.icon3d = new Ico3d();

                    if(o2.ContainsKey("icon") && !o2.GetString("icon").Equals("n/a")) {
                        i.icon3d.icon = Resources.Load<GameObject>("Prefabs/Items/" + o2.GetString("icon"));
                    }
                    i.icon3d.posOffset = o2.GetVector3("pos");
                    i.icon3d.rotOffset = o2.GetVector3("rot");
                }

                // Debug.Log(i);
                items.Add(p.Key, i);
            }

        }
    }

    public static Sprite GetSprite(string id) {
        if(items.ContainsKey(id)) {
            Sprite sp = items[id].icon;
            if(sp != null) {
                return sp;
            }
        }
        // Debug.Log("Warn: Missing sprite for id:\"" + id + "\"");
        return missingSprite;
    }

    public static Ico3d GetIcon3d(string id) {
        if(items.ContainsKey(id)) {
            Ico3d ico = items[id].icon3d;
            if(ico == null) {
                return null;
            }
            if(ico.icon != null) {
                return ico;
            }
        }
        return null;
    }
}
