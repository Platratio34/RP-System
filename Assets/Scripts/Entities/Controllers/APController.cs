using UnityEngine;

public class APController : MonoBehaviour {

    public Helm helm;

    public void onChange(EID eid) {
        helm.autopilot = eid.state == 1;
    }
}