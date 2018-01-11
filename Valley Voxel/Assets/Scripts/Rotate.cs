using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    private bool rotatingClockWise = false;
    public GameObject obj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (rotatingClockWise && ( transform.eulerAngles.z> 270 || transform.eulerAngles.z<=0))
        {
            transform.Rotate(0, 0, -1);
        }
        else if (!rotatingClockWise && transform.eulerAngles.z>0)
        {

            transform.Rotate(0, 0, 1);
        }

        if (Input.anyKeyDown)
        {
            rotatingClockWise = !rotatingClockWise;
        }
	}
}
