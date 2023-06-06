using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdentifiable
{
    string ID
    {
        get;
        set;
    }
    string personalID
    {
        get;
        set;
    }
    Transform Transform
    {
        get;
    }
    public void Init();
}
