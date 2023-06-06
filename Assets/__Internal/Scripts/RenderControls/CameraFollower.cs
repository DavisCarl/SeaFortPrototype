using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Camera cameraObject;
    private Transform t;
    [SerializeField]
    private bool setRotation = false;
    private void Start()
    {
        t = transform;
    }
    Vector3 v;
    // Update is called once per frame
    void Update()
    {
        cameraObject = PlayerInput.currentCamera;
        if (cameraObject)
        {
            if (setRotation)
                t.SetPositionAndRotation(cameraObject.transform.root.position, cameraObject.transform.root.rotation);
            else
            {
                t.SetPositionAndRotation(cameraObject.transform.root.position, transform.rotation);
                v = transform.position;
                v.y = WaterController.waterHeight;
                transform.position = v;
            }
        }
    }
}
