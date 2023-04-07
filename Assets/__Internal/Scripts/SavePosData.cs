using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Vector3Data
{
    public float x, y, z;
    public Vector3Data(Vector3 inV)
    {
        x = inV.x;
        y = inV.y;
        z = inV.z;
    }
}
public class QuaternionData
{
    public float w, x, y, z;
    public QuaternionData(Quaternion inV)
    {
        w = inV.w;
        x = inV.x;
        y = inV.y;
        z = inV.z;
    }
}

public class SavePosData
{
    public Vector3Data pos;
    public QuaternionData rot;
    public string ID;
    public string personalID;
    public Vector3 ToVector() { return new Vector3(pos.x, pos.y, pos.z); }
    public Quaternion ToQuaternion() { return new Quaternion(rot.x, rot.y, rot.z, rot.w); }
    public SavePosData(Identifiable i)
    {
        pos = new Vector3Data(i.Transform.position);
        rot = new QuaternionData(i.Transform.rotation);
        ID = i.ID;
        personalID = i.ID;
    }
    

    public SavePosData(Vector3Data pos, QuaternionData rot, string iD)
    {
        this.pos = pos;
        this.rot = rot;
        ID = iD;
    }

    public SavePosData()
    {
    }
}
