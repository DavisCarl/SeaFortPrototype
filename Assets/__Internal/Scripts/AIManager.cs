using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public bool running = true;
    public static List<AIShipMotor> AIShipMotors = new List<AIShipMotor>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static void Register(AIShipMotor motor)
    {
        if (!AIShipMotors.Contains(motor)) { AIShipMotors.Add(motor); }
    }
    public static void Deregister(AIShipMotor motor)
    {
        if (AIShipMotors.Contains(motor)) { AIShipMotors.Remove(motor); }
    }
    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            foreach(AIShipMotor motor in AIShipMotors) { motor.AIUpdate(); }
        }
    }
}
