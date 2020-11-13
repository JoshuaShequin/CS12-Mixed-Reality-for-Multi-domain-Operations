using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    private int currentCam;
     // Start is called before the first frame update
    void Start()
    {
        cam1.enabled = true;
        cam2.enabled = false;
        cam3.enabled = false;
        currentCam = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            currentCam++;
            switch(currentCam)
            {
                case 2:                 // 2nd camera activated
                    cam1.enabled = false;
                    cam2.enabled = true;
                    cam3.enabled = false;
                    break;
                case 3:                 // 3rd camera activated
                    cam1.enabled = false;
                    cam2.enabled = false;
                    cam3.enabled = true;
                    break;
                default:                // Reset flag and activate 1st
                    cam1.enabled = true;
                    cam2.enabled = false;
                    cam3.enabled = false;
                    currentCam = 1;
                    break;
            }            
        }
        
    }
}
