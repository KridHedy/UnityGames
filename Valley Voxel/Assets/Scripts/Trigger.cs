using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
    public GameObject GameObject;
    public Rotate rot;
	// Use this for initialization
	void Start () {
       rot.enabled = false;
    }
	
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rot.enabled = true;
        
            Debug.Log("DETECTED");
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rot.enabled = false;
            rot.rotatingClockWise = !rot.rotatingClockWise;
            Debug.Log("DETECTED OUT");
        }

    }

}
