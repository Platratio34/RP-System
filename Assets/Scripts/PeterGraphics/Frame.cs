using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Frame to be drawn on a CustomScreen
/// </summary>
public class Frame {

    /// <summary>
    /// Destination screen index
    /// </summary>
    private int dest;
    /// <summary>
    /// The pixel width of the frame
    /// </summary>
    private int width;
    /// <summary>
    /// The pixel height of the frame
    /// </summary>
    private int height;
    /// <summary>
    /// List of actions for rendering
    /// </summary>
    private Queue<FrameAction> actions;
    /// <summary>
    /// The output texture from rendering
    /// </summary>
    private Texture output;
    /// <summary>
    /// If the frame has been rendered
    /// </summary>
    private bool rendered;
    /// <summary>
    /// Done rendering event
    /// </summary>
    public FrameEvent doneRendering;
    /// <summary>
    /// The graphics card the frame is rended from
    /// </summary>
    private GraphicsCard card;
    /// <summary>
    /// The number of actions to render
    /// </summary>
    private int numActions = 0;

    /// <summary>
    /// Creates a new Frame with given width and height
    /// </summary>
    /// <param name="width">Pixel width</param>
    /// <param name="height">Pixel height</param>
    public Frame(int width, int height) {
        this.width = width;
        this.height = height;
        dest = 0;
        doneRendering = new FrameEvent();
        actions = new Queue<FrameAction>();
    }
    /// <summary>
    /// Creates a new Frame with a givin width, height, and destination index
    /// </summary>
    /// <param name="width">Pixel width</param>
    /// <param name="height">Pixel height</param>
    /// <param name="dest">Destination index</param>
    public Frame(int width, int height, int dest) {
        this.width = width;
        this.height = height;
        this.dest = dest;
        doneRendering = new FrameEvent();
        actions = new Queue<FrameAction>();
    }
    /// <summary>
    /// Cremates a new Frame for a screen, and sets the destination index
    /// </summary>
    /// <param name="screen">Target screen</param>
    /// <param name="dest">Target index</param>
    public Frame(CustomScreen screen, int dest) {
        width = screen.size.x;
        height = screen.size.y;
        this.dest = dest;
        doneRendering = new FrameEvent();
        actions = new Queue<FrameAction>();
    }

    ~Frame() {
        // output.Dispose();
    }

    /// <summary>
    /// Gets the index of the destination screen
    /// </summary>
    /// <returns>Screen index</returns>
    public int GetDest() {
        return dest;
    }
    /// <summary>
    /// Gets the Texture2D from rendering
    /// </summary>
    /// <returns></returns>
    public Texture2D GetOutput() {
        return output.GetTexture2D();
    }
    public void Dispose() {
        // Object.Destroy(output);
    }

