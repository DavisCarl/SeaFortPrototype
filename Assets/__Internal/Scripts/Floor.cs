using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour, Identifiable
{
    [SerializeField]
    private string id;
    public string ID
    {
        get {return id; }
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
}
