using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class GChar {

    private static Dictionary<int, float[][,]> chars;
    private static bool inited = false;
    public static int[,] charMaps = null;
    private static string log;

    private static bool detailLog = false;

    public static float[,] getChar(string c, int size) {
        return getChar(c[0], size);
    }
    public static float[,] getChar(char c, int size) {
        return getChar(c, size, false);
    }
    public static float[,] getChar(char c, int size, bool showWhite) {
        // c = char.ToUpper(c);
        int code = (int)c;
        string dsp = c+"";
        if(c=='\n') {
            dsp = "\\n";
            if(!showWhite)
                return new float[1,1];
        }
        if(c=='\t') {
            dsp = "\\t";
            if(!showWhite)
                return new float[1,1];
        }
        if(!chars.ContainsKey(size)) {
            Debug.LogError("Invalid charecter size; {size=" + size + " ; char='" + dsp + "' ; code=" + code + "}");
            return new float[1,1];
        }
        if(chars[size][code] == null) {
            Debug.LogError("No charecter; {size=" + size + " ; char='" + dsp + "' ; code=" + code + "}");
            return chars[size][0];
        }
        return chars[size][code];
    }

    static GChar() {
        if(charMaps == null) {
            charMaps = JsonUtility.FromJson<CharMapArr>(SaveLoadData.LoadString("charMaps.json")).getArr();
			if(charMaps == null) {
				charMaps = new int[,] { {0,0} };
				Debug.LogError("Loaded null charMaps array: created default array");
			}
			chars = new Dictionary<int, float[][,]>();
			initF();
        }
        // Debug.Log("Inited: " + inited);
    }

    public static void init() {
        if(charMaps == null) {
            charMaps = JsonUtility.FromJson<CharMapArr>(SaveLoadData.LoadString("charMaps.json")).getArr();
			if(charMaps == null) {
				charMaps = new int[,] { {0,0} };
				Debug.LogError("Loaded null charMaps array: created default array");
			}
			chars = new Dictionary<int, float[][,]>();
			initF();
        }}

    public static void initF() {
        if(inited) {
            return;
        }
        inited = true;
        log += "Log for charMap loading; Started at: " + DateTime.Now.ToString("h:mm:ss tt");
        log += "\nNumber of maps: " + charMaps.GetLength(0);
        string logFile = "charMapLog.log";
        for(int i = 0; i < charMaps.GetLength(0); i++) {
            int h = charMaps[i,0];
            int w = charMaps[i,1];
            int h2 = charMaps[i,2];
            if(h == 0 || w == 0) {
                log += "\nSkiping 0x0 CharMap";
                SaveLoadData.SaveString(logFile,log);
                break;
            }
            string path = "CharMaps/CharMap-" + h;
            log += "\nLoading CharMap {h=" + h + "; w=" + w + "; path=\"" + path + "\"}";
            SaveLoadData.SaveString(logFile,log);
            Texture2D map = Resources.Load<Texture2D>(path);
            chars.Add(h, new float[256][,]);
            for(int cy = 0; cy < 16; cy++) {
                for(int cx = 0; cx < 16; cx++) {
                    int c = cx+(cy*16);
                    if(detailLog) {
                        log += "\n\t- Loading Char {x=" + cx + "; y=" + cy + "; c=" + c + "; char=\'" + visChar((char)c) + "\'}";
                        SaveLoadData.SaveString(logFile,log);
                    }
                    chars[h][c] = new float[h2,w];
                    int x = cx*w;
                    int y = cy*h2;
                    for(int xo = 0; xo < w; xo++) {
                        for(int yo = 0; yo < h2; yo ++) {
                            chars[h][c][yo,xo] = map.GetPixel(x+xo,(h2*16-1)-(y+yo)).grayscale;
                        }
                    }
                }
            }
            log += "\nFinished Loading CharMap {path=\"" + path + "\"}";
            SaveLoadData.SaveString(logFile,log);
        }
        log += "\nFinished Loading CharMaps at: " + DateTime.Now.ToString("h:mm:ss tt");
        SaveLoadData.SaveString(logFile,log);
    }

    public static string visChar(char c) {
        if(c == '\n')
            return "\\n";
        if(c == '\t')
            return "\\t";
        if(c == '\r')
            return "\\r";
        if(c == '\f')
            return "\\f";
        if(c == '\0')
            return "null";
        if(c == '\a')
            return "\\a";
        if(c == '\v')
            return "\\v";
        if(c == '\b')
            return "\\b";
        return c+"";
    }

    public static void initL() {
        if(inited) {
            return;
        }
        inited = true;
        chars.Add(7, new float[256][,]);
        // chars[7] = ;
    	chars[7][0] = new float[,] { // Missing char
    		{1,1,1,1,1},
    		{1,0,0,0,1},
    		{1,1,0,1,1},
    		{1,0,1,0,1},
    		{1,1,0,1,1},
    		{1,0,0,0,1},
    		{1,1,1,1,1}
    	};
    	chars[7][10] = new float[,] { // /n
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0}
    	};
        chars[7][32] = new float[,] { // space
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0}
    	};
    	chars[7][65] = new float[,] { // A
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,1,1,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1}
    	};
    	chars[7][66] = new float[,] { // B
    		{1,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,1,1,0}
    	};
    	chars[7][67] = new float[,] { // C
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][68] = new float[,] { // D
    		{1,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,1,1,0}
    	};
    	chars[7][69] = new float[,] { // E
    		{1,1,1,1,1},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,1,1,1,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,1,1,1,1}
    	};
    	chars[7][70] = new float[,] { // F
    		{1,1,1,1,1},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,1,1,1,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0}
    	};
    	chars[7][71] = new float[,] { // G
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,1,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][72] = new float[,] { // H
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,1,1,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1}
    	};
    	chars[7][73] = new float[,] { // I
    		{0,1,1,1,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,1,1,1,0}
    	};
    	chars[7][74] = new float[,] { // J
    		{0,0,1,1,1},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{1,0,0,1,0},
    		{0,1,1,0,0}
    	};
    	chars[7][75] = new float[,] { // K
    		{1,0,0,0,1},
    		{1,0,0,1,0},
    		{1,0,1,0,0},
    		{1,1,0,0,0},
    		{1,0,1,0,0},
    		{1,0,0,1,0},
    		{1,0,0,0,1}
    	};
    	chars[7][76] = new float[,] { // L
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,1,1,1,1}
    	};
    	chars[7][77] = new float[,] { // M
    		{1,0,0,0,1},
    		{1,1,0,1,1},
    		{1,0,1,0,1},
    		{1,0,1,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1}
    	};
    	chars[7][78] = new float[,] { // N
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,0,0,1},
    		{1,0,1,0,1},
    		{1,0,0,1,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1}
    	};
    	chars[7][79] = new float[,] { // O
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][80] = new float[,] { // P
    		{1,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,1,1,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0}
    	};
    	chars[7][81] = new float[,] { // Q
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,1,0,1},
    		{1,0,0,1,0},
    		{0,1,1,0,1}
    	};
    	chars[7][82] = new float[,] { // R
    		{1,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,1,1,0},
    		{1,0,1,0,0},
    		{1,0,0,1,0},
    		{1,0,0,0,1}
    	};
    	chars[7][83] = new float[,] { // S
    		{0,1,1,1,0},
    		{1,0,0,0,0},
    		{1,0,0,0,0},
    		{0,1,1,1,0},
    		{0,0,0,0,1},
    		{0,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][84] = new float[,] { // T
    		{1,1,1,1,1},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0}
    	};
    	chars[7][85] = new float[,] { // U
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][86] = new float[,] { // V
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,0,1,0},
    		{0,1,0,1,0},
    		{0,0,1,0,0}
    	};
    	chars[7][87] = new float[,] { // W
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,0,1,0,1},
    		{1,0,1,0,1},
    		{1,0,1,0,1},
    		{0,1,0,1,0}
    	};
    	chars[7][88] = new float[,] { // X
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,0,1,0},
    		{0,0,1,0,0},
    		{0,1,0,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1}
    	};
    	chars[7][89] = new float[,] { // Y
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,0,1,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0}
    	};
    	chars[7][90] = new float[,] { // Z
    		{1,1,1,1,1},
    		{0,0,0,0,1},
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0},
    		{1,0,0,0,0},
    		{1,1,1,1,1}
    	};
    	chars[7][48] = new float[,] { // 0
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,1,1},
    		{1,0,1,0,1},
    		{1,1,0,0,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][49] = new float[,] { // 1
    		{0,0,1,0,0},
    		{0,1,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,1,1,1,0}
    	};
    	chars[7][50] = new float[,] { // 2
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{0,0,0,0,1},
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0},
    		{1,1,1,1,1}
    	};
    	chars[7][51] = new float[,] { // 3
    		{1,1,1,1,1},
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,0,0,1,0},
    		{0,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][52] = new float[,] { // 4
    		{0,0,0,1,0},
    		{0,0,1,1,0},
    		{0,1,0,1,0},
    		{1,0,0,1,0},
    		{1,1,1,1,1},
    		{0,0,0,1,0},
    		{0,0,0,1,0}
    	};
    	chars[7][53] = new float[,] { // 5
    		{1,1,1,1,1},
    		{1,0,0,0,0},
    		{1,1,1,1,0},
    		{0,0,0,0,1},
    		{0,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][54] = new float[,] { // 6
    		{0,0,1,1,0},
    		{0,1,0,0,0},
    		{1,0,0,0,0},
    		{1,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][55] = new float[,] { // 7
    		{1,1,1,1,1},
    		{0,0,0,0,1},
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0}
    	};
    	chars[7][56] = new float[,] { // 8
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{0,1,1,1,0}
    	};
    	chars[7][57] = new float[,] { // 9
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{1,0,0,0,1},
    		{1,1,1,1,1},
    		{0,0,0,0,1},
    		{0,0,0,1,0},
    		{0,1,1,0,0}
    	};
    	chars[7][46] = new float[,] { // .
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,1,0,0,0}
    	};
    	chars[7][45] = new float[,] { // -
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,1,1,1,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0}
    	};
    	chars[7][58] = new float[,] { // :
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,1,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,1,0,0},
    		{0,0,0,0,0}
    	};
    	chars[7][59] = new float[,] { // ;
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,1,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0}
    	};
    	chars[7][33] = new float[,] { // !
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,0,0,0},
    		{0,0,1,0,0}
    	};
    	chars[7][63] = new float[,] { // ?
    		{0,1,1,1,0},
    		{1,0,0,0,1},
    		{0,0,0,0,1},
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,0,0,0,0},
    		{0,0,1,0,0}
    	};
    	chars[7][43] = new float[,] { // +
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,1,0,0},
    		{0,1,1,1,0},
    		{0,0,1,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0}
    	};
    	chars[7][42] = new float[,] { // *
    		{0,1,0,1,0},
    		{0,0,1,0,0},
    		{0,1,0,1,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0}
    	};
    	chars[7][47] = new float[,] { // /
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0}
    	};
    	chars[7][92] = new float[,] { // \
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0}
    	};
    	chars[7][60] = new float[,] { // <
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0},
    		{1,0,0,0,0},
    		{0,1,0,0,0},
    		{0,0,1,0,0},
    		{0,0,0,1,0}
    	};
    	chars[7][62] = new float[,] { // >
    		{0,1,0,0,0},
    		{0,0,1,0,0},
    		{0,0,0,1,0},
    		{0,0,0,0,1},
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0}
    	};
    	chars[7][38] = new float[,] { // &
    		{0,1,1,0,0},
    		{1,0,0,1,0},
    		{1,0,0,1,0},
    		{0,1,1,0,1},
    		{1,0,0,1,0},
    		{1,0,0,1,0},
    		{0,1,1,0,1}
    	};
    	chars[7][124] = new float[,] { // |
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0}
    	};
    	chars[7][61] = new float[,] { // =
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,1,1,1,0},
    		{0,0,0,0,0},
    		{0,1,1,1,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0}
    	};
    	chars[7][40] = new float[,] { // (
    		{0,0,1,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,0,1,0,0}
    	};
    	chars[7][41] = new float[,] { // )
    		{0,0,1,0,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,1,0,0}
    	};
    	chars[7][94] = new float[,] { // ^
    		{0,0,1,0,0},
    		{0,1,0,1,0},
    		{1,0,0,0,1},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0}
    	};
    	chars[7][37] = new float[,] { // %
    		{1,1,0,0,0},
    		{1,1,0,0,1},
    		{0,0,0,1,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0},
    		{1,0,0,1,1},
    		{0,0,0,1,1}
    	};
    	chars[7][35] = new float[,] { // #
    		{0,0,0,0,0},
    		{0,1,0,1,0},
    		{1,1,1,1,1},
    		{0,1,0,1,0},
    		{1,1,1,1,1},
    		{0,1,0,1,0},
    		{0,0,0,0,0}
    	};
    	chars[7][44] = new float[,] { // ,
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,0,0,0},
    		{0,0,1,0,0},
    		{0,0,1,0,0},
    		{0,1,0,0,0}
    	};
    	chars[7][123] = new float[,] { // {
    		{0,0,1,1,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{1,0,0,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,0,1,1,0}
    	};
    	chars[7][125] = new float[,] { // }
    		{0,1,1,0,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,0,1},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,1,1,0,0}
    	};
    	chars[7][91] = new float[,] { // [
    		{0,1,1,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,1,0,0,0},
    		{0,1,1,0,0}
    	};
    	chars[7][93] = new float[,] { // ]
    		{0,0,1,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,0,1,0},
    		{0,0,1,1,0}
    	};
    }
}

[Serializable]
public class CharMapArr {
    public int[] maps;
    public int[] size;

    public CharMapArr(int[,] maps_) {
        size = new int[] {maps_.GetLength(0), maps_.GetLength(1)};
        maps = new int[size[0]*size[1]];
        for(int i = 0; i < size[0]; i++) {
            for(int j = 0; j < size[1]; j++) {
                maps[j+(i*size[1])] = maps_[i,j];
            }
        }
    }

    public int[,] getArr() {
        if(size == null || maps == null) {
            return null;
        }
        int[,] maps_ = new int[size[0],size[1]];
        for(int i = 0; i < size[0]; i++) {
            for(int j = 0; j < size[1]; j++) {
                maps_[i,j] = maps[j+(i*size[1])];
            }
        }
        return maps_;
    }
}