    /// <summary>
    /// Replaces the pixels of a Texture2D with the render
    /// </summary>
    /// <param name="texture">Texture to modify</param>
    /// <returns>The modified texture</returns>
    public Texture2D ReplaceTexture2D(Texture2D texture) {
        texture.SetPixels(output.GetPixels());
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }
    /// <summary>
    /// Crops the render for multi screen usage
    /// </summary>
    /// <param name="xo">Starting x</param>
    /// <param name="yo">Starting y</param>
    /// <param name="w">Width</param>
    /// <param name="h">Height</param>
    /// <returns>Cropped texture</returns>
    public Texture2D SplitTexture(int xo, int yo, int w, int h) {
        Texture2D texture = new Texture2D(w, h);
        texture.filterMode = FilterMode.Point;
        for(int x = 0; x < w; x++) {
            for(int y = 0; y < h; y++) {
                texture.SetPixel(x, y, output.GetPixel(x + xo, y + yo));
            }
        }
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// Gets the height of the frame
    /// </summary>
    /// <returns>Pixel height</returns>
    public int GetHeight() {
        return height;
    }
    /// <summary>
    /// Gets the width of the frame
    /// </summary>
    /// <returns>Pixel width</returns>
    public int GetWidth() {
        return width;
    }

    /// <summary>
    /// Draws text at location, with color and size
    /// </summary>
    /// <param name="x">Left x</param>
    /// <param name="y">Top y</param>
    /// <param name="text">Text</param>
    /// <param name="h">Text size</param>
    /// <param name="color">Text color</param>
    public void DrawText(int x, int y, string text, int h, Color color) {
        actions.Enqueue(new FrameAction(ActionCode.TEXT, x, y, 0, h, text, color));
    }
    /// <summary>
    /// Draws text at location, with color and size, and if it should render whitespace characters
    /// </summary>
    /// <param name="x">Left x</param>
    /// <param name="y">Top y</param>
    /// <param name="text">Text</param>
    /// <param name="h">Text size</param>
    /// <param name="color">Text color</param>
    /// <param name="debug">Draw whitespace characters</debug>
    public void DrawText(int x, int y, string text, int h, Color color, bool debug) {
        actions.Enqueue(new FrameAction(ActionCode.TEXT, x, y, 0, h, text, color,debug));
    }
    /// <summary>
    /// Draws text at location, with color and size
    /// </summary>
    /// <param name="x">Left x</param>
    /// <param name="y">Top y</param>
    /// <param name="text">Text</param>
    /// <param name="h">Text size</param>
    /// <param name="color">Text color</param>
    public void DrawTextC(int x, int y, string text, int h, Color color) {
        actions.Enqueue(new FrameAction(ActionCode.TEXT, x-((text.Length*(h-1))/2), y, 0, h, text, color));
    }

    /// <summary>
    /// Draws a rectangle outline in color
    /// </summary>
    /// <param name="x">Left x</param>
    /// <param name="y">Top y</param>
    /// <param name="w">Rectangle width</param>
    /// <param name="h">Rectangle height</param>
    /// <param name="color">Rectangle color</param>
    public void DrawRect(int x, int y, int w, int h, Color color) {
        actions.Enqueue(new FrameAction(ActionCode.RECT, x, y, w, h, null, color));
    }
    /// <summary>
    /// Draws a filled rectangle in color
    /// </summary>
    /// <param name="x">Left x</param>
    /// <param name="y">Top y</param>
    /// <param name="w">Rectangle width</param>
    /// <param name="h">Rectangle height</param>
    /// <param name="color">Rectangle color</param>
    public void DrawRectF(int x, int y, int w, int h, Color color) {
        actions.Enqueue(new FrameAction(ActionCode.RECTF, x, y, w, h, null, color));
    }

    /// <summary>
    /// Draws and image at location by name (not yet implemented)
    /// </summary>
    /// <param name="x">Left x</param>
    /// <param name="y">Top y</param>
    /// <param name="link">Image name</param>
    public void DrawImg(int x, int y, string link) {
        actions.Enqueue(new FrameAction(ActionCode.IMG, x, y, 0, 0, link, Color.black));
    }

    /// <summary>
    /// Draws a checkerboard grid in color
    /// </summary>
    /// <param name="color">Grid color</param>
    public void DrawGrid(Color color) {
        actions.Enqueue(new FrameAction(ActionCode.GRID, 0, 0, 0, 0, null, color));
    }

    /// <summary>
    /// Clears the frame
    /// </summary>
    public void Clear() {
        actions.Clear();
    }

    /// <summary>
    /// Fills the frame with a color
    /// </summary>
    /// <param name="color">Fill color</param>
    public void Fill(Color color) {
        actions.Enqueue(new FrameAction(ActionCode.RECTF, 0, 0, width, height, null, color));
    }

    /// <summary>
    /// Set tha associated graphics card
    /// </summary>
    /// <param name="card">Graphics card</param>
    public void SetCard(GraphicsCard card) {
        this.card = card;
    }

    private long startTime;
    private long endTime;
    public bool profile = false;

    /// <summary>
    /// Render the frame. SHOULD BE DONE IN SEPARATE THREAD TO AVOID LARGE LAG
    /// </summary>
    public void Render() {
        // card.Log("Frame rendering " + actions.Count + " actions");
        if(profile && GetTimeSinceStart() > 1) {
            card.Log(string.Format("Took {0}ms to start rendering frame", GetTimeSinceStart()));
        }
        if(!profile) startTime = GetUnixTime();
        if(!rendered) {
            output = new Texture(width, height);

            while(actions.Count > 0) {
                FrameAction action = actions.Dequeue();
                numActions++;
                if(action.code == ActionCode.RECT) {
                    for(int i = 0; i < action.w; i++) {
                        output.SetPixel(action.x + i, height-(action.y), action.color);
                        output.SetPixel(action.x + i, height-(action.y + action.h - 1), action.color);
                    }
                    for(int i = 1; i < action.h - 1; i++) {
                        output.SetPixel(action.x, height-(action.y + i), action.color);
                        output.SetPixel(action.x + action.w - 1, height-(action.y + i), action.color);
                    }
                } else if(action.code == ActionCode.RECTF) {
                    for(int i = 0; i < action.w; i++) {
                        for(int j = 0; j < action.h; j++) {
                            output.SetPixel(action.x + i, height-(action.y + j), action.color);
                        }
                    }
                } else if(action.code == ActionCode.GRID) {
                    for(int i = 0; i < width; i++) {
                        for(int j = 0; j < height; j++) {
                            if((i % 2 == 0 && j % 2 == 0) || (i % 2 == 1 && j % 2 == 1)) {
                                output.SetPixel(i, j, action.color);
                            }
                        }
                    }
                } else if(action.code == ActionCode.TEXT) {
                    char[] chars = action.text.ToCharArray();
                    int xo = action.x;
                    int yo = action.y + 1;
                    for(int i = 0; i < chars.Length; i++) {
                        if(chars[i] == '\n') {
                            DrawMap(xo, yo, GChar.getChar(chars[i], action.h, action.debug), action.color);
                            xo = action.x - (action.h-1);
                            yo += (int)(action.h*1.5f);
                        }
                        if(xo + 6 > width) {
                            xo = action.x;
                            yo += (int)(action.h*1.5f);
                        }
                        DrawMap(xo, yo, GChar.getChar(chars[i], action.h, action.debug), action.color);
                        xo += (action.h-1);
                    }
                }
            }
            rendered = true;
        }
        if(card != null) {
            lock(card.queueLocker) {
                card.frames.Enqueue(this);
            }
        }
        endTime = GetUnixTime();
    }

    /// <summary>
    /// Use for rendering, draws a pixel map to the texture
    /// </summary>
    /// <param name="x">Left x</param>
    /// <param name="y">Top y</param>
    /// <param name="map">Pixel map to draw</param>
    /// <param name="color">Color to draw in</param>
    public void DrawMap(int x, int y, float[,] map, Color color) {
		int w = map.GetLength(1);
		for(int i = 0; i < map.GetLength(0); i++) {
			for(int j = 0; j < w; j++) {
				Color sColor = output.GetPixel(x+j, height-(y+i));
				output.SetPixel(x+j, height-(y+i), new Color(Mathf.Lerp(sColor.r, color.r, map[i,j]), Mathf.Lerp(sColor.g, color.g, map[i,j]), Mathf.Lerp(sColor.b, color.b, map[i,j])));
			}
		}
	}

    /// <summary>
    /// Gets the number of actions for rendering
    /// </summary>
    /// <returns></returns>
    public int GetNumActions() {
        return numActions;
    }

    public void MarkStart() {
        if(profile) startTime = GetUnixTime();
    }
    public long GetRenderTime() {
        if(endTime > 0) return endTime - startTime;
        else return -1;
    }
    public long GetTimeSinceStart() {
        if(startTime > 0) return GetUnixTime() - startTime;
        else return -1;
    }

    private long GetUnixTime() {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }

    // public void SetPixel(int x, int y, Color color) {
    //     output.SetPixel(width - x, height - y, )
    // }

    /// <summary>
    /// A render action
    /// </summary>
    private class FrameAction {
        /// <summary>
        /// The action type
        /// </summary>
        public ActionCode code;
        /// <summary>
        /// Left x
        /// </summary>
        public int x;
        /// <summary>
        /// Top y
        /// </summary>
        public int y;
        /// <summary>
        /// Width
        /// </summary>
        public int w;
        /// <summary>
        /// Height
        /// </summary>
        public int h;
        /// <summary>
        /// Text or image name
        /// </summary>
        public string text;
        /// <summary>
        /// Draw color
        /// </summary>
        public Color color;
        /// <summary>
        /// Debug draw
        /// </summary>
        public bool debug;

        /// <summary>
        /// New Frame action with all but debug
        /// </summary>
        /// <param name="code">Action type</param>
        /// <param name="x">Left x</param>
        /// <param name="y">Top y</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="text">Text or link</param>
        /// <param name="color">Draw color</param>
        public FrameAction(ActionCode code, int x, int y, int w, int h, string text, Color color) {
            this.code = code;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.text = text;
            this.color = color;
        }
        /// <summary>
        /// New Frame action with debug draw option
        /// </summary>
        /// <param name="code">Action type</param>
        /// <param name="x">Left x</param>
        /// <param name="y">Top y</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="text">Text or link</param>
        /// <param name="color">Draw color</param>
        /// <param name="debug">Debug draw</param>
        public FrameAction(ActionCode code, int x, int y, int w, int h, string text, Color color, bool debug) {
            this.code = code;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.text = text;
            this.color = color;
            this.debug = debug;
        }
    }

    /// <summary>
    /// Type of render action
    /// </summary>
    private enum ActionCode {
        /// <summary>
        /// Rectangle outline
        /// </summary>
        RECT = 0,
        /// <summary>
        /// Filled rectangle
        /// </summary>
        RECTF = 1,
        /// <summary>
        /// Text
        /// </summary>
        TEXT = 2,
        /// <summary>
        /// Image
        /// </summary>
        IMG = 3,
        /// <summary>
        /// Checkerboard grid
        /// </summary>
        GRID = 99
    }

    private class Texture {

        private Color[] pixels;
        private int w;
        private int h;

        /// <summary>
        /// Creates a new blank texture with size
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public Texture(int w, int h) {
            this.w = w;
            this.h = h;
            pixels = new Color[w*h];
            for(int i = 0; i < pixels.GetLength(0); i++) {
                pixels[i] = new Color(0,0,0,1);
            }
        }

        /// <summary>
        /// Sets the color of a pixel
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="c">Pixel color</param>
        public void SetPixel(int x, int y, Color c) {
            if(IsInBounds(x, y)) {
                pixels[x+(y*w)] = c;
            }
        }

        /// <summary>
        /// Converts to a Texture2D and returns it
        /// </summary>
        /// <returns>Texture2D representation</returns>
        public Texture2D GetTexture2D() {
            Texture2D t = new Texture2D(w, h);
            t.SetPixels(pixels);
            t.filterMode = FilterMode.Point;
            t.Apply();
            return t;
        }

        /// <summary>
        /// Gets the pixel a location
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>Color of the pixel</returns>
        public Color GetPixel(int x, int y) {
            if(IsInBounds(x, y)) {
                return pixels[x+(y*w)];
            } else {
                return new Color();
            }
        }

        /// <summary>
        /// Gets the pixel array
        /// </summary>
        /// <returns></returns>
        public Color[] GetPixels() {
            return pixels;
        }

        /// <summary>
        /// If the location is within the texture
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>Localtion within bounds</returns>
        public bool IsInBounds(int x, int y) {
            return x >= 0 && x < w && y >= 0 && y < h;
        }
    }
}

/// <summary>
/// Frame done rendering event
/// </summary>
public class FrameEvent : UnityEvent<Frame> {}
