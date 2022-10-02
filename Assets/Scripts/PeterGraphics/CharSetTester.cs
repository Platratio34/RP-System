using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharSetTester : MonoBehaviour {

    public GraphicsCard gCard;
    void Start() {
        thing();
    }

    void Update() {

    }

    private void thing() {
        Frame f = gCard.NewFrame(0);
        string s = "";
        for(int i = 0; i < 256; i++) {
            s += (char)i+"";
            // if(i > 31) print(i+": '"+(char)i+"'");
            if(i%32==31) {
                s+="\n";
            }
        }
        f.DrawText(1,1,s,9,Color.red,true);
        gCard.Render(f);
    }
}
