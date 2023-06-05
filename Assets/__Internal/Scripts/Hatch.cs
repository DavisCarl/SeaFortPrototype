using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hatch : MonoBehaviour, IInteractable
{
    public Transform pointA, pointB;
    public HullRenderController controller;
    public bool aInside, bInside;
    public int floorA, floorB;
    void Start()
    {
        controller = GetComponentInParent<HullRenderController>();
    }
    public bool Execute(Transform target)
    {
        
        Transport(target);
        return true;
    }

    void Transport(Transform target)
    {
        if (Vector3.Distance(pointA.position, target.position) > Vector3.Distance(pointB.position, target.position)) 
        { 
            controller.RenderDeck(floorA);
            controller.RenderHull(!aInside);
            target.position = pointA.position; 
            target.GetComponent<PlayerMotor>().indoors = aInside; 
        } else { 
            controller.RenderDeck(floorB);
            controller.RenderHull(!bInside);
            target.position = pointB.position; 
            target.GetComponent<PlayerMotor>().indoors = bInside; }
    }

}
