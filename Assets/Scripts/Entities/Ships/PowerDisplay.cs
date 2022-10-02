using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Display for power network
/// </summary>
public class PowerDisplay : Interactable, Displayer {

    public PowerNetwork network;
    public GraphicsCard gCard;
    public string networkName;

    private float tBF = 1f/15f;
    private float tsl = 0f;
    private long frames;

    // Called when the display is created
    void Start() {
        gCard.addDisplayer(this, 10);
    }

    // Called every tick
    void Update() {
        tsl += Time.deltaTime;
        if(tsl < tBF) {
            return;
        }
        frames ++;
        tsl = 0f;

        // display();
    }

    public void display() {
        print("Somthing");
        Frame frame = gCard.NewFrame(0);

        frame.Fill(new Color(0.05f, 0.05f, 0.08f, 1));
        // frame.DrawText(1, 1, "Power Network: " + networkName, 7, Color.white);
        
        float bp = network.getBatUssage();
        float ta = network.getTotalAvalib();
        if(bp > 0) {
            ta += bp;
        }
        float tu = network.getTotalReq();
        if(bp < 0) {
            tu += bp;
        }
        float tp = Mathf.Max(ta, tu);
        float np = network.getTotalAvalib() - network.getTotalReq();
        float pg = network.getTotalAvalib() / tp;
        float pu = network.getTotalReq() / tp;
        float pb = bp / tp;

        Color oc = Color.white;
        if(pg > pu) {
            oc = Color.green;
        } else if(pu > pg) {
            if(pg + pb < pu) {
                oc = Color.red;
            } else {
                oc = Color.yellow;
            }
        }

        String text = "Power Network: " + networkName+"\n\n\n\n\n";
        String text2 = "Availble:  " + PowerString.convert(network.getTotalAvalib()) + "W\n";
        text2 += "Required:  " + PowerString.convert(network.getTotalReq()) + "W\n";
        text2 += "Net:       " + PowerString.convert(np) + "W\n";

        frame.DrawText(1, 14, text2, 9, oc);

        // frame.DrawText(1, 11, "Availble:  " + PowerString.convert(network.getTotalAvalib()) + "W", 7, oc);
        // frame.DrawText(1, 21, "Required:  " + PowerString.convert(network.getTotalReq()) + "W", 7, oc);
        // frame.DrawText(1, 31, "Net:       " + PowerString.convert(np) + "W", 7, oc);
        
        frame.DrawRectF(201, 11, 21, 100, new Color(0.1f,0.1f,0.1f));

        int gbh = (int)(pg * 100);
        frame.DrawRectF(201, (100 - gbh) + 11, 10, gbh, Color.green);
        int ubh = (int)(pu * 100);
        frame.DrawRectF(212, (100 - ubh) + 11, 10, ubh, Color.red);
        int bbh = (int)Math.Abs((pb * 100));
        if(pb > 0) {
            frame.DrawRectF(202, (100 - gbh - bbh) + 11, 10, bbh, Color.blue);
        } else if(pb < 0) {
            frame.DrawRectF(212, (100 - ubh - bbh) + 11, 10, bbh, Color.blue);
        }

        frame.DrawRect(200, 10, 23, 102, Color.gray);
        frame.DrawRect(211, 11, 1, 100, Color.white);
        frame.DrawText(201, 1, PowerString.convert(tp) + "W", 7, Color.white);

        text += "Bat Draw:  " + PowerString.convert(network.getBatUssage()) + "W\n";
        text += "Overage:   " + PowerString.convert(network.getOverage()) + "W\n";
        // frame.DrawText(1, 51, "Bat Draw:  " + PowerString.convert(network.getBatUssage()) + "W", 7, Color.white);
        // frame.DrawText(1, 61, "Overage:   " + PowerString.convert(network.getOverage()) + "W", 7, Color.white);

        text += "Stored:    " + PowerString.convert(network.getTotalStored()/360f)+"Wh\n";
        text += "Capacity:  " + PowerString.convert(network.getMaxStorage()/360f)+"Wh\n";
        // frame.DrawText(1, 71, "Stored:    " + PowerString.convert(network.getTotalStored()/360f)+"Wh", 7, Color.white);
        // frame.DrawText(1, 81, "Capacity:  " + PowerString.convert(network.getMaxStorage()/360f)+"Wh", 7, Color.white);

        float sTO = network.getTimeToOut();
        String tTO = "";
        if(sTO != float.PositiveInfinity && sTO < TimeSpan.MaxValue.TotalSeconds) {
            tTO = TimeSpan.FromSeconds(sTO).ToString(@"hh\:mm\:ss");
        } else {
            tTO = "Infinity";
        }
        text += "Time out:  " + tTO + "\n";
        // frame.DrawText(1, 91, "Time out:  " + tTO, 7, Color.white);

        float sTC = network.getTimeToCharged();
        String tTC = "";
        if(sTC != float.PositiveInfinity && sTC < TimeSpan.MaxValue.TotalSeconds) {
            tTC = TimeSpan.FromSeconds(sTC).ToString(@"hh\:mm\:ss");
        } else {
            tTC = "Infinity";
        }
        text += "Time full: " + tTC + "\n";
        // frame.DrawText(1, 101, "Time full:  " + tTC, 7, Color.white);
        
        int bpb = (int)((network.getTotalStored()/360f) / (network.getMaxStorage()/360f) * 100f);
        frame.DrawRectF(251, 11, 10, 100, new Color(0.1f,0.1f,0.1f));
        frame.DrawRectF(251, (100 - bpb) + 11, 10, bpb, Color.blue);
        frame.DrawRect(250, 10, 12, 102, Color.gray);
        frame.DrawText(251, 1, PowerString.convert(network.getMaxStorage()/360f) + "Wh", 7, Color.white);

        frame.DrawText(1,1,text,9,Color.white);

        gCard.Render(frame);
    }
}
