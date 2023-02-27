using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour {

    public float accl = 9.81f;
    public int priority = 0;

    private Dictionary<string, Entity> entities;

    void Start() {
        entities = new Dictionary<string, Entity>();
    }

    void Update() {
        foreach (Entity ent in entities.Values) {
            if(!ent.gravity) continue;
            if(ent.gravitySource != null && ent.gravitySource != this) {
                if(ent.gravitySource.priority >= priority) continue;
            }
            ent.GetComponent<Rigidbody>().AddForce(transform.up*-9.81f, ForceMode.Acceleration);
            ent.gravitySource = this;
        }
    }

    void OnTriggerEnter(Collider other) {
        Entity ent = (Entity)Interactable.GetInteractable(other.gameObject);
        if(ent != null) {
            entities.Add(ent.entityId, ent);
        }
    }
    
    void OnTriggerExit(Collider other){
        Entity ent = (Entity)Interactable.GetInteractable(other.gameObject);
        if(ent != null) {
            entities.Remove(ent.entityId);
            if(ent.gravitySource == this) {
                ent.gravitySource = null;
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * -0.1f * accl) );
    }
}
