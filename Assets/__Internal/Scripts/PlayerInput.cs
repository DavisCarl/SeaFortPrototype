using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string savePath;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.New(); }
        if (Input.GetKeyDown(KeyCode.F5)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.Save(); }
        if (Input.GetKeyDown(KeyCode.F9)) { WorldRoot root = FindObjectOfType<WorldRoot>(); root.Load(); }
    }
}
