using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/// <summary>
/// Handles rendering for CustomScreens with Frames
/// </summary>
public class GraphicsCard : MonoBehaviour {

    /// <summary>
    /// Queue of rendered frames
    /// </summary>
    public Queue<Frame> frames;
    /// <summary>
    /// Fancy thing to prevent bugs with filling the rendered queue
    /// </summary>
    public System.Object queueLocker = new System.Object();
    /// <summary>
    /// Array of screens connected to the graphics card
    /// </summary>
    public CustomScreen[] screens;
    /// <summary>
    /// Array of render threads
    /// </summary>
    private List<Thread> threads;
    // public int frame = 0;

    private List<DisplayerHandel> dspHandles;

    void Awake() {
        frames = new Queue<Frame>();
        threads = new List<Thread>();
        dspHandles = new List<DisplayerHandel>();
        GChar.init();
    }

    void Update() {
        if(frames.Count > 0) { // go through all rendered frames and apply them
            lock(queueLocker) { // prevent the queue from being modified while reading from it
                while(frames.Count > 0) {
                    Frame frame = frames.Dequeue();
                    int dest = frame.GetDest();
                    if(screens.Length >= dest) {
                        if(screens[dest] != null) {
                            if(!screens[dest].SetTexture(frame)) {
                                print("Couldn't Draw frame for screen " + dest + ", because the renderer is null");
                            }
                        } else {
                            print("Couldn't Draw frame for screen " + dest + " with " + frame.GetNumActions() + " actions, because the screen is null");
                        }
                    } else {
                        print("Couldn't Draw frame for screen " + dest + " with " + frame.GetNumActions() + " actions, because the index is out of range");
                    }
                    frame.Dispose();
                }
            }
        }
        foreach (DisplayerHandel handel in dspHandles) {
            if(handel.evaluate(Time.deltaTime)) {
                handel.dsp.display();
            }
        }
    }

    /// <summary>
    /// Render and apply a frame
    /// </summary>
    /// <param name="frame">Frame to render</param>
    public void Render(Frame frame) {
        frame.SetCard(this);
        // print("Starting render of frame for screen " + frame.GetDest());
        // threads.Add(new Thread(frame.Render));
        Thread t = new Thread(frame.Render);
        t.Start();
        // threads.Add(t);
        // frame.Render();
    }

    /// <summary>
    /// Create a new frame for the screen at index
    /// </summary>
    /// <param name="screen">Screen index</param>
    /// <returns>New Frame initialized for screen</returns>
    public Frame NewFrame(int screen) {
        return new Frame(screens[screen], screen);
    }
    /// <summary>
    /// Creates a new frame for the screen with name
    /// </summary>
    /// <param name="name">Screen name</param>
    /// <returns>New Frame initialized for screen</returns>
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

    /// <summary>
    /// Logs from this object
    /// </summary>
    /// <param name="text">Log message</param>
    public void Log(string text) {
        print(text);
    }

    public void addDisplayer(Displayer dsp, int fps) {
        dspHandles.Add(new DisplayerHandel(dsp, fps));
    }

    private class DisplayerHandel {

        public Displayer dsp;
        public int rfsp;
        private float tft;
        private float tsf;

        public DisplayerHandel(Displayer dsp, int fsp) {
            this.dsp = dsp;
            rfsp = fsp;
            tft = 1f/(float)rfsp;
        }

        public bool evaluate(float time) {
            tsf += time;
            if(tsf > tft) {
                tsf = 0;
                return true;
            }
            return false;
        }
    }
}
