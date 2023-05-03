using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public Transform cameraJoint;
    public float force = 30;
    public Rigidbody playerBody;
    public float maxSpeed = 20;
    bool mouseFree = false;
    bool blockInput = false;
    public float lookSpeed = 3;
    private Vector2 rotation = Vector2.zero;
    // Update is called once per frame
    void Update()
    {
        if (!blockInput)
        {
            if (!mouseFree)
            {
                Look();
            }
            Move();
        }
    }
    #region Actions
    public void Look() // Look rotation (UP down is Camera) (Left right is Transform rotation)
    {
        rotation.y += PlayerInput.MouseX;
        rotation.x += -PlayerInput.MouseY;
        rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        transform.eulerAngles = new Vector2(0, rotation.y) * lookSpeed;
        cameraJoint.localRotation = Quaternion.Euler(rotation.x * lookSpeed, 0, 0);
    }
    void Move()
    {
        Vector2 v = new Vector2(PlayerInput.KeyX, PlayerInput.KeyY);
        if (v.SqrMagnitude() > .01f)
        {
            if(playerBody.velocity.sqrMagnitude < maxSpeed * maxSpeed){ playerBody.AddRelativeForce(new Vector3(v.x * force, 0, v.y * force), ForceMode.Acceleration); }
        }
    }
    #endregion
}
