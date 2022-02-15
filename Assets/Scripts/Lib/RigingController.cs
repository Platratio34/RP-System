using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CatchCo;

// [ExecuteInEditMode]
public class RigingController : MonoBehaviour {
    
    public RigPose[] poses;
    public GameObject[] bones;
    public float speed = 1f;
    private float pos = 0f;
    private RigPose oP;
    private RigPose nP;
    public string newPoseName;
    public GameObject parentBone;

    void Start() {
        // poses = new RigPose[0];
        oP = poses[0];
        nP = poses[0];
    }

    void Update() {
        if(oP != nP && nP != null) {
            pos += Time.deltaTime * speed;
            pos = MathP.Clamp(pos, 0f, 1f);
            for(int i = 0; i < nP.bones.Length; i++) {
                int id = nP.bones[i].boneID;
                if(bones[id].transform.eulerAngles == null) {
                    Debug.LogError("Bone null");
                }
                bones[id].transform.localRotation = Quaternion.Lerp(oP.bones[i].rot, nP.bones[i].rot, pos);
            }
            if(oP.pBonePos != null && nP.pBonePos != null) {
                parentBone.transform.localPosition = Vector3.Lerp(oP.pBonePos, nP.pBonePos, pos);
            } else if(nP.pBonePos != null) {
                parentBone.transform.localPosition = nP.pBonePos;
            }
            if(pos >= 1f) {
                oP = nP;
            }
        } else if(nP == null) {
            nP = oP;
            Debug.LogError("Couldn't load pose, new pose was null; reseting to pose \"" + oP.name + "\"");
        }
    }

    [ExposeMethodInEditor]
    public void PopulateBones(GameObject parent) {
        List<GameObject> l = new List<GameObject>();
        l.Add(parent);
        PopulateBones(parent.transform, l);
        bones = l.ToArray();
    }
    public void PopulateBones(Transform t, List<GameObject> l) {
        for(int i = 0; i < t.childCount; i++) {
            Transform b = t.GetChild(i);
            l.Add(b.gameObject);
            PopulateBones(b, l);
        }
    }

    // [ExposeMethodInEditor]
    public void SavePose(string name) {
        RigPose p = MakePose(name);
        AddPose(p);
        oP = p;
    }

    private RigPose MakePose(string name) {
        if(bones == null || bones.Length == 0) {
            Debug.LogError("Couldn't Save Pose, no bones to save");
            return null;
        }
        RigPose p = new RigPose(name);
        p.bones = new RigBonePose[bones.Length];
        p.pBonePos = parentBone.transform.localPosition;
        for(int i = 0; i < bones.Length; i++) {
            p.bones[i] = new RigBonePose(i, bones[i].transform.localRotation);
        }
        return p;
    }

    public void AddPose(RigPose pose) {
        if(poses == null || poses.Length == 0) {
            poses = new RigPose[1];
            poses[0] = pose;
        } else {
            RigPose[] pA = new RigPose[poses.Length + 1];
            Array.Copy(poses, 0, pA, 0, poses.Length);
            for(int i = 0; i < poses.Length; i++) {
                if(poses[i] != null) {
                    if(name.Equals(poses[i].name)) {
                        poses[i] = pose;
                        return;
                    }
                } else {
                    Debug.LogError("Couldn't check for overwrite, pose not created, " + i);
                }
            }
            pA[pA.Length - 1] = pose;
            poses = pA;
        }
    }

    public void UpdatePose(int index) {
        RigPose p = poses[index];
        p.bones = new RigBonePose[bones.Length];
        p.pBonePos = parentBone.transform.localPosition;
        for(int i = 0; i < bones.Length; i++) {
            p.bones[i] = new RigBonePose(i, bones[i].transform.localRotation);
        }
    }

