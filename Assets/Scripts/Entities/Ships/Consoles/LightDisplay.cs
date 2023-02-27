using UnityEngine;

public class LightDisplay : MonoBehaviour, Displayer {

    public GraphicsCard gCard;

    public LightDisplayArea[] lightAreas;

    void Start() {
        gCard.addDisplayer(this, 2);
    }

    public void display() {
        Frame frame = gCard.NewFrame(0);
        frame.Fill(new Color(0.05f, 0.05f, 0.08f, 1));

        for (int i = 0; i < lightAreas.Length; i++) {
            lightAreas[i].draw(frame);
        }

        gCard.Render(frame);
    }


    [System.Serializable]
    public class LightDisplayArea {

        public string name;
        public Vector2Int pos;
        public LightSwitcher lights;

        public void draw(Frame frame) {
            Color c = Color.white;
            if(!lights.on) {
                c = Color.gray;
            } else if(lights.backup) {
                c = Color.yellow;
            } else if(!lights.onPow) {
                c = Color.red;
            } else if(lights.opMode) {
                c = Color.blue;
            }
            frame.DrawTextC(pos.x, pos.y, name, 9, c);
        }

    }

}