using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMount : Equipment {

    [Range(1,5)]
    public int size = 1;
    private GameObject weaponObj;
    private Weapon weapon;
    public Weapons type;

    public float powerStore;
    public float storedPower;
    public float maxIn;

    void Start() {
        if(weaponObj == null) setWeapon();
    }

    protected override void onUpdate() {
        if(storedPower < powerStore) {
            powerInReq = Mathf.Min((powerStore - storedPower) * Time.deltaTime, maxIn);
            storedPower = Mathf.Min(powerStore, storedPower + powerIn / Time.deltaTime);
        } else {
            powerInReq = 5;
        }
    }

    public void setWeapon() {
        if(weapon != null && weapon.type == type) return;

        if(weapon != null) {
            DestroyImmediate(weaponObj);
        }

        string path = string.Format(weaponPrefabPath, weaponPrefabTable[(int)type][size-1]);
        var prefab = Resources.Load<GameObject>(path);
        GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, new Quaternion());
        if(obj == null) {
            Debug.LogError(string.Format("Could not load '{0}'",path));
            return;
        }
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = new Quaternion();
        weaponObj = obj;
        weapon = obj.GetComponent<Weapon>();
    }

    public void shoot() {
        if(storedPower > weapon.shotPower) {
            storedPower -= weapon.shotPower;
            weapon.shoot();
            totalHeat += weapon.shotHeat;
        }
    }

    [System.Serializable]
    public enum Weapons {
        SENSOR_POD = 0,
        FIXED_PULSE_CANNON = 1,
        MINI_BOMB_LAUNCHER = 2,
        BOMB_LAUNCHER = 3,
        DUAL_PULSE_TURRET = 4,
        QUAD_PULSE_TURRET = 5,
        MISSILE_POD = 6,
        PULSE_POINT_DEFENSE = 7,
        HEAVY_PULSE_CANNON = 8,
        HEAVY_DUAL_PULSE_TURRET = 9,
        ORBITAL_BOMBARDMENT_CANNON = 10
    }

    private static bool[][] weaponTypeTable = new bool[][] {
        new bool[] {true , true , true , false, false},
        new bool[] {true , true , true , false, false},
        new bool[] {false, true , false, false, false},
        new bool[] {false, false, true , false, false},
        new bool[] {false, true , true , true , false},
        new bool[] {false, true , true , true , false},
        new bool[] {false, true , true , true , false},
        new bool[] {false, false, false, true , true },
        new bool[] {false, false, false, true , true },
        new bool[] {false, false, false, false, true },
        new bool[] {false, false, false, false, true }
    };

    public static bool isWeaponOfSize(Weapons wep, int size) {
        return weaponTypeTable[(int)wep][size - 1];
    }

    private static string weaponPrefabPath = "Prefabs/Entities/Interactables/Weapons/{0}";
    private static string[][] weaponPrefabTable = new string[][] {
        new string[] {"SensorPod S1", "SensorPod S2", "SensorPod S3", "", ""},
        new string[] {"Fixed Pulse Cannon S1", "Fixed Pulse Cannon S2", "Fixed Pulse Cannon S3", "", ""},
        new string[] {"", "Mini Bomb Launcher", "", "", ""},
        new string[] {"", "", "Bomb Launcher", "", ""},
        new string[] {"", "Dual Pulse Turret S2", "Dual Pulse Turret S3", "Dual Pulse Turret S4", ""},
        new string[] {"", "Quad Pulse Turret S2", "Quad Pulse Turret S3", "Quad Pulse Turret S4", ""},
        new string[] {"", "Missile Pod S2", "Missile Pod S3", "Missile Pod S4", ""},
        new string[] {"", "", "", "Pulse Point Defense S4", "Pulse Point Defense S5"},
        new string[] {"", "", "", "Heavy Pulse Cannon S4", "Heavy Pulse Cannon S5"},
        new string[] {"", "", "", "", "Heavy Dual Pulse Turret"},
        new string[] {"", "", "", "", ""}
    };
}
