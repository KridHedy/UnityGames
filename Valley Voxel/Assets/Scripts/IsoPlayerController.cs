using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoPlayerController : MonoBehaviour {

    public float speed;
    public GameObject[] ar;
    public float aBoxHeight;
    public bool debug;
    public int index;
    private void Start()
    {
        index = 0;
    }

    private void Update()
    {
        if (!debug) return;

        if (Vector3.Distance(transform.position, ar[index].transform.position + aBoxHeight * Vector3.up) < 0.01f) index++;
        index = index >= ar.Length ? 0 : index;

        if (Vector3.Distance(transform.position, ar[index].transform.position + aBoxHeight * Vector3.up) < 2f)
            transform.position = Vector3.MoveTowards(transform.position, ar[index].transform.position + aBoxHeight * Vector3.up, Time.deltaTime * (speed));
        else
            transform.position = ar[index].transform.position + aBoxHeight * Vector3.up;

        transform.LookAt(new Vector3(ar[index].transform.position.x, transform.position.y, ar[index].transform.position.z));
    }
}
