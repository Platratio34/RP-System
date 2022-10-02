using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates power for the network, also produces heat
/// </summary>
public class HeatSink : Equipment {

    
    public float maxHeatIn = 1;
    public Equipment[] sources;
    private float heatIn;

    void Start() {

    }

    protected override void onUpdate() {
        heatIn = 0;
        float hIR = maxHeatIn * Time.deltaTime;
        for(int i = 0; i < sources.Length; i++) {
            float amt = Mathf.Min(hIR, sources[i].getTotalHeat());
            sources[i].addHeatOut(amt/Time.deltaTime);
            heatIn += amt;
            hIR -= amt;
            // if(hIR <= 0) {
            //     break;
            // }
            // if(sources[i].getTotalHeat() > hIR) {
            //     heatIn += hIR;
            //     sources[i].takeHeat(hIR);
            //     hIR = 0;
            //     break;
            // } else {
            //     float hI = sources[i].getTotalHeat();
            //     heatIn += hI;
            //     hIR -= hI;
            //     sources[i].takeHeat(hI);
            // }
        };
        heatIn = heatIn / Time.deltaTime;
        heatGen = heatIn;
    }

    public float getHeatIn() {
        return heatIn;
    }
}
