using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    Transform _transform;
    public Transform transformToPlace;
    bool cursorVis = false;
    Vector3 placementRotation;
    #region Transform Modifications
    public void RotateX(bool dir)
    {
        if (dir)
        {
            placementRotation.x += 90;
        }
        else
        {
            placementRotation.x -= 90;
        }
    }
    public void RotateY(bool dir)
    {
        if (dir)
        {
            placementRotation.y += 90;
        }
        else
        {
            placementRotation.y -= 90;
        }
    }
    public void RotateZ(bool dir)
    {
        if (dir)
        {
            placementRotation.z += 90;
        }
        else
        {
            placementRotation.z -= 90;
        }
    }
    public void DestroyTTP()
    {
        if (transformToPlace) { Destroy(transformToPlace.gameObject); }
    }
    private void SetCursorVisible(bool vis)
    {
        if (vis) { foreach (Renderer r in _transform.GetComponentsInChildren<Renderer>()) { r.enabled = true; } }
        else { foreach (Renderer r in _transform.GetComponentsInChildren<Renderer>()) { r.enabled = false; } }
    }
    #endregion
    #region Unity Methods 
    private void Start()
    {
        _transform = transform;
        placementRotation = new Vector3();
        SetCursorVisible(cursorVis);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateX(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateX(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateY(true);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            RotateY(false);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RotateZ(true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            RotateZ(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            cursorVis = !cursorVis;
            SetCursorVisible(cursorVis);
        }
    }
    void LateUpdate()
    {
        var v = PlayerInput.GetLook();
        if (v.HasValue) 
        {
            _transform.SetPositionAndRotation(v.Value.point, Quaternion.FromToRotation(Vector3.up, v.Value.normal));
            if (transformToPlace)
            {
                transformToPlace.SetPositionAndRotation(v.Value.point, Quaternion.FromToRotation(Vector3.up, v.Value.normal));
                transformToPlace.Rotate(placementRotation);
            }
        }
    }
    #endregion
}
