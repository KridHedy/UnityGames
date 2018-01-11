using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public bool rotatingClockWise = true;
    public float min=0;
    public float max=270;
    
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    
            if (rotatingClockWise && (transform.eulerAngles.z > max || transform.eulerAngles.z <= min))
            {
                transform.Rotate(0, 0, -1);

            }
            else if (!rotatingClockWise && transform.eulerAngles.z > 0)
            {

                transform.Rotate(0, 0, 1);
            }


        
	}
}
