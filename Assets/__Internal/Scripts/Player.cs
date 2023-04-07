using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Identifiable
{
    #region Interfaces
    public string ID
    {
        get;
        set;
    }
    public string personalID
    {
        get;
        set;
    }
    public Transform Transform
    {
        get { return transform; }
    }
    #endregion
}
