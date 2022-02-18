using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Frame {

    private int dest;
    private int width;
    private int height;
    private Queue<FrameAction> actions;
    private Texture output;
    private bool rendered;
    public FrameEvent doneRendering;
    private GraphicsCard card;
    private int numActions = 0;

    public Frame(int width, int height) {
        this.width = width;
        this.height = height;
        dest = 0;
        doneRendering = new FrameEvent();
        actions = new Queue<FrameAction>();
    }
    public Frame(int width, int height, int dest) {
        this.width = width;
        this.height = height;
        this.dest = dest;
        doneRendering = new FrameEvent();
        actions = new Queue<FrameAction>();
    }
    public Frame(CustomScreen screen, int dest) {
        width = screen.size.x;
        height = screen.size.y;
        this.dest = dest;
        doneRendering = new FrameEvent();
        actions = new Queue<FrameAction>();
    }

    public int GetDest() {
        return dest;
    }
    public Texture2D GetOutput() {
        return output.GetTexture2D();
    }
    public void Dispose() {
        // Object.Destroy(output);
    }

    public Texture2D ReplaceTexture2D(Texture2D texture) {
        texture.SetPixels(output.GetPixels());
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }
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

    public int GetHeight() {
        return height;
    }
    public int GetWidth() {
        return width;
    }

    public void DrawText(int x, int y, string text, int h, Color color) {
        actions.Enqueue(new FrameAction(ActionCode.TEXT, x, y, 0, h, text, color));
    }
    public void DrawText(int x, int y, string text, int h, Color color, bool debug) {
        actions.Enqueue(new FrameAction(ActionCode.TEXT, x, y, 0, h, text, color,debug));
    }

    public void DrawRect(int x, int y, int w, int h, Color color) {
        actions.Enqueue(new FrameAction(ActionCode.RECT, x, y, w, h, null, color));
    }
    public void DrawRectF(int x, int y, int w, int h, Color color) {
        actions.Enqueue(new FrameAction(ActionCode.RECTF, x, y, w, h, null, color));
    }

    public void DrawImg(int x, int y, string link) {
        actions.Enqueue(new FrameAction(ActionCode.IMG, x, y, 0, 0, link, Color.black));
    }
    public void DrawGrid(Color color) {
        actions.Enqueue(new FrameAction(ActionCode.GRID, 0, 0, 0, 0, null, color));
    }

    public void Clear() {
        actions.Clear();
    }

    public void Fill(Color color) {
        actions.Enqueue(new FrameAction(ActionCode.RECTF, 0, 0, width, height, null, color));
    }

    public void SetCard(GraphicsCard card) {
        this.card = card;
    }

    public void Render() {
        // card.Log("Frame rendering " + actions.Count + " actions");
        if(!rendered) {
            output = new Texture(width, height);

            while(actions.Count > 0) {
                FrameAction action = actions.Dequeue();
                numActions++;
                if(action.code == ActionCode.RECT) {
                    for(int i = 0; i < action.w; i++) {
                        output.SetPixel(action.x + i, height-(action.y), action.color);
                        output.SetPixel(action.x + i, (action.y + action.h - 1), action.color);
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
                            xo = action.x - 6;
                            yo += 10;
                        }
                        if(xo + 6 > width) {
                            xo = action.x;
                            yo += 10;
                        }
                        DrawMap(xo, yo, GChar.getChar(chars[i], action.h, action.debug), action.color);
                        xo += 6;
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
    }

    public void DrawMap(int x, int y, float[,] map, Color color) {
		int w = map.GetLength(1);
		for(int i = 0; i < map.GetLength(0); i++) {
			for(int j = 0; j < w; j++) {
				Color sColor = output.GetPixel(x+j, height-(y+i));
				output.SetPixel(x+j, height-(y+i), new Color(Mathf.Lerp(sColor.r, color.r, map[i,j]), Mathf.Lerp(sColor.g, color.g, map[i,j]), Mathf.Lerp(sColor.b, color.b, map[i,j])));
			}
		}
	}

    public int GetNumActions() {
        return numActions;
    }

    // public void SetPixel(int x, int y, Color color) {
    //     output.SetPixel(width - x, height - y, )
    // }

    private class FrameAction {
        public ActionCode code;
        public int x;
        public int y;
        public int w;
        public int h;
        public string text;
        public Color color;
        public bool debug;

        public FrameAction(ActionCode code, int x, int y, int w, int h, string text, Color color) {
            this.code = code;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.text = text;
            this.color = color;
        }
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

    private enum ActionCode {
        RECT = 0,
        RECTF = 1,
        TEXT = 2,
        IMG = 3,
        GRID = 99
    }

    private class Texture {

        private Color[] pixels;
        private int w;
        private int h;

        public Texture(int w, int h) {
            this.w = w;
            this.h = h;
            pixels = new Color[w*h];
            for(int i = 0; i < pixels.GetLength(0); i++) {
                pixels[i] = new Color(0,0,0,1);
            }
        }

        public void SetPixel(int x, int y, Color c) {
            if(IsInBounds(x, y)) {
                pixels[x+(y*w)] = c;
            }
        }

        public Texture2D GetTexture2D() {
            Texture2D t = new Texture2D(w, h);
            t.SetPixels(pixels);
            t.filterMode = FilterMode.Point;
            t.Apply();
            return t;
        }

        public Color GetPixel(int x, int y) {
            if(IsInBounds(x, y)) {
                return pixels[x+(y*w)];
            } else {
                return new Color();
            }
        }

        public Color[] GetPixels() {
            return pixels;
        }

        public bool IsInBounds(int x, int y) {
            return x >= 0 && x < w && y >= 0 && y < h;
        }
    }
}

public class FrameEvent : UnityEvent<Frame> {}
