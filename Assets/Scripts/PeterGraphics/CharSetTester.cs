using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharSetTester : MonoBehaviour {

    public GraphicsCard gCard;
    void Start() {
    }

    void Update() {
        thing();
    }

    private void thing() {
        Frame f = gCard.NewFrame(0);
        string s = "";
        for(int i = 0; i < 128; i++) {
            s += (char)i+"";
            if(i%16==15) {
                s+="\n";
            }
        }
        f.DrawText(1,1,s,7,Color.red,true);
        gCard.Render(f);
    }
}
