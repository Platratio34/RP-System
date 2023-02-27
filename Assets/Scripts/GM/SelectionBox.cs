using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SelectionBox : MonoBehaviour {

    public GameMaster gameMaster;

    public LineRenderer lower;
    public LineRenderer upper;
    public LineRenderer side0;
    public LineRenderer side1;
    public LineRenderer side2;
    public LineRenderer side3;
    public GameObject box;

    public bool show;
    private bool lShow;

    void Start() {
        lShow = show;
        upper.enabled = show;
        lower.enabled = show;
        side0.enabled = show;
        side1.enabled = show;
        side2.enabled = show;
        side3.enabled = show;
        box.SetActive(show);
    }

    private Vector3[] cubePoints = new Vector3[] {
        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, -0.5f, -0.5f),
        new Vector3(-0.5f, 0.5f, -0.5f),
        new Vector3(-0.5f, 0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, -0.5f),
    };

    void Update() {
        if(show == lShow) return;
        lShow = show;
        upper.enabled = show;
        lower.enabled = show;
        side0.enabled = show;
        side1.enabled = show;
        side2.enabled = show;
        side3.enabled = show;
        box.SetActive(show);
    }

    public void DrawSelBox(Transform transform, string entityType, Color c) {
        DefaultEntity dE = EntityIndex.GetEntity(entityType);
        if(dE == null) {
            return;
        }
        Vector3 offset = Vector3.zero;
        offset += dE.selBoxOffset.z * transform.forward;
        offset += dE.selBoxOffset.y * transform.up;
        offset += dE.selBoxOffset.x * transform.right;
        this.transform.position = transform.position + offset;
        this.transform.localScale = dE.selBoxScale;
        this.transform.rotation = transform.rotation;
        // Vector3[] p = new Vector3[8];
        // for(int i = 0; i < p.Length; i++) {
        //     p[i] = (cubePoints[i].x * dE.selBoxScale.x) * transform.right;
        //     p[i] += (cubePoints[i].y * dE.selBoxScale.y) * transform.up;
        //     p[i] += (cubePoints[i].z * dE.selBoxScale.z) * transform.forward;
        //     p[i] += center;
        // }
        // lower.SetPositions(new Vector3[] {p[0], p[1], p[2], p[3]});
        // upper.SetPositions(new Vector3[] {p[4], p[5], p[6], p[7]});
        // side0.SetPositions(new Vector3[] {p[0], p[4]});
        // side1.SetPositions(new Vector3[] {p[1], p[5]});
        // side2.SetPositions(new Vector3[] {p[2], p[6]});
        // side3.SetPositions(new Vector3[] {p[3], p[7]});

        lower.startColor = c;
        upper.startColor = c;
        side0.startColor = c;
        side1.startColor = c;
        side2.startColor = c;
        side3.startColor = c;
        lower.endColor = c;
        upper.endColor = c;
        side0.endColor = c;
        side1.endColor = c;
        side2.endColor = c;
        side3.endColor = c;
    }
    // public void DrawSelBox(Transform transform, Interactable interactable, Color c) {
    //     Vector3 offset = Vector3.zero;
    //     offset += interactable.selBoxOffset.z * transform.forward;
    //     offset += interactable.selBoxOffset.y * transform.up;
    //     offset += interactable.selBoxOffset.x * transform.right;
    //     this.transform.position = transform.position + offset;
    //     this.transform.localScale = interactable.selBoxScale;
    //     this.transform.rotation = transform.rotation;

    //     lower.startColor = c;
    //     upper.startColor = c;
    //     side0.startColor = c;
    //     side1.startColor = c;
    //     side2.startColor = c;
    //     side3.startColor = c;
    //     lower.endColor = c;
    //     upper.endColor = c;
    //     side0.endColor = c;
    //     side1.endColor = c;
    //     side2.endColor = c;
    //     side3.endColor = c;
    // }
}
