using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipMotor : MonoBehaviour, IMotor
{
    public float force = 30;
    public Rigidbody aiBody;
    public float maxSpeed = 20;
    public float lookSpeed = 10;
    public float deadZone = 15;
    public Rigidbody target;
    private bool tryToAlign = false;
    private Vector2 rotation = Vector2.zero;
    private Vector3 projectedLocation;
    private bool freeLook = false;
    #region Actions
    public void Rotate() // Look rotation (UP down is Camera) (Left right is Transform rotation)
    {
        rotation.y += PlayerInput.MouseX;
        rotation.x += -PlayerInput.MouseY;
        rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
    }
    public void Move()
    {
        Vector2 v = new Vector2(0, 1);
        if (v.SqrMagnitude() > .01f)
        {
            if (aiBody.velocity.sqrMagnitude < maxSpeed * maxSpeed) { aiBody.AddRelativeForce(new Vector3(v.x * force, 0, v.y * force), ForceMode.Acceleration); }
        }
    }

    public void RegisterMotor()
    {
        AIManager.Register(this);
    }
    public void DeregisterMotor()
    {
        AIManager.Deregister(this);
    }
    private Vector3 PredictLocation()
    {
        return projectedLocation = (target.velocity.sqrMagnitude / aiBody.velocity.sqrMagnitude) * target.velocity + target.position;
    }
    public void Track()
    {
        Quaternion q = Quaternion.RotateTowards(transform.rotation, CalculateRotation(), lookSpeed * Time.deltaTime);
        transform.rotation = q;
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

    #endregion
    #region Unity Methods
    void Start()
    {
        RegisterMotor();
    }
    public void AIUpdate()
    {
        if (target)
        {
            PredictLocation();
            Track();
        }
        Move();
    }
    #endregion
}
