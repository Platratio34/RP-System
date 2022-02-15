using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Log {
    
    public string timeStart;
    public string name;
    private string log;
    public string format = "dd/MM/yyyy HH:mm:ss.ff";
    public string saveLocation = "log";

    public Log(string name) {
        timeStart = GetTime();
        this.name = name;
        log = timeStart + ": " + name;
    }

    public void LogMsg(string msg) {
        string t = GetTime();
        log += "\n" + t + ": LOG: " + msg;
    }

    public void Error(string errorType, string msg) {
        string t = GetTime();
        log += "\n" + t + ": ERROR: " + errorType + "; Msg: " + msg;
    }
    public void Warn(string msg) {
        string t = GetTime();
        log += "\n" + t + ": WARN: " + msg;
    }

    public void Save() {
        Save(saveLocation);
    }
    public void Save(string filename) {
        string str = log;
        str += "\n\nTime Saved: " + GetTime();
        SaveLoadData.SaveString("Logs/" + filename + ".log", str);
    }

    private string GetTime() {
        return DateTime.Now.ToString(format);
    }
}
