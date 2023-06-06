using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInfoData
{
    public string personalID;
    public string serializedData;
    public SaveInfoData()
    {
    }

    public SaveInfoData(string personalID, string data)
    {
        this.personalID = personalID;
        this.serializedData = data;
    }
}
