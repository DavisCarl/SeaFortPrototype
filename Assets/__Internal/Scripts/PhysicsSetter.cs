using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSetter : MonoBehaviour
{
    public Vector3 gravity;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = gravity;
    }

}
