using System;
using UnityEngine;

public class PowerString {

    private static float[] amounts = new float[] {
        0,
        1e3f,
        1e6f,
        1e9f,
        1e12f
    };
    private static string[] names = new string[] {
        "",
        "K",
        "M",
        "G",
        "T"
    };

    public static string convert(float input) {
        bool n = input < 0;
        float i2 = input * (n ? -1 : 1);
        for(int i = amounts.Length-1; i >= 0; i--) {
            if(i2 >= amounts[i]) {
                // int[] digits = new int[5];
                // int dm = 1000 * amounts[i];
                // int nz = 0;
                // for (int d = 5; d >= 0; d--) {
                //     int c = dm / 10;
                //     digits[d] = (int)(i2 % dm) / c;
                //     if(digits[d] > 0) nz++;

                //     if(nz == 3) {

                //     }

                //     dm = c;
                // }
                // double amt = Math.Round(input/amounts[i],3);
                double amt = input / amounts[i];
                double amtA = Math.Abs(amt);
                if(amtA >= 100) {
                    amt = Math.Round(amt);
                } else if(amtA >= 10) {
                    amt = Math.Round(amt,1);
                } else if(amtA >= 1) {
                    amt = Math.Round(amt,2);
                } else {
                    amt = Math.Round(amt,3);
                }
                // cStr = amt + names[i];
                return string.Format("{0}{1}", amt, names[i]);
            }
        }
        return Math.Round(input,3)+"";
    }

    public static int check(float input) {
        int l = 0;
        for(int i = amounts.Length-1; i >= 0; i--) {
            if(input >= amounts[i]*10f) {
                l = i;
                return l;
            }
        }
        return l;
    }

    public static string convert(float input, int l) {
        return Math.Round(input/amounts[l],1) + names[l];
    }
}