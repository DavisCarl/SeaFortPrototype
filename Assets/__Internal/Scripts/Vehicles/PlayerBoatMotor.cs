using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoatMotor : MonoBehaviour, IMotor
{
    public Transform cameraJoint;
    public float forwardForce = 30;
    public float torqueForce = 30;
    public Rigidbody playerBody;
    public float maxSpeed = 20;
    public float lookSpeed = 10;
    public float deadZone = 15;
    public Vector2 xLimits = new Vector2(10, 90);
    public Camera cam;
    private Vector2 rotation = Vector2.zero;
    private Vector3 projectedLocation;
    private bool freeLook = false;
    [SerializeField]
    private Transform thruster;
    #region Actions
    public void Rotate() // Look rotation (UP down is Camera) (Left right is Transform rotation)
    {
        rotation.y += PlayerInput.MouseX;
        rotation.x += -PlayerInput.MouseY;
        //rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        cameraJoint.localRotation = Quaternion.Euler(Mathf.Clamp(rotation.x * lookSpeed, xLimits.x, xLimits.y), rotation.y * lookSpeed, 0);

    }
    public void Move()
    {
        Vector2 v = new Vector2(PlayerInput.KeyX, PlayerInput.KeyY);
        if (v.SqrMagnitude() > .01f)
        {
            float waveYPos = WaterController.CalculateWave(thruster.position);
            Vector3 forceVector = (thruster.forward) * forwardForce * v.y;
            
            if (thruster.position.y < waveYPos)
            {
                Debug.DrawRay(thruster.position, forceVector, Color.green);
                if (playerBody.velocity.sqrMagnitude < maxSpeed * maxSpeed) { playerBody.AddForce( forceVector, ForceMode.Impulse); }
                playerBody.AddRelativeTorque(new Vector3(0, torqueForce*v.x, 0), ForceMode.Impulse);
            }
        }
    }
    void OnEnable()
    {
        RegisterMotor();
    }
    public void RegisterMotor()
    {
        PlayerInput.SetMotor(this);
        PlayerInput.SetCamera(cam);
        PlayerInput.SetPlayerTransform(transform);
        cameraJoint.parent = null;
    }

    #endregion
    #region Unity Methods
    void Start()
    {
        RegisterMotor();
    }
    void Update()
    {

        cameraJoint.position = transform.position;
        if (Input.GetKey(KeyCode.C)) { freeLook = true; } else { freeLook = false; }
    }

    public void ResetToSpawn()
    {
        transform.position = Vector3.zero;
    }
    #endregion
}
