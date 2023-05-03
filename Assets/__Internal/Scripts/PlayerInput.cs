using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool lockMouse = true;
    public string savePath;
    public static float MouseX, MouseY, KeyX, KeyY;
    public static Transform lookTransform;
    public static Vector3 lookPoint;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = SetState(lockMouse);
    }

    CursorLockMode SetState(bool input)
    {
        if (input)
        {
            return CursorLockMode.Locked;
        }
        else
        {
            return CursorLockMode.None;
        }
    }
    public static RaycastHit? GetLook()
    {
        RaycastHit newHit;
        if (hit.HasValue)
        {
            return hit.Value;
        }
         if (Camera.main)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out newHit))
                    {
                        hit = newHit;
                        return hit;
                    }
                }
        return null;
    }
    static RaycastHit? hit;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            lockMouse = !lockMouse;
        }
        Cursor.lockState = SetState(lockMouse);
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        KeyX = Input.GetAxis("Horizontal");
        KeyY = Input.GetAxis("Vertical");
        hit = null;
        if (Input.GetKeyDown(KeyCode.F1)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.New(); }
        if (Input.GetKeyDown(KeyCode.F5)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.Save(); }
        if (Input.GetKeyDown(KeyCode.F9)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.Load(); }
    }
}
