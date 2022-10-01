using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// File logger
/// </summary>
public class Log {
    
    /// <summary>
    /// The time the logger was started
    /// </summary>
    public string timeStart;
    /// <summary>
    /// The name of the log (not filename)
    /// </summary>
    public string name;
    /// <summary>
    /// The contents of the log
    /// </summary>
    private string log;
    /// <summary>
    /// The time format string
    /// </summary>
    public string format = "dd/MM/yyyy HH:mm:ss.ff";
    /// <summary>
    /// The filename of the log without extension. .log is automatically used. Can include file path
    /// </summary>
    public string saveLocation = "log";

    /// <summary>
    /// Creates a new logger with name
    /// </summary>
    /// <param name="name"></param>
    public Log(string name) {
        timeStart = GetTime();
        this.name = name;
        log = timeStart + ": " + name;
    }

    /// <summary>
    /// Logs an LOG level message
    /// </summary>
    /// <param name="msg">Message</param>
    public void LogMsg(string msg) {
        string t = GetTime();
        log += "\n" + t + ": LOG: " + msg;
    }

    /// <summary>
    /// Loges an ERROR level message
    /// </summary>
    /// <param name="errorType">The type of error</param>
    /// <param name="msg">Error message</param>
    public void Error(string errorType, string msg) {
        string t = GetTime();
        log += "\n" + t + ": ERROR: " + errorType + "; Msg: " + msg;
    }
    /// <summary>
    /// Logs a WARN level message
    /// </summary>
    /// <param name="msg">Warning</param>
    public void Warn(string msg) {
        string t = GetTime();
        log += "\n" + t + ": WARN: " + msg;
    }

    /// <summary>
    /// Saves the log to file
    /// </summary>
    public void Save() {
        Save(saveLocation);
    }
    /// <summary>
    /// Saves the log to file with name. Does not update filename of logger
    /// </summary>
    /// <param name="filename">Log filename</param>
    public void Save(string filename) {
        string str = log;
        str += "\n\nTime Saved: " + GetTime();
        SaveLoadData.SaveString("Logs/" + filename + ".log", str);
    }

    /// <summary>
    /// Gets the time formatted with format string
    /// </summary>
    /// <returns>Formatted time string</returns>
    private string GetTime() {
        return DateTime.Now.ToString(format);
    }
}
