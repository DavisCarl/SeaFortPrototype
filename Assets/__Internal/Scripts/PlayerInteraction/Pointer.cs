using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    Transform _transform;
    public Transform transformToPlace;
    bool cursorVis = false;
    bool canPlace = true;
    [SerializeField]
    private string itemToBuild, builtItemToBuild;
    Vector3 placementRotation;
    WorldRoot root;
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
        if (transformToPlace) { foreach (ITickable t in transformToPlace.GetComponentsInChildren<ITickable>()) { root.Deregister(t); } Destroy(transformToPlace.gameObject); }
    }
    private void SetCursorVisible(bool vis)
    {
        if (vis) { foreach (Renderer r in _transform.GetComponentsInChildren<Renderer>()) { r.enabled = true; } }
        else { foreach (Renderer r in _transform.GetComponentsInChildren<Renderer>()) { r.enabled = false; } }
    }

    private void ClearBuildCursor()
    {
        DestroyTTP();
    }
    void SetBuildCursorCollision(bool value)
    {
        foreach (Collider c in transformToPlace.GetComponentsInChildren<Collider>())
        {
            c.enabled = value;
        }
    }
    void SetBuildCursorVisible(bool value)
    {
        foreach (Renderer c in transformToPlace.GetComponentsInChildren<Renderer>())
        {
            c.enabled = value;
        }
        canPlace = value;
    }
    private void PlaceItem()
    {
        if (canPlace)
        {
            transformToPlace.parent = root.transform;
            SetBuildCursorCollision(true);
            var r = transformToPlace.GetComponentsInChildren<IIdentifiable>();
            foreach (IIdentifiable i in r)
            {
                i.Init();
            }
            transformToPlace = null;
        }
    }
    public void GetBuiltItem(string itemName)
    {
        transformToPlace = root.GetBuiltItem(itemName);
        SetBuildCursorCollision(false);
    }
    public void GetItem(string itemName)
    {
        transformToPlace = root.GetItem(itemName);
        SetBuildCursorCollision(false);
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
        if (!root) { root = FindObjectOfType<WorldRoot>(); }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateY(true);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            RotateY(false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ClearBuildCursor();
            GetBuiltItem(builtItemToBuild);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ClearBuildCursor();
            GetItem(itemToBuild);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlaceItem();
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
                var t = transformToPlace.GetComponent<BuiltItem>();
                if (t)
                {
                    if (v.Value.transform.tag == t.GetSurface())
                    {
                        SetBuildCursorVisible(true);
                    }
                    else
                    {
                        SetBuildCursorVisible(false);
                    }
                }
            }
        }
    }
    #endregion
}
