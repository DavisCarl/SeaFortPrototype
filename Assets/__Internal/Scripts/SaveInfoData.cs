using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInfoData
{
    public string personalID;
    public Dictionary<string, float> floats;
    public Dictionary<string, int> ints;
    public SaveInfoData()
    {
    }

    public SaveInfoData(string personalID, Dictionary<string, float> floats, Dictionary<string, int> ints)
    {
        this.personalID = personalID;
        this.floats = floats;
        this.ints = ints;
    }
}
