using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableTarget : MonoBehaviour, IInteractable
{
    public bool Execute(Transform t)
    {
        Debug.Log("Clicked");
        if (PlayerInput.shipFireControl != null) { PlayerInput.shipFireControl.SetTarget(GetComponent<Rigidbody>()); return true; }
        else return false;
    }
}
