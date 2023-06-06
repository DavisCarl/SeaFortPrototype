using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltItem : MonoBehaviour, IIdentifiable
{
    private bool wallItem = false;
    public string humanName;
    [SerializeField]
    private string buildSurface = "Wall";
    public string GetSurface()
    {
        return buildSurface;
    }
    public bool isWallItem() { return wallItem; }
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
            personalID = "BuiltItem-" + transform.root.childCount;
        }
        foreach (IStoredData sd in transform.GetComponentsInChildren<IStoredData>())
        {
            sd.personalID = personalID;
        }
        WorldRoot root = FindObjectOfType<WorldRoot>();
        root.Register(this);
    }
    #endregion
}
