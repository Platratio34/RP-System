using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSel : Selectable {

    public bool state = false;

    public override void OnSelect(BaseEventData eventData) {
        state = true;
    }

    public override void OnDeselect(BaseEventData eventData) {
        state = false;
    }
}
