using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCount : MonoBehaviour {
    public TMP_Text text;

    public float tps { get; private set; }

    private int smoothI;
    private float[] smooth;
    public float smoothTps { get; private set; }

    // private 
    void Start() {
        smooth = new float[100];
    }

    // Update is called once per frame
    void Update() {
        float tickTime = Time.deltaTime;
        tps = 1f / tickTime;

        smooth[smoothI] = tickTime;
        smoothI++;
        if(smoothI == smooth.Length) {
            smoothI = 0;
        }
        float sTT = 0;
        for (int i = 0; i < smooth.Length; i++) {
            sTT += smooth[i];
        }
        sTT /= smooth.Length;
        smoothTps = Mathf.Floor((1f / sTT)*10f) / 10f;

        text.text = string.Format("TPS: {0}\nSmoothed: {1} ({2} samples)", tps, smoothTps, smooth.Length);
    }
}
