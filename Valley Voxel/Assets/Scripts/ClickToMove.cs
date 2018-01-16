using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour {

    Vector3 targetPostion;
    Vector3 lookAtTarget;
    Quaternion playerRot;

    float rotSpeed = 7f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0))
        {
            SetTargetPosition();
        }
	}

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit ,1000))
        {
            targetPostion = hit.point;
            // this.transform.LookAt(targetPostion);
            lookAtTarget = new Vector3(targetPostion.x - transform.position.x, transform.position.y, targetPostion.z - transform.position.z);
            playerRot = Quaternion.LookRotation(lookAtTarget);
        }
    }

    void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, rotSpeed * Time.deltaTime);
    }
}
