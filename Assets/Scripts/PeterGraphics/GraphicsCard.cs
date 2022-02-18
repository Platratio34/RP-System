using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GraphicsCard : MonoBehaviour {

    public Queue<Frame> frames;
    public System.Object queueLocker = new System.Object();
    public CustomScreen[] screens;
    private List<Thread> threads;
    // public int frame = 0;

    void Start() {
        frames = new Queue<Frame>();
        threads = new List<Thread>();
        GChar.init();
    }

    void Update() {
        if(frames.Count > 0) {
            lock(queueLocker) {
                while(frames.Count > 0) {
                    Frame frame = frames.Dequeue();
                    int dest = frame.GetDest();
                    if(screens.Length >= dest) {
                        if(screens[dest] != null) {
                            if(!screens[dest].SetTexture(frame)) {
                                print("Couldn't Draw frame for screen " + dest + ", because the renderer is null");
                            } else {
                                frame.Dispose();
                            }
                        } else {
                            print("Couldn't Draw frame for screen " + dest + " with " + frame.GetNumActions() + " actions, becasue the screen is null");
                        }
                    } else {
                        print("Couldn't Draw frame for screen " + dest + " with " + frame.GetNumActions() + " actions, becasue the index is out of range");
                    }
                }
            }
        }
    }

    public void Render(Frame frame) {
        frame.SetCard(this);
        // print("Starting render of frame for screen " + frame.GetDest());
        // threads.Add(new Thread(frame.Render));
        Thread t = new Thread(frame.Render);
        t.Start();
        threads.Add(t);
        // frame.Render();
    }

    public Frame NewFrame(int screen) {
        return new Frame(screens[screen], screen);
    }
    public Frame NewFrameByName(string name) {
        int index = 0;
        for(int i = 0; i < screens.Length; i++) {
            if(screens[i].screenName.Equals(name)) {
                index = i;
                i = screens.Length;
                // print("Found screen");
            }
        }
        return new Frame(screens[index], index);
    }

    public void Log(string text) {
        print(text);
    }
}
