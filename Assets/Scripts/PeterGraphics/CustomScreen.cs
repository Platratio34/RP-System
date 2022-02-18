using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomScreen : MonoBehaviour {

    public Renderer screen;
    public Vector2Int size;
    public string screenName;

    void Start() {

    }


    void Update() {

    }

    public virtual bool SetTexture(Frame frame) {
        if(screen != null) {
            if(screen.material.mainTexture == null) {
                screen.material.mainTexture = new Texture2D(size.x, size.y);
            }
            screen.material.mainTexture = frame.ReplaceTexture2D((Texture2D)screen.material.mainTexture);
            return true;
        } else {
            return false;
        }
    }
    public virtual bool SetTexture(Texture2D texture) {
        if(screen != null) {
            if(screen.material.mainTexture != null) {
                Object.Destroy(screen.material.mainTexture);
            }

            screen.material.mainTexture = texture;
            return true;
        } else {
            return false;
        }
    }
}
