using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Identifiable 
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
}
