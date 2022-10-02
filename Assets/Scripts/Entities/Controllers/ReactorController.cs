using UnityEngine;

public class ReactorController : MonoBehaviour {

    public Reactor reactor;

    public void onChange(EID eid) {
        reactor.controlLevel = eid.state / 10f;
    }
}