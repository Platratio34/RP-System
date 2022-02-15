using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GmAble : MonoBehaviour {

    public Entity selEntity;
    private Interactable selInter;
    private Interactable selInterO;
    public Camera cam;
    public InvDisp invDisp1;
    public InvDisp invDisp2;
    public GameMaster gameMaster;

    [Header("Sel obj")]
    public string enityName = "";
    public Vector3 moveOffset;
    private Vector3 minMoveOffet;
    public bool moving = false;
    private float mD = 0;
    public float moveDelay = 1;
    private Vector3 selPos;
    public SelectionBox selectionBox;
    public Popup popup;
    public Popup popupT;
    public Dropdown entityList;
    public bool eListActive = false;
    private List<string> eTypes;

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

    void Start() {
        entityList.options = new List<Dropdown.OptionData>();
        eTypes = new List<string>();
        string[] sA = EntityIndex.GetEntityTypes();
        for(int i = 0; i < sA.Length; i++) {
            if(EntityIndex.IsSpawnable(sA[i])) {
                Dropdown.OptionData d = new Dropdown.OptionData();
                d.text = LocalStrings.GetLocalString("entitiyTypeNames", sA[i]);
                eTypes.Add(sA[i]);
                entityList.options.Add(d);
            }
        }
        entityList.gameObject.SetActive(false);
        eListActive = false;
    }

    void Update() {
        bool isNotOverUI = !GameMaster.IsPointerOverUIObject();
        if(Input.GetButtonDown("GM Select Entity") && isNotOverUI) {
            if(selInterO != null) {
                selInterO.UnSelect();
            }
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Entity o = selEntity;
            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                selEntity = objectHit.GetComponent<Entity>();
                if(selEntity != null) {
                    if(!selEntity.editableEntity) {
                        selEntity = null;
                        selInter = null;
                    } else {
                        selInter = (Interactable)selEntity;
                    }
                } else {
                    ChildCollider c = hit.transform.GetComponent<ChildCollider>();
                    if(c != null) {
                        if(c.entity != null) {
                            if(c.entity.editableEntity) {
                                selEntity = c.entity;
                            }
                        } else {
                            selInter = c.interactable;
                        }
                    } else {
                        Interactable i = hit.transform.GetComponent<Interactable>();
                        if(i != null) {
                            selInter = i;
                        }
                    }
                }
            }
            if(selEntity != o) {
                popup.Hide();
                popupT.Hide();
            }
        }
        if(Input.GetButtonDown("Cancel")) {
            selEntity = null;
            selInter = null;
            popup.Hide();
            popupT.Hide();
            entityList.gameObject.SetActive(false);
            eListActive = false;
        }

        if(Input.GetButtonDown("GM Entity Spawn")) {
            if(!eListActive) {
                entityList.gameObject.SetActive(true);
                eListActive = true;
                selEntity = null;
                selInter = null;
            } else {
                entityList.gameObject.SetActive(false);
                eListActive = false;
            }
        }

        if(selEntity != null) {
            enityName = selEntity.entityName;

            selEntity.Select();
            // selectionBox.DrawSelBox(selEntity.transform, selEntity, Color.blue);
            // selectionBox.show = true;

            if(popup.shown) {
                string[] strs = selEntity.GetParams();
                for(int i = 0; i < strs.Length; i++) {
                    if(selEntity.GetParamType(strs[i]) == ParamType.BOOL) {
                        popup.SetValue(strs[i], selEntity.GetParamB(strs[i]) );
                    } else if(selEntity.GetParamType(strs[i]) == ParamType.STRING){
                        popup.SetValue(strs[i], selEntity.GetParamS(strs[i]));
                    } else if(selEntity.GetParamType(strs[i]) == ParamType.FLOAT) {
                        popup.SetValue(strs[i], selEntity.GetParamF(strs[i]));
                    } else if(selEntity.GetParamType(strs[i]) == ParamType.INT) {
                        popup.SetValue(strs[i], selEntity.GetParamI(strs[i]));
                    }
                    if(selEntity.IsParameterRanged(strs[i])) {
                        popup.SetValueRange(strs[i], selEntity.GetParamRange(strs[i]));
                    }
                    popup.SetValueSlid(strs[i], selEntity.IsParamSlid(strs[i]));
                }
            }
            if(popupT.shown) {
                popupT.SetValue("static", selEntity.staticEntity);
                popupT.SetValue("px", selEntity.transform.localPosition.x + "");
                popupT.SetValue("py", selEntity.transform.localPosition.y + "");
                popupT.SetValue("pz", selEntity.transform.localPosition.z + "");
                popupT.SetValue("rx", selEntity.transform.localEulerAngles.x + "");
                popupT.SetValue("ry", selEntity.transform.localEulerAngles.y + "");
                popupT.SetValue("rz", selEntity.transform.localEulerAngles.z + "");
            }

            if(Input.GetButtonDown("GM Entity Delete")) {
                gameMaster.DestroyEntity(selEntity.entityId);
            } else if(Input.GetButtonDown("GM Entity Inventory")) {
                if(selEntity.inventory != null) {
                    if(invDisp1.hiden) {
                        invDisp1.SetInv(selEntity.inventory);
                    } else {
                        invDisp2.SetInv(selEntity.inventory);
                    }
                }
            } else if(Input.GetButtonDown("GM Entity Properties")) {
                if(!popup.shown) {
                    if(selEntity.eParams.parameters.Length > 0) {
                        popup.Reset();
                        popup.PopUp(enityName);
                        string[] strs = selEntity.GetParams();
                        for(int i = 0; i < strs.Length; i++) {
                            if(selEntity.GetParamType(strs[i]) == ParamType.BOOL) {
                                popup.AddValue(strs[i], LocalStrings.GetLocalString("parameterNames", strs[i]), selEntity.GetParamB(strs[i]) );
                            } else if(selEntity.GetParamType(strs[i]) == ParamType.STRING){
                                popup.AddValue(strs[i], LocalStrings.GetLocalString("parameterNames", strs[i]), selEntity.GetParam(strs[i], selEntity.GetParamType(strs[i]) ), true);
                            } else if(selEntity.GetParamType(strs[i]) == ParamType.FLOAT) {
                                popup.AddValue(strs[i], LocalStrings.GetLocalString("parameterNames", strs[i]), selEntity.GetParamF(strs[i]), true);
                            } else if(selEntity.GetParamType(strs[i]) == ParamType.INT) {
                                popup.AddValue(strs[i], LocalStrings.GetLocalString("parameterNames", strs[i]), selEntity.GetParamI(strs[i]), true);
                            }
                        }
                    } else {
                        popup.Hide();
                    }
                } else {
                    popup.Hide();
                }
            } else if(Input.GetButtonDown("GM Entity Tranform")) {
                if(!popupT.shown) {
                    popupT.Reset();
                    popupT.PopUp(enityName);
                    popupT.AddValue("static", LocalStrings.GetLocalString("parameterNames", "transform.static"), selEntity.staticEntity);
                    popupT.AddValue("px", LocalStrings.GetLocalString("parameterNames", "transform.px"), selEntity.transform.localPosition.x, true);
                    popupT.AddValue("py", LocalStrings.GetLocalString("parameterNames", "transform.py"), selEntity.transform.localPosition.y, true);
                    popupT.AddValue("pz", LocalStrings.GetLocalString("parameterNames", "transform.pz"), selEntity.transform.localPosition.z, true);
                    popupT.AddValue("rx", LocalStrings.GetLocalString("parameterNames", "transform.rx"), selEntity.transform.localEulerAngles.x, true);
                    popupT.AddValue("ry", LocalStrings.GetLocalString("parameterNames", "transform.ry"), selEntity.transform.localEulerAngles.y, true);
                    popupT.AddValue("rz", LocalStrings.GetLocalString("parameterNames", "transform.rz"), selEntity.transform.localEulerAngles.z, true);
                } else {
                    popupT.Hide();
                }
            } else if(Input.GetButtonDown("Interact")) {
                selEntity.OnInteract(true);
            }
            if(!selEntity.staticEntity) {
                if(Input.GetButton("GM Select Entity")) {
                    mD += Time.deltaTime;
                    if(mD >= moveDelay) {
                        if(!moving) {
                            moving = true;
                            RaycastHit hitInfo = new RaycastHit();
                            Physics.Raycast(selEntity.transform.position, -selEntity.transform.up, out hitInfo, 20f);
                            moveOffset = hitInfo.distance * (-selEntity.transform.up);
                            minMoveOffet = (-selEntity.transform.up) * ((EntityIndex.GetEntity(selEntity.entityType).selBoxScale.y / 2f) - EntityIndex.GetEntity(selEntity.entityType).selBoxOffset.y);
                        } else {
                            selEntity.GetComponent<Collider>().enabled = false;
                            if(Input.GetButton("GM Entity Change Alt")) {
                                float n = -Input.GetAxis("GM Cam Look Up/Down") / 10f;
                                moveOffset += selEntity.transform.up * n;
                                moveOffset = new Vector3(Mathf.Min(moveOffset.x, minMoveOffet.x), Mathf.Min(moveOffset.y, minMoveOffet.y), Mathf.Min(moveOffset.z, minMoveOffet.z));
                            } else {
                                RaycastHit hit;
                                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                                if (Physics.Raycast(ray, out hit)) {
                                    selPos = hit.point;
                                }
                            }
                            selEntity.transform.position = selPos - moveOffset;
                        }
                    }
                } else {
                    if(moving) {
                        selEntity.SetPosition(selPos - moveOffset);
                        selEntity.GetComponent<Collider>().enabled = true;
                        moving = false;
                    }
                    mD = 0;
                }
            }
        } else if(selInter != null) {
            // selectionBox.DrawSelBox(selInter.transform, selInter, Color.blue);
            // selectionBox.show = true;
            selInter.Select();

            if(popup.shown) {
                string[] strs = selInter.GetParams();
                for(int i = 0; i < strs.Length; i++) {
                    if(selInter.GetParamType(strs[i]) == ParamType.BOOL) {
                        popup.SetValue(strs[i], selInter.GetParamB(strs[i]) );
                    } else if(selInter.GetParamType(strs[i]) == ParamType.STRING){
                        popup.SetValue(strs[i], selInter.GetParamS(strs[i]));
                    } else if(selInter.GetParamType(strs[i]) == ParamType.FLOAT) {
                        popup.SetValue(strs[i], selInter.GetParamF(strs[i]));
                    } else if(selInter.GetParamType(strs[i]) == ParamType.INT) {
                        popup.SetValue(strs[i], selInter.GetParamI(strs[i]));
                    }
                    if(selInter.IsParameterRanged(strs[i])) {
                        popup.SetValueRange(strs[i], selInter.GetParamRange(strs[i]));
                    }
                    popup.SetValueSlid(strs[i], selInter.IsParamSlid(strs[i]));
                }
            }

            if(Input.GetButtonDown("Interact")) {
                selInter.OnInteract(true);
            } else if(Input.GetButtonDown("GM Entity Properties")) {
                if(!popup.shown) {
                    if(selInter.eParams.parameters.Length > 0) {
                        popup.Reset();
                        popup.PopUp(selInter.isNameKey ? LocalStrings.GetLocalString("interactableNames", selInter.dispName) : selInter.dispName);
                        string[] strs = selInter.GetParams();
                        for(int i = 0; i < strs.Length; i++) {
                            if(selInter.GetParamType(strs[i]) == ParamType.BOOL) {
                                popup.AddValue(strs[i], LocalStrings.GetLocalString("parameterNames", strs[i]), selInter.GetParamB(strs[i]) );
                            } else if(selInter.GetParamType(strs[i]) == ParamType.STRING){
                                popup.AddValue(strs[i], LocalStrings.GetLocalString("parameterNames", strs[i]), selInter.GetParam(strs[i], selInter.GetParamType(strs[i]) ), true);
                            } else if(selInter.GetParamType(strs[i]) == ParamType.FLOAT) {
                                popup.AddValue(strs[i], LocalStrings.GetLocalString("parameterNames", strs[i]), selInter.GetParamF(strs[i]), true);
                            } else if(selInter.GetParamType(strs[i]) == ParamType.INT) {
                                popup.AddValue(strs[i], LocalStrings.GetLocalString("parameterNames", strs[i]), selInter.GetParamI(strs[i]), true);
                            }
                        }
                    } else {
                        popup.Hide();
                    }
                } else {
                    popup.Hide();
                }
            }
        } else {
            popup.Hide();
            popupT.Hide();
            selectionBox.show = false;
            if(selInterO != null) {
                selInterO.UnSelect();
            }
        }
        selInterO = selInter;
    }

    public void OnPopupEdit() {
        if(selEntity != null) {
            for(int i = 0; i < popup.values.Length; i++) {
                if(!popup.values[i].shown) {
                    return;
                }

                if(popup.values[i].type == ParamType.BOOL) {
                    selEntity.SetParameter(popup.values[i].key, popup.values[i].GetValueB());
                } else if(popup.values[i].type == ParamType.FLOAT) {
                    selEntity.SetParameter(popup.values[i].key, popup.values[i].GetValueF());
                } else if(popup.values[i].type == ParamType.INT) {
                    selEntity.SetParameter(popup.values[i].key, popup.values[i].GetValueI());
                } else if(popup.values[i].type == ParamType.STRING) {
                    selEntity.SetParameter(popup.values[i].key, popup.values[i].GetValue());
                }
            }
        } else if(selInter != null) {
            for(int i = 0; i < popup.values.Length; i++) {
                if(!popup.values[i].shown) {
                    return;
                }

                if(popup.values[i].type == ParamType.BOOL) {
                    selInter.SetParameter(popup.values[i].key, popup.values[i].GetValueB());
                } else if(popup.values[i].type == ParamType.FLOAT) {
                    selInter.SetParameter(popup.values[i].key, popup.values[i].GetValueF());
                } else if(popup.values[i].type == ParamType.INT) {
                    selInter.SetParameter(popup.values[i].key, popup.values[i].GetValueI());
                } else if(popup.values[i].type == ParamType.STRING) {
                    selInter.SetParameter(popup.values[i].key, popup.values[i].GetValue());
                }
            }
        }
    }

    public void OnPopupTEdit() {
        if(selEntity != null) {
            selEntity.staticEntity = bool.Parse(popupT.GetValue("static"));
            Vector3 p = Vector3.zero;
            p.x = float.Parse( popupT.GetValue("px") );
            p.y = float.Parse( popupT.GetValue("py") );
            p.z = float.Parse( popupT.GetValue("pz") );
            selEntity.SetLocalPosition(p);
            p = Vector3.zero;
            p.x = float.Parse( popupT.GetValue("rx") );
            p.y = float.Parse( popupT.GetValue("ry") );
            p.z = float.Parse( popupT.GetValue("rz") );
            selEntity.transform.localEulerAngles = p;
        } else {
            popupT.Hide();
        }
    }

    public void OnEntitySelect() {
        string type = eTypes[entityList.value];
        string id = gameMaster.SpawnEntity(type);
        Entity en = gameMaster.GetEntity(id);
        if(en != null) {
            en.SetPosition(transform.position);
        }
        entityList.SetValueWithoutNotify(0);
        entityList.gameObject.SetActive(false);
        eListActive = false;
    }
}
