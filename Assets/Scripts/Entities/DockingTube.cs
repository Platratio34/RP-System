using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingTube : Interactable {

    [Header("Docking Ring")]
    public bool open;
    public bool extend;
    public bool manual;

    [Header("Floor")]
    [Range(0,3)]
    public int fPos;
    public ObjAnimator floorAnimator;

    [Header("Extender")]
    [Range(0,2)]
    public int exPos;
    public ObjAnimator ringAnimator;


    [Header("Door")]
    public bool door;
    public ObjAnimator doorAnimator;

    void Update() {
        // if(fT < fPos) {
        //     fT += Time.deltaTime;
        //     fT = Mathf.Min(fT, fPos);
        // } else if(fT > fPos) {
        //     fT -= Time.deltaTime;
        //     fT = Mathf.Max(fT, fPos);
        // }
        // if(fT <= 1) {
        //     floor0.transform.localPosition = MathP.VLerp(fPos0[0], fPos0[1], Mathf.Min(fT, 1) );
        //     floor1.transform.localPosition = MathP.VLerp(fPos1[0], fPos1[1], Mathf.Min(fT, 1) );
        //     floor2.transform.localPosition = MathP.VLerp(fPos2[0], fPos2[1], Mathf.Min(fT, 1) );
        // } else if(fT > 1 && fT <= 2) {
        //     floor0.transform.localPosition = fPos0[1];
        //     floor1.transform.localPosition = MathP.VLerp(fPos1[1], fPos1[2], Mathf.Max(Mathf.Min(fT - 1, 1), 0) );
        //     floor2.transform.localPosition = MathP.VLerp(fPos2[1], fPos2[2], Mathf.Max(Mathf.Min(fT - 1, 1), 0) );
        // } else {
        //     floor0.transform.localPosition = fPos0[1];
        //     floor1.transform.localPosition = fPos1[2];
        //     floor2.transform.localPosition = MathP.VLerp(fPos2[2], fPos2[3], Mathf.Max(Mathf.Min(fT - 2, 1), 0) );
        // }

        // if(exT < exPos) {
        //     exT += Time.deltaTime;
        //     exT = Mathf.Min(exT, exPos);
        // } else if(exT > exPos) {
        //     exT -= Time.deltaTime;
        //     exT = Mathf.Max(exT, exPos);
        // }
        // if(exT <= 1) {
        //     ex0.transform.localPosition = MathP.VLerp(exPos0[0], exPos0[1], Mathf.Min(exT, 1) );
        //     ex1.transform.localPosition = MathP.VLerp(exPos1[0], exPos1[1], Mathf.Min(exT, 1) );
        //     end.transform.localPosition = MathP.VLerp(exPosE[0], exPosE[1], Mathf.Min(exT, 1) );
        // } else if(exT > 1 && exT <= 2) {
        //     ex0.transform.localPosition = exPos0[1];
        //     ex1.transform.localPosition = MathP.VLerp(exPos1[1], exPos1[2], Mathf.Max(Mathf.Min(exT - 1, 1), 0) );
        //     end.transform.localPosition = MathP.VLerp(exPosE[1], exPosE[2], Mathf.Max(Mathf.Min(exT - 1, 1), 0) );
        // }

        // if(door) {
        //     dT += Time.deltaTime;
        //     dT = Mathf.Min(dT, 2);
        // } else {
        //     dT -= Time.deltaTime;
        //     dT = Mathf.Max(dT, 0);
        // }
        // end.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(0, 5.625f, dT / 2f), 90);
        // for(int i = 0; i < end.transform.childCount; i++) {
        //     Transform t = end.transform.GetChild(i).GetChild(0);
        //     t.localEulerAngles = new Vector3(0, Mathf.Lerp(0, 90, dT / 2f), 0);
        // }

        // if(open) {
        //     if(!extend) {
        //         door = true;
        //         fPos = 2;
        //         exPos = 2;
        //     } else {
        //         door = true;
        //         fPos = 3;
        //         exPos = 2;
        //     }
        // } else if(!manual) {
        //     extend = false;
        //     if(fT <= 2) {
        //         door = !(fT <= exT + 0.01f);
        //         fPos = 0;
        //         exPos = 0;
        //     } else {
        //         door = !(fT <= exT + 0.01f);
        //         fPos = 2;
        //         exPos = 2;
        //     }
        // }


        if(!manual) {
            if(open) {
                floorAnimator.target = extend?3f:2f;
            } else {
                floorAnimator.target = extend?3f:0f;
            }
            // floorAnimator.speed = ;
            ringAnimator.target = !(!open && ! extend)?2f:0f;
            // ringAnimator.speed = !(!open && ! extend)?1f:-1f;
            doorAnimator.target = ((open||extend)||door)?1f:0f;
        } else {
            floorAnimator.target = fPos;
            ringAnimator.target = exPos;
            doorAnimator.target = door?1f:0f;
        }

        eParams.GetParam("open").valueB = open;
        eParams.GetParam("extend").valueB = extend;
        eParams.GetParam("manual").valueB = manual;
        eParams.GetParam("floorPos").valueI = fPos;
        eParams.GetParam("ringPos").valueI = exPos;
        eParams.GetParam("door").valueB = door;
    }

    public override void OnInteract(bool gm) {
        open = !open;
        eParams.GetParam("open").valueB = open;
    }

    protected override void OnEdit() {
        open = eParams.GetParam("open").valueB;
        extend = eParams.GetParam("extend").valueB;
        manual = eParams.GetParam("manual").valueB;
        fPos = eParams.GetParam("floorPos").valueI;
        exPos = eParams.GetParam("ringPos").valueI;
        door = eParams.GetParam("door").valueB;
    }

    public override JsonObj Save() {
        JsonObj obj = new JsonObj();

        obj.AddBool("open", open);
        obj.AddBool("extend", extend);
        obj.AddBool("manual", manual);
        obj.AddFloat("fPos", fPos);
        obj.AddFloat("exPos", exPos);
        obj.AddBool("door", door);

        return obj;
    }

    public override void Load(JsonObj obj) {
        if(obj.ContainsKey("open")) open = obj.GetBool("open");
        if(obj.ContainsKey("extend")) extend = obj.GetBool("extend");
        if(obj.ContainsKey("manual")) manual = obj.GetBool("manual");
        if(obj.ContainsKey("fPos")) fPos = (int)obj.GetFloat("fpos");
        if(obj.ContainsKey("exPos")) exPos = (int)obj.GetFloat("exPos");
        if(obj.ContainsKey("door")) door = obj.GetBool("door");
    }


}
