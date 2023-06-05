using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControlConsole : MonoBehaviour, IInteractable
{
    public PlayerBoatMotor boatMotor;
    public bool Execute(Transform t)
    {
        PlayerInput.baseMotor = t.GetComponent<IMotor>();
        boatMotor.RegisterMotor();
        return true;
    }

}
