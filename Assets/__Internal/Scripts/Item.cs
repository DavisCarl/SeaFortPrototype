using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IIdentifiable
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
    public void Init()
    {
        if (personalID == null)
        {
            personalID = "Item-" + transform.root.childCount;
        }
        foreach (IStoredData sd in transform.GetComponentsInChildren<IStoredData>())
        {
            sd.personalID = personalID;
        }
        WorldRoot root = FindObjectOfType<WorldRoot>();
        root.Register(this);
    }
    #endregion
    public string humanName;
}
