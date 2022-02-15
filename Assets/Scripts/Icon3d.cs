using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon3d : MonoBehaviour {
    
    private GameObject icon;
    private string n;
    void Start() {
        
    }

    void Update() {
        
    }

    public void SetIcon(GameObject g, Vector3 pO, Vector3 rO, string n) {
        if(icon == null) {
            this.n = n;
            icon = Instantiate(g, transform.position, transform.rotation, transform);
            icon.transform.localScale = new Vector3(icon.transform.localScale.x*100, icon.transform.localScale.y*100, icon.transform.localScale.z*100);
            icon.transform.localPosition = pO;
            icon.transform.localEulerAngles = rO;
            SetLayer(LayerMask.NameToLayer("UI"), icon.gameObject);
        } else if(this.n != n) {
            Destroy(icon);
            this.n = n;
            icon = Instantiate(g, transform.position, transform.rotation, transform);
            icon.transform.localScale = new Vector3(icon.transform.localScale.x*100, icon.transform.localScale.y*100, icon.transform.localScale.z*100);
            icon.transform.localPosition = pO;
            icon.transform.localEulerAngles = rO;
            // icon.gameObject.layer = LayerMask.NameToLayer("UI");
            SetLayer(LayerMask.NameToLayer("UI"), icon.gameObject);
        }
    }

    public void RemoveIcon() {
        Destroy(icon);
        n = "";
    }

    private void SetLayer(int l, GameObject gO) {
        gO.layer = l;
        for(int i = 0; i < gO.transform.childCount; i++) {
            SetLayer(l, gO.transform.GetChild(i).gameObject);
        }
    }
}
