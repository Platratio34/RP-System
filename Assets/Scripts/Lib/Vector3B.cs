using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class Vector3B {
    /// <summary>
    /// Positive x value
    /// </summary>
    public float xp;
    /// <summary>
    /// Negative x value
    /// </summary>
    public float xn;

    /// <summary>
    /// Positive y value
    /// </summary>
    public float yp;
    /// <summary>
    /// Negative y value
    /// </summary>
    public float yn;

    /// <summary>
    /// Positive z value
    /// </summary>
    public float zp;
    /// <summary>
    /// Negative z value
    /// </summary>
    public float zn;

    /// <summary>
    /// Creates a new Vector3B with all values 0
    /// </summary>
    public Vector3B() {}
    /// <summary>
    /// Creates a new Vector3B with specific values
    /// </summary>
    /// <param name="xp">Positive x value</param>
    /// <param name="xn">Negative x value</param>
    /// <param name="yp">Positive y value</param>
    /// <param name="yn">Negative y value</param>
    /// <param name="zp">Positive z value</param>
    /// <param name="zn">Negative z value</param>
    public Vector3B(float xp, float xn, float yp, float yn, float zp, float zn) {
        this.xp = xp;
        this.xn = xn;

        this.yp = yp;
        this.yn = yn;

        this.zp = zp;
        this.zn = zn;
    }

    /// <summary>
    /// Returns a Vector3B with all values 1
    /// </summary>
    /// <returns>A new Vector3B</returns>
    public static Vector3B one() {
        return new Vector3B(1,1, 1,1, 1,1);
    }

    /// <summary>
    /// Returns a Vector3 of the positive values
    /// </summary>
    /// <returns>(xp, yp, zp)</returns>
    public Vector3 positive3() {
        return new Vector3(xp,yp,zp);
    }
    /// <summary>
    /// Returns a Vector3 of the negative values
    /// </summary>
    /// <returns>(xn, yn, zn)</returns>
    public Vector3 negative3() {
        return new Vector3(xn,yn,zn);
    }

    /// <summary>
    /// Sets the positive values from a Vector3
    /// </summary>
    /// <param name="p">(xp, yp, zp)</param>
    public void applyPositive3(Vector3 p) {
        xp = p.x;
        yp = p.y;
        zp = p.z;
    }
    /// <summary>
    /// Sets the negative values from a Vector3
    /// </summary>
    /// <param name="n">(xn, yn, zn)</param>
    public void applyNegative3(Vector3 n) {
        xn = n.x;
        yn = n.y;
        zn = n.z;
    }

    /// <summary>
    /// Multiplies a Vector3 by the Vector3B,
    /// choosing the positive or negative value by sign of the input
    /// </summary>
    /// <param name="v3">Vector 3 to multiply by (not modified)</param>
    /// <returns>New Vector3 resulting from the multiplication</returns>
    public Vector3 multiply(Vector3 v3) {
        Vector3 vo = new Vector3();
        if(v3.x > 0) {
            vo.x = v3.x * xp;
        } else {
            vo.x = v3.x * xn;
        }
        
        if(v3.y > 0) {
            vo.y = v3.y * yp;
        } else {
            vo.y = v3.y * yn;
        }

        if(v3.z > 0) {
            vo.z = v3.z * zp;
        } else {
            vo.z = v3.z * zn;
        }
        return vo;
    }

    /// <summary>
    /// Divides a Vector3 by the Vector3B,
    /// choosing the positive or negative value by sign of the input
    /// </summary>
    /// <param name="v3">Vector 3 to divide by (not modified)</param>
    /// <returns>New Vector3 resulting from the division</returns>
    public Vector3 divide(Vector3 v3) {
        Vector3 vo = new Vector3();
        if(v3.x > 0) {
            vo.x = v3.x / xp;
        } else {
            vo.x = v3.x / xn;
        }
        if(vo.x == float.NaN) {
            vo.x = 0;
        }
        
        if(v3.y > 0) {
            vo.y = v3.y / yp;
        } else {
            vo.y = v3.y / yn;
        }
        if(vo.y == float.NaN) {
            vo.y = 0;
        }

        if(v3.z > 0) {
            vo.z = v3.z / zp;
        } else {
            vo.z = v3.z / zn;
        }
        if(vo.z == float.NaN) {
            vo.z = 0;
        }
        return vo;
    }

    /// <summary>
    /// Creates a Vector3B from a SerializedProperty
    /// </summary>
    /// <param name="prop">The property representing the Vector3B</param>
    /// <returns>A new Vector3B from the property</returns>
    public static Vector3B unProperty(SerializedProperty prop) {
        Vector3B v = new Vector3B();
        v.xp = prop.FindPropertyRelative("xp").floatValue;
        v.xn = prop.FindPropertyRelative("xn").floatValue;

        v.yp = prop.FindPropertyRelative("yp").floatValue;
        v.yn = prop.FindPropertyRelative("yn").floatValue;
        
        v.zp = prop.FindPropertyRelative("zp").floatValue;
        v.zn = prop.FindPropertyRelative("zn").floatValue;
        return v;
    }

    /// <summary>
    /// Updates a SerializedProperty from this Vector3B
    /// </summary>
    /// <param name="prop">The property to update</param>
    public void property(SerializedProperty prop) {
        prop.FindPropertyRelative("xp").floatValue = xp;
        prop.FindPropertyRelative("xn").floatValue = xn;

        prop.FindPropertyRelative("yp").floatValue = yp;
        prop.FindPropertyRelative("yn").floatValue = yn;
        
        prop.FindPropertyRelative("zp").floatValue = zp;
        prop.FindPropertyRelative("zn").floatValue = zn;
    }
}