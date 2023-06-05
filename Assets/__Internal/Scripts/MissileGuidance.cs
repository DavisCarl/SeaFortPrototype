using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGuidance : MonoBehaviour
{
    public Missile missileBody;
    private Vector3 projectedLocation;
    public Vector3 PredictLocation()
    {
        return projectedLocation = (missileBody.target.velocity.magnitude / missileBody.velocity) * missileBody.target.velocity + missileBody.target.position;
    }
    public Quaternion CalculateRotation()
    {
        Quaternion rotation;
        Quaternion lookRotation = Quaternion.LookRotation(projectedLocation - transform.position, transform.up);
        Vector3 euler = lookRotation.eulerAngles;
        //euler.x = 0; euler.z = 0;
        rotation = Quaternion.Euler(euler);
        return rotation;
    }
}
