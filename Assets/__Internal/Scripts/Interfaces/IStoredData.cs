using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoredData
{
    string personalID
    {
        get;
        set;
    }
    void Load(SaveInfoData d);
    SaveInfoData Save();
}
