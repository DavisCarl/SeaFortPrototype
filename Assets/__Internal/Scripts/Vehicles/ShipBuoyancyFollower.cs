using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuoyancyFollower : MonoBehaviour
{
    public Rigidbody buoyancyObject;
    private Transform t;
    [SerializeField] 
    private bool setRotation = false;
    private void Start()
    {
        t = transform;
    }
    Vector3 v;
    // Update is called once per frame
    void Update()
    {
        if (setRotation)
            t.SetPositionAndRotation(buoyancyObject.transform.position, buoyancyObject.transform.rotation);
        else
        {
            t.SetPositionAndRotation(buoyancyObject.transform.position, transform.rotation);
            v = transform.position;
            v.y = WaterController.waterHeight;
            transform.position = v;
        }
    }
}
