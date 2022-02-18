using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPartScreen : CustomScreen {

    public CustomScreen[] parts;

    void Start() {
        size.x = 0;
        size.y = 0;
        for(int i = 0; i < parts.Length; i++) {
            size.x += parts[i].size.x;
            size.y = Mathf.Max(parts[i].size.y, size.y);
        }
    }

    void Update() {

    }

    public override bool SetTexture(Frame frame) {
        int xo = 0;
        for(int i = 0; i < parts.Length; i++) {
            parts[i].SetTexture(frame.SplitTexture(xo, 0, parts[i].size.x, parts[i].size.y));
            xo += parts[i].size.x;
        }
        return true;
    }
}
