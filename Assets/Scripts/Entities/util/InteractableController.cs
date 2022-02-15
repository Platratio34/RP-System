using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class InteractableController : MonoBehaviour {
    
    public Interactable[] interactables;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public JsonObj Save() {
        JsonObj obj = new JsonObj();
        obj.array = new JsonObj[interactables.Length];
        for(int i = 0; i < interactables.Length; i++) {
            obj.array[i] = interactables[i].Save();
        }

        return obj;
    }
    
    public void Load(JsonObj obj) {
        for(int i = 0; i < interactables.Length; i++) {
            interactables[i].Load(obj.array[i]);
        }
    }

    [ExposeMethodInEditor]
    public void Populate() {
        List<Interactable> list = new List<Interactable>();
        for(int i = 0; i < transform.childCount; i++) {
            Transform t = transform.GetChild(i); {
                Interactable inter = t.GetComponent<Interactable>();
                if(inter != null) {
                    list.Add(inter);
                } else {
                    for(int i2 = 0; i2 < t.childCount; i2++) {
                        Transform t2 = t.GetChild(i2); {
                            inter = t2.GetComponent<Interactable>();
                            if(inter != null) {
                                list.Add(inter);
                            }
                        }
                    }
                }
            }
        }
        interactables = list.ToArray();
    }
}
