using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour {

    public float accl = 9.81f;
    public int priority = 0;

    private Dictionary<string, Entity> entities;

    private Vector3 force;

    void Start() {
        entities = new Dictionary<string, Entity>();
    }

    void Update() {
        if(entities.Count > 0) force = transform.up * -9.81f;
        foreach (Entity ent in entities.Values) {
            if(!ent.gravity) continue;
            if(ent.gravitySource != null && ent.gravitySource != this) {
                if(ent.gravitySource.priority >= priority) continue;
            }
            ent.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
            ent.gravitySource = this;
        }
    }

    void OnTriggerEnter(Collider other) {
        Interactable intr = Interactable.GetInteractable(other.gameObject);
        if(!(intr is Entity)) {
            return;
        }
        Entity ent = (Entity)intr;
        if(ent != null && ent.GetComponent<Rigidbody>() && !entities.ContainsKey(ent.entityId)) {
            entities.Add(ent.entityId, ent);
        }
    }
    
    void OnTriggerExit(Collider other){
        Interactable intr = Interactable.GetInteractable(other.gameObject);
        if(!(intr is Entity)) {
            return;
        }
        Entity ent = (Entity)intr;
        if(ent != null && entities.ContainsKey(ent.entityId)) {
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
