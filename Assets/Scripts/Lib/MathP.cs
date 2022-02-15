using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathP {

    /// <summary>Interpolates between 2 Vector3s by time <c>t</c></summary>
    /// <param name="a">Value a <c>t=0</c></param>
    /// <param name="b">Value a <c>t=1</c></param>
    /// <param name="a">Position between a and b</param>
    /// <returns>A Vector3 between a and b</returns>
    public static Vector3 VLerp(Vector3 a, Vector3 b, float t) {
        t = Clamp(t, 0f, 1f);
        Vector3 v = Vector3.zero;
        v.x = Mathf.Lerp(a.x, b.x, t);
        v.y = Mathf.Lerp(a.y, b.y, t);
        v.z = Mathf.Lerp(a.z, b.z, t);
        return v;
    }

    /// <summary>Clamps <c>a</c> between <c>min</c> and <c>max</c></summary>
    /// <param name="a">Value To clamp</param>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <returns>Float <c>a</c> clamped between <c>min</c> and <c>max</c></returns>
    public static float Clamp(float a, float min, float max) {
        if(min > max) {
            float temp = min;
            max = min;
            min = temp;
        }
        return Mathf.Min(Mathf.Max(a, min), max);
    }

    /// <summary>Finds the smallest common multiple of <c>a</c> and <c>b</c></summary>
    /// <param name="a">Value a</param>
    /// <param name="b">Value a</param>
    /// <returns>Smallest common multiple of <c>a</c> and <c>b</c></returns>
    public static float LCM(float a, float b) {
        float am = a;
        float bm = b;
        while(am*a != bm*b) {
            if(am*a > bm*b) {
                bm++;
            } else if(am*a < bm*b) {
                am++;
            }
        }
        return am*a;
    }
}
