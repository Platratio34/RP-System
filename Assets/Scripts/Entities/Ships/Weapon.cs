using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    
    public WeaponMount.Weapons type;
    [Range(1,5)]
    public int size = 1;

    public float shotPower;
    public float shotHeat;

    public void shoot() {
        
    }

}