    public void RecalPose(int index) {
        if(poses != null) {
            RecalPose(poses[index]);
        }
    }
    public void RecalPose(string name) {
        if(poses != null) {
            RecalPose(GetPose(name));
        }
    }
    public void RecalPose(RigPose p) {
        if(!Application.IsPlaying(this)) {
            // Debug.Log("Recaled Pose \"" + p.name + "\"");
            for(int i = 0; i < p.bones.Length; i++) {
                bones[p.bones[i].boneID].transform.localRotation = p.bones[i].rot;
            }
            if(p.pBonePos != null) {
                parentBone.transform.localPosition = p.pBonePos;
            }
            oP = p;
            nP = p;
            pos = 1f;
        } else {
            if(pos < 1f) {
                oP = MakePose("Current");
            }
            pos = 0f;
            nP = p;
        }
    }

    public void RemovePose(int index) {
        if(poses != null && index < poses.Length) {
            int i = 0;
            RigPose[] pA = new RigPose[poses.Length - 1];
            while(i < poses.Length && i != index) {
                pA[i] = poses[i];
                i++;
            }
            i++;
            while(i < poses.Length) {
                pA[i - 1] = poses[i];
                i++;
            }
            poses = pA;
        }
    }
    public void RemovePose(string name) {
        if(poses != null && GetPose(name) != null) {
            int i = 0;
            RigPose[] pA = new RigPose[poses.Length - 1];
            while(i < poses.Length && !poses[i].name.Equals(name)) {
                pA[i] = poses[i];
                i++;
            }
            i++;
            while(i < poses.Length) {
                pA[i - 1] = poses[i];
                i++;
            }
            poses = pA;
        }
    }

    public RigPose GetPose(string name) {
        for(int i = 0; i < poses.Length; i++) {
            if(poses[i].name.Equals(name)) {
                return poses[i];
            }
        }
        return null;
    }

    public void SavePoseToFile(int index, string filename) {
        filename = "Poses/" + filename + ".json";
        if(index >= poses.Length) {
            Debug.LogError("Pose index out of bounds; index:" + index + ", max:" + (poses.Length - 1) + ", filename:\"" + filename + "\"");
            return;
        }
        JsonObj o = poses[index].Save();
        Debug.Log("Saveing pose to file:\"" + filename + "\"");
        SaveLoadData.SaveString(o.ToString(true, ""), filename);
    }
    public void LoadFromFile(string filename) {
        AddPose( new RigPose( StringParser.ParseObject( SaveLoadData.LoadString( "Poses/" + filename + ".json" ) ).data ) );
    }

    public string GetCurrentPoseName() {
        if(oP == null || oP.name == null) {
            return "null";
        }
        return oP.name;
    }

    public string GetNextPoseName() {
        if(nP == null || nP.name == null) {
            return "null";
        }
        return nP.name;
    }
}

[System.Serializable]
public class RigPose {
    public string name;
    public RigBonePose[] bones;
    public Vector3 pBonePos;

    public RigPose(string name) {
        this.name = name;
    }
    public RigPose(JsonObj obj) {
        name = obj.GetString("name");
        bones = new RigBonePose[obj.array.Length];
        for(int i = 0; i < obj.array.Length; i++) {
            bones[i] = new RigBonePose(obj.array[i]);
        }
    }

    public override bool Equals(object obj) {
        if(obj.GetType() != typeof(RigPose) ) return false;
        RigPose o = (RigPose)obj;
        if(!o.name.Equals(name)) return false;
        return true;
    }
    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public JsonObj Save() {
        JsonObj o = new JsonObj();
        o.AddKey("name", name);
        JsonObj[] bs = new JsonObj[bones.Length];
        for(int i = 0; i < bones.Length; i++) {
            bs[i] = bones[i].Save();
        }
        o.AddKey("bones", bs);
        return o;
    }
}

[System.Serializable]
public class RigBonePose {
    public int boneID;
    public Quaternion rot;

    public RigBonePose(int boneID, Quaternion rot) {
        this.boneID = boneID;
        this.rot = rot;
    }
    public RigBonePose(JsonObj obj) {
        // boneID = (int)obj.GetFloat("id");
        // rot = obj.GetVector3("rot");
    }

    public JsonObj Save() {
        JsonObj o = new JsonObj();
        // o.AddFloat("id", boneID);
        // o.AddVector3("rot", rot);
        return o;
    }
}

[System.Serializable]
public class RigBone {
    public GameObject bone;

    public RigBone(GameObject bone) {
        this.bone = bone;
    }
}