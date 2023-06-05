using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour, IMotor
{
    public Transform cameraJoint;
    public float force = 30;
    public Rigidbody playerBody;
    public float maxSpeed = 20;
    public float lookSpeed = 3;
    public Camera cam;
    public bool indoors = false;
    private bool indoorCheck = false;
    public LayerMask indoor, outdoor;
    private Vector2 rotation = Vector2.zero;

    #region Actions
    public void Rotate() // Look rotation (UP down is Camera) (Left right is Transform rotation)
    {
        rotation.y += PlayerInput.MouseX;
        rotation.x += -PlayerInput.MouseY;
        rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        transform.eulerAngles = new Vector2(0, rotation.y) * lookSpeed;
        cameraJoint.localRotation = Quaternion.Euler(rotation.x * lookSpeed, 0, 0);
    }
    public void Move()
    {
        Vector2 v = new Vector2(PlayerInput.KeyX, PlayerInput.KeyY);
        if (v.SqrMagnitude() > .01f)
        {
            if (playerBody.velocity.sqrMagnitude < maxSpeed * maxSpeed) { playerBody.AddRelativeForce(new Vector3(v.x * force, 0, v.y * force), ForceMode.Acceleration); }
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
        PlayerInput.baseMotor = this;
        PlayerInput.SetPlayerTransform(transform);
    }

    #endregion
    #region Unity Methods
    void Start()
    {
        RegisterMotor();
    }
    void Update()
    {
        if(indoors != indoorCheck) { indoorCheck = indoors; if (indoors) { cam.cullingMask = indoor; } else { cam.cullingMask = outdoor; } }
    }
    #endregion
}
