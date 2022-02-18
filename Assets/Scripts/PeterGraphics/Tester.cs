using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

    public GraphicsCard gCard;
    private int frame = 0;
    public int t = 4;
    public bool str = true;
    public bool grid = true;
    public bool boxes = true;

    void Start() {

    }

    void Update() {
        frame++;
        Frame f = gCard.NewFrame(0);
        if(grid) { f.DrawGrid(Color.red); }
        if(str) {
            if((frame/t) % 4 == 0) {
                f.DrawText(0, f.GetHeight() - 11, "Testing", 7, Color.white);
            } else if((frame/t) % 4 == 1) {
                f.DrawText(0, f.GetHeight() - 11, "Testing .", 7, Color.white);
            } else if((frame/t) % 4 == 2) {
                f.DrawText(0, f.GetHeight() - 11, "Testing . .", 7, Color.white);
            } else {
                f.DrawText(0, f.GetHeight() - 11, "Testing . . .", 7, Color.white);
            }
        }
        if(boxes) {
            f.DrawRectF(frame % (f.GetWidth() - 2), 1, 2, 2, Color.green);
            f.DrawRectF(f.GetWidth() - (frame % (f.GetWidth() - 2)) - 2, f.GetHeight() - 1, 2, 2, Color.green);
        }

        gCard.Render(f);
    }
}
