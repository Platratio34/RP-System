using System;
using UnityEngine;

public class PowerString {

    private static float[] amounts = new float[] {
        1e3f,
        1e6f,
        1e9f,
        1e12f
    };
    private static string[] names = new string[] {
        "K",
        "M",
        "G",
        "T"
    };

    public static string convert(float input) {
        bool n = input < 0;
        float i2 = input * (n ? -1 : 1);
        string cStr = Math.Round(input,3)+"";
        if(input >= 1) {
            cStr = Math.Round(input,2)+"";
        }
        if(input >= 10) {
            cStr = Math.Round(input,1)+"";
        }
        if(input >= 100) {
            cStr = Math.Round(input)+"";
        }
        for(int i = 0; i < amounts.Length; i++) {
            if(i2 >= amounts[i]) {
                double amt = Math.Round(input/amounts[i],3);
                if(amt >= 1) {
                    amt = Math.Round(amt,2);
                }
                if(amt >= 10) {
                    amt = Math.Round(amt,1);
                }
                if(amt >= 100) {
                    amt = Math.Round(amt);
                }
                cStr = amt + names[i];
            }
        }
        return cStr;
    }

    public static int check(float input) {
        int l = 0;
        for(int i = 0; i < amounts.Length; i++) {
            if(input >= amounts[i]*10f) {
                l = i;
            }
        }
        return l;
    }

    public static string convert(float input, int l) {
        return Math.Round(input/amounts[l],1) + names[l];
    }
}