using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class InvDisp : MonoBehaviour {
    
    public Inventory inventory;
    public Text invName;
    public UnityEngine.UI.Image[] itemIcons;
    private float t = 0;
    public float mt = 0.5f;
    public int page = 0;
    public Vector2 inPos;
    private int cSelItem = -1;
    private UnityEngine.UI.Image cSelImg = null;
    public Sprite back;
    public GameObject itemData;
    public Inventory hand;
    public bool hideIfEmpty;
    public GameObject pullB;
    public bool hiden;
    private bool hidenE;
    void Start() {
        // if(inventory != null) {
        //     inventory.AddItem("creditChip", 255);
        // }
        // if(itemData != null) {
        //     Transform t = itemData.transform.Find("Grab");
        //     if(t != null) {
        //         Button b = t.GetComponent<Button>();
        //         if(b != null) {
        //             b.onClick.AddListener(OnGrab);
        //             print("S");
        //         }
        //     }
        // }
    }

    
    void Update() {
        RectTransform rt = GetComponent<RectTransform>();
        if(!(hiden || hidenE)) {

            if(hideIfEmpty && inventory.items.Length <= 0) {
                hidenE = true;
            }
            //move pane in
            if(t < mt) {
                t += Time.deltaTime;
                float p = Mathf.Lerp(-1,1, t/mt);
                rt.anchoredPosition = new Vector3(p*inPos.x, p*inPos.y,0);
            }
            if(t >= mt) {
                t = mt;
                rt.anchoredPosition = new Vector3(1*inPos.x,1*inPos.y,0);
            }
            
            //display items
            if(invName != null)
                invName.text = inventory.invName;
            page = Mathf.Max(Mathf.Min(page, Mathf.CeilToInt((inventory.items.Length*1f) / (itemIcons.Length*1f)) - 1), 0);
            int pO = page * (itemIcons.Length);
            for(int i = 0 ; i+pO < itemIcons.Length; i++) {
                if(i+pO < inventory.items.Length && inventory.items[i+pO].q > 0) {
                    itemIcons[i].gameObject.SetActive(true);
                    Ico3d i3d = Items.GetIcon3d(inventory.items[i+pO].id);
                    if(i3d != null) {
                        itemIcons[i].sprite = back;
                        itemIcons[i].transform.Find("icon3d").GetComponent<Icon3d>().SetIcon(i3d.icon, i3d.posOffset, i3d.rotOffset, inventory.items[i+pO].id);
                    } else {
                        itemIcons[i].sprite = Items.GetSprite(inventory.items[i+pO].id);
                        itemIcons[i].transform.Find("icon3d").GetComponent<Icon3d>().RemoveIcon();
                    }
                    Text q = itemIcons[i].transform.Find("Q").GetComponent<Text>();
                    if(q != null) {
                        q.text = inventory.items[i+pO].q + "";
                    }
                } else {
                    itemIcons[i].gameObject.SetActive(false);
                }
            }

            //select item
            bool oSel = false;
            for(int i = 0 ; i < itemIcons.Length; i++) {
                ItemSel sl = itemIcons[i].GetComponent<ItemSel>();
                if(sl != null) {
                    if(sl.state) {
                        cSelItem = i + pO;
                        cSelImg = itemIcons[i];
                        // print(i + ", " + (i+pO));
                        oSel = true;
                    }
                }
            }
            if(!oSel) {
                // cSelItem = -1;
            }
            if(cSelItem >= 0 && cSelItem < inventory.items.Length) {
                if(itemData != null) {
                    itemData.SetActive(true);
                    RectTransform rtI = cSelImg.GetComponent<RectTransform>();
                    RectTransform rtD = itemData.GetComponent<RectTransform>();
                    rtD.localPosition = rtI.localPosition;
                    if(inPos.x > 0) {
                        rtD.localPosition = new Vector3(Mathf.Min((rt.sizeDelta.x/2f) - (150f/2f), rtD.localPosition.x), rtD.localPosition.y, rtD.localPosition.z);
                    } else {
                        rtD.localPosition = new Vector3(Mathf.Min((rt.sizeDelta.x/2f) - (150f/2f), rtD.localPosition.x), rtD.localPosition.y, rtD.localPosition.z);
                    }
                    Text q = itemData.transform.Find("Text").GetComponent<Text>();
                    if(q != null) {
                        q.text = LocalStrings.GetLocalString("itemNames", inventory.items[cSelItem].id) + "\n" + LocalStrings.GetLocalString("itemDescs", inventory.items[cSelItem].id);
                    }
                }
            } else {
                if(itemData != null) {
                    itemData.SetActive(false);
                }
            }
            if(pullB != null) {
                if(hand.items.Length <= 0) {
                    pullB.SetActive(false);
                } else {
                    pullB.SetActive(true);
                }
            }

            //display info about itme
        } else {

            if(hidenE && hideIfEmpty && inventory.items.Length > 0) {
                hidenE = false;
            }
            
            itemData.SetActive(false);
            
            //move panel out
            if(t > 0) {
                t -= Time.deltaTime;
                float p = Mathf.Lerp(-1,1, t/mt);
                rt.anchoredPosition = new Vector3(p*inPos.x, p*inPos.y,0);
            }
            if(t <= 0) {
                t = 0;
                rt.anchoredPosition = new Vector3(-1*inPos.x,-1*inPos.y,0);
            }
        }
    }

    public void OnGrab() {
        print("t");
        if(cSelItem >= 0) {
            if(hand.CanAdd()) {
                hand.AddItem(inventory.items[cSelItem].Copy());
                inventory.RemoveItem(inventory.items[cSelItem]);
            }
        }
    }

    public void OnPut() {
        if(hand != null) {
            hand.MoveItems(inventory);
        }
    }

    public void SetInv(Inventory inv) {
        inventory = inv;
        hiden = false;
    }

    public void Hide() {
        hiden = true;
    }
}
