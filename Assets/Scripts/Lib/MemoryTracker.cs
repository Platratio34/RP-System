using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using Unity.Profiling;

public class MemoryTracker : MonoBehaviour {
    
    public Text text;

    private long frame;
    private float[] fTimeArr = new float[120];
    private int fTimeArrInd = 0;

    ProfilerRecorder totalReservedMemoryRecorder;
    ProfilerRecorder gcReservedMemoryRecorder;
    ProfilerRecorder systemUsedMemoryRecorder;

    private long maxMemUsed = 0;
    private float[] memUsedArr = new float[256];
    private int memUsedArrInd = 0;

    void OnEnable()
    {
        // totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        // gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        // systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Managed Heap Used");
    }

    void OnDisable()
    {
        // totalReservedMemoryRecorder.Dispose();
        // gcReservedMemoryRecorder.Dispose();
        // systemUsedMemoryRecorder.Dispose();
    }

    void Start() {
        
    }

    
    void Update() {
        frame++;

        fTimeArr[fTimeArrInd] = Time.deltaTime;
        fTimeArrInd++;
        if(fTimeArrInd >= fTimeArr.Length) {
            fTimeArrInd = 0;
        }
        float fTime = 0;
        for(int i = 0; i < fTimeArr.Length; i++) {
            fTime += fTimeArr[i];
        }
        fTime /= fTimeArr.Length;

        text.text = frame+" " + Math.Round(1f/fTime,1) + "fps\n";
        var sb = new StringBuilder(500);
        // if (totalReservedMemoryRecorder.Valid)
        //     sb.AppendLine($"Total Reserved Memory: {PowerString.convert(totalReservedMemoryRecorder.LastValue)}B");
        // if (gcReservedMemoryRecorder.Valid)
        //     sb.AppendLine($"GC Reserved Memory: {PowerString.convert(gcReservedMemoryRecorder.LastValue)}B");
        // if (systemUsedMemoryRecorder.Valid)
        //     sb.AppendLine($"Managed Heap Used: {PowerString.convert(systemUsedMemoryRecorder.LastValue)}B");
        long memUsed = System.GC.GetTotalMemory(false);
        if(maxMemUsed < memUsed) {
            maxMemUsed = memUsed;
        }

        memUsedArr[memUsedArrInd] = memUsed;
        memUsedArrInd++;
        if(memUsedArrInd >= memUsedArr.Length) {
            memUsedArrInd = 0;
        }
        float avgMemUsed = 0;
        for(int i = 0; i < memUsedArr.Length; i++) {
            avgMemUsed += memUsedArr[i];
        }
        avgMemUsed /= memUsedArr.Length;

        sb.AppendLine($"Managed Heap Used: {PowerString.convert(memUsed)}B");
        sb.AppendLine($"Max Heap Used: {PowerString.convert(maxMemUsed)}B");
        sb.AppendLine($"Avg Heap Used: {PowerString.convert(avgMemUsed)}B");
        text.text += sb.ToString();
    }
}
