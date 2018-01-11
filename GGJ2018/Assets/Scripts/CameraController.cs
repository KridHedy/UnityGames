
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public Transform target;

    public float distance = 5.0f;

    public float maxDistance = 20;
    public float minDistance = .6f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public int yMinLimit = -80;
    public int yMaxLimit = 80;

    public int zoomRate = 40;

    public float rotationDampening = 3.0f;
    public float zoomDampening = 5.0f;

    private float targetHeight = 1.7f;
    private float x = 0.0f;
    private float y = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;

        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        targetHeight = target.localScale.y;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void Update()
    {

        // Don't do anything if target is not defined
        if (!target)
            return;

        // The mouse govern camera position
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        
        // Zooming
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);

    }

    /**
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */
    private void LateUpdate()
    {
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // Set camera rotation
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // Calculate the desired distance
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        correctedDistance = desiredDistance;

        // Calculate desired camera position
        Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + new Vector3(0, targetHeight, 0));

        // Check for collision using the true target's desired registration point as set by user using height
        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(target.position.x, target.position.y + targetHeight, target.position.z);

        // If there was a collision, correct the camera position and calculate the corrected distance
        bool isCorrected = false;

        if (Physics.Linecast(trueTargetPosition, position, out collisionHit))
        {
            if (Vector3.Distance(trueTargetPosition, transform.position) < Vector3.Distance(trueTargetPosition, position))
                position = collisionHit.point;
            correctedDistance = Vector3.Distance(trueTargetPosition, position);
            isCorrected = true;
        }

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
        if (!isCorrected || correctedDistance >= currentDistance)
                desiredDistance = Mathf.Lerp(desiredDistance, correctedDistance, Time.deltaTime * zoomDampening);
            else
                desiredDistance = correctedDistance;

        // Recalculate position based on the new currentDistance
        position = target.position - (rotation * Vector3.forward * desiredDistance + new Vector3(0, -targetHeight / 1.5f, 0));

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
