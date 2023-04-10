using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Identifiable
{
    #region Interfaces
    [SerializeField]
    private string id;
    public string ID
    {
        get { return id; }
        set { id = value; }
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
