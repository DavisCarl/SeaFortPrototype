using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerShipMotor : MonoBehaviour, IMotor
{
    public Transform cameraJoint;
    public float force = 30;
    public Rigidbody playerBody;
    public float maxSpeed = 20;
    public float lookSpeed = 10;
    public float deadZone = 15;
    public bool usePID = false;
    private bool tryToAlign = false;
    private Vector2 rotation = Vector2.zero;
    private Vector3 projectedLocation;
    private bool freeLook = false;
    private readonly VectorPID angularVelocityController = new VectorPID(33.7766f, 0, 0.2553191f);
    private readonly VectorPID headingController = new VectorPID(9.244681f, 0, 0.06382979f);
    #region Actions
    public void Rotate() // Look rotation (UP down is Camera) (Left right is Transform rotation)
    {
        rotation.y += PlayerInput.MouseX;
        rotation.x += -PlayerInput.MouseY;
        //rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        cameraJoint.localRotation = Quaternion.Euler(rotation.x * lookSpeed, rotation.y*lookSpeed, 0);
        
    }
    public void Move()
    {
        Vector2 v = new Vector2(PlayerInput.KeyX, PlayerInput.KeyY);
        if (v.SqrMagnitude() > .01f)
        {
            if (playerBody.velocity.sqrMagnitude < maxSpeed * maxSpeed) { playerBody.AddRelativeForce(new Vector3(v.x * force, 0, v.y * force), ForceMode.Acceleration); }
        }
        float f = PlayerInput.KeyZ;
        if (Mathf.Abs(f) > .01f)
        {
            Debug.Log("Rolling");
            playerBody.AddRelativeTorque(new Vector3(0, 0, f * force), ForceMode.Force);
        }
    }
    void OnEnable()
    {
        RegisterMotor();
    }
    public void RegisterMotor()
    {
        PlayerInput.SetMotor(this);
        PlayerInput.SetPlayerTransform(transform);
        cameraJoint.parent = null;
    }

    private Vector3 PredictLocation()
    {
        return projectedLocation = Camera.main.transform.position + Camera.main.transform.forward * 100;
    }
    public Vector3 maxTorque = new Vector3(10, 40, 5);
    public Vector3 maxAngularVelocity = new Vector3(10, 2, 5);
    private Vector3 MaxAVDeg2Rad()
    {
        Vector3 v = maxAngularVelocity;
        v.x = v.x * Mathf.Deg2Rad;
        v.y = v.y * Mathf.Deg2Rad;
        v.z = v.z * Mathf.Deg2Rad;
        return v;
    }
    private Vector3 ClampTorque(Vector3 v) 
    { 
        v.x = Mathf.Min(v.x, maxTorque.x);
        if(Mathf.Abs(playerBody.angularVelocity.x) > MaxAVDeg2Rad().x) { v.x = 0; }
        v.y = Mathf.Min(v.y, maxTorque.y);
        if (Mathf.Abs(playerBody.angularVelocity.y) > MaxAVDeg2Rad().y) { v.y = 0; }
        v.z = Mathf.Min(v.z, maxTorque.z);
        if (Mathf.Abs(playerBody.angularVelocity.z) > MaxAVDeg2Rad().z) { v.z = 0; }
        return v;
    }
    public void Track()
    {
        if (usePID)
        {
            var angularVelocityError = playerBody.angularVelocity * -1;

            Debug.DrawRay(transform.position, playerBody.angularVelocity * 10, Color.black);

            var angularVelocityCorrection = angularVelocityController.Update(angularVelocityError, Time.deltaTime);
            Debug.DrawRay(transform.position, angularVelocityCorrection, Color.green);

            var desiredHeading = projectedLocation - transform.position;
            Debug.DrawRay(transform.position, desiredHeading, Color.magenta);

            var currentHeading = transform.forward;
            Debug.DrawRay(transform.position, currentHeading * 150, Color.blue);

            var headingError = Vector3.Cross(currentHeading, desiredHeading);
            var headingCorrection = headingController.Update(headingError, Time.deltaTime);
            Debug.DrawRay(transform.position, headingCorrection, Color.red);
            var totalTorque = headingCorrection + angularVelocityCorrection;
            Debug.DrawRay(transform.position, totalTorque, Color.white);
            playerBody.AddTorque(ClampTorque(totalTorque), ForceMode.Force);
        }
        else 
        {
            Quaternion q = Quaternion.RotateTowards(transform.rotation, CalculateRotation(), lookSpeed * Time.deltaTime);
            transform.rotation = q;
        }
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
    void Update()
    {
        if (Vector3.Distance(cameraJoint.forward, transform.forward) > deadZone && !freeLook)
        {

            PredictLocation();
            Track();
        }
        cameraJoint.position = transform.position;
        if (Input.GetKey(KeyCode.C)) { freeLook = true; } else { freeLook = false; }
    }
    #endregion
}
