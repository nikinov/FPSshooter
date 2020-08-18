using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForse = Vector3.zero;

    [SerializeField] private float cameraRotationLimit = 85;
    [SerializeField] private ConfigurableJoint joint;

    public bool grounded;
    private Vector3 posCur;
    private Quaternion rotCur;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();
    }

    // get movement vector
    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // get rotation vector
    public void Rotate (Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // get rotation vector
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    public void ApplyThruster(Vector3 _thruseterForce)
    {
        thrusterForse = _thruseterForce;
    }

    // run every physics iteration
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    // performe movement based on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            //declare a new Ray. It will start at this object's position and it's direction will be straight down from the object (in local space, that is)
            Ray ray = new Ray(transform.position, -transform.up);
            //decalre a RaycastHit. This is neccessary so it can get "filled" with information when casting the ray below.
            RaycastHit hit;
            //cast the ray. Note the "out hit" which makes the Raycast "fill" the hit variable with information. The maximum distance the ray will go is 1.5
            if (Physics.Raycast(ray, out hit) == true)
            {
                //draw a Debug Line so we can see the ray in the scene view. Good to check if it actually does what we want. Make sure that it uses the same values as the actual Raycast. In this case, it starts at the same position, but only goes up to the point that we hit.
                Debug.DrawLine(transform.position, hit.point, Color.green);
                if (joint.connectedAnchor.y != hit.point.y + 1.5f)
                {
                    joint.connectedAnchor = new Vector3(0, hit.point.y + 1f, 0);
                }
            }
        }

        if (thrusterForse != Vector3.zero)
        {
            rb.AddForce(thrusterForse * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    // performe rotation
    void PerformRotation ()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            // set our rotation and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            // apply our rotation to the transform of our camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }
}
