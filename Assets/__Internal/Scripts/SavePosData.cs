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
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
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
    public Quaternion ToQuaternion() { return new Quaternion(x, y, z, w);}
}

public class SavePosData
{
    public Vector3Data pos;
    public QuaternionData rot;
    public string ID;
    public string personalID;
    public Vector3 ToVector() { return pos.ToVector3(); }
    public Quaternion ToQuaternion() { return rot.ToQuaternion(); }
    public SavePosData(IIdentifiable i)
    {
        pos = new Vector3Data(i.Transform.position);
        rot = new QuaternionData(i.Transform.rotation);
        ID = i.ID;
        personalID = i.personalID;
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
