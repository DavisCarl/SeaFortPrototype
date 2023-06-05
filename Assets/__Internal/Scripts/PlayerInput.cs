using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{



    #region Public Vars
    public string savePath;
    public LayerMask currentMask;
    #endregion
    #region Private Vars
    private bool blockInput = false;
    private bool lockMouse = true;
    #endregion
    #region Static Vars
    public static float MouseX, MouseY, KeyX, KeyY, KeyZ;
    public static Transform lookTransform;
    public static Vector3 lookPoint;
    public static string itemToBuild, builtItemToBuild;
    public static IMotor currentMotor;
    public static IMotor baseMotor;
    public static PlayerShipFireControl shipFireControl;
    public static Camera currentCamera;
    public static Transform playerTransform;
    static LayerMask staticMask;
    static RaycastHit? hit;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = SetState(lockMouse);
        staticMask = currentMask;
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
    public static void SetMotor(IMotor motor)
    {
        currentMotor = motor;
    }
    public static void SetCamera(Camera camera)
    {
        if (currentCamera) { currentCamera.enabled = false; }
        currentCamera = camera;
        camera.enabled = true;
    }
    public static void SetPlayerTransform(Transform t)
    {
        playerTransform = t;
    }
    public static RaycastHit? GetLook()
    {

        if (hit.HasValue)
        {
            return hit.Value;
        }
        if (Camera.main)
        {
            RaycastHit newHit;
            if (Physics.Raycast(ray: Camera.main.ScreenPointToRay(Input.mousePosition), hitInfo: out newHit, layerMask: staticMask, maxDistance: 100))
            {
                hit = newHit;
                return hit;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        hit = null;
        staticMask = currentMask;
        #region Motor Input
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            MouseX = 0;
            MouseY = 0;
        }
        KeyX = Input.GetAxis("Horizontal");
        KeyY = Input.GetAxis("Vertical");
        KeyZ = Input.GetAxis("Roll");
        if (!blockInput)
        {
            if (currentMotor != null)
            {
                currentMotor.Rotate();
                currentMotor.Move();
            }
        }
        #endregion
        #region Effector Input
        if (Input.GetMouseButtonDown(0))
        {
            var look = GetLook();
            if (look.HasValue)
            {
                foreach (IInteractable i in look.Value.transform.GetComponentsInParent<IInteractable>())
                {
                    i.Execute(playerTransform);
                }
            }

        }
        if(Input.GetKey(KeyCode.Space)) { if(shipFireControl) shipFireControl.FireAutocannons(); }
        if (Input.GetKey(KeyCode.LeftControl)) { if (shipFireControl) shipFireControl.FireMissiles(); }
        if (Input.GetKeyDown(KeyCode.LeftAlt)) { lockMouse = !lockMouse; Cursor.lockState = SetState(lockMouse); }
        #endregion
        #region Generic Input
#if !UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.F1)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.New(); }
        if (Input.GetKeyDown(KeyCode.F5)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.Save(); }
        if (Input.GetKeyDown(KeyCode.F9)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.Load(); }
#endif
#if UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.Alpha8)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.New(); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.Save(); }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.Load(); }
#endif
        if(Input.GetKeyDown(KeyCode.F)) { baseMotor.RegisterMotor(); }
        #endregion
    }
}
