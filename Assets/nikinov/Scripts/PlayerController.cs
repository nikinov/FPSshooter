using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 5f;
    [SerializeField] private float thrusterForce = 1000f;

    [Header("Spring settings:")]
    [SerializeField] private JointDriveMode JointMode;
    [SerializeField] private float JointSpring = 20f;
    [SerializeField] private float JointMaxForse = 40f;
    [SerializeField] private DynamicJoystick variableJoystick;
    [SerializeField] private string m_DeviceType;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(JointSpring);
        //Check if the device running this is a desktop
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            m_DeviceType = "Desktop";
            variableJoystick.gameObject.SetActive(false);
        }

        //Check if the device running this is a handheld
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            m_DeviceType = "Handheld";
            variableJoystick.gameObject.SetActive(true);
        }

        Debug.Log("Device type : " + m_DeviceType);
    }
    private void Update()
    {

        float xMov = 0;
        float zMov = 0;
        if (m_DeviceType == "Desktop")
        { 
            xMov = Input.GetAxisRaw("Horizontal");
            zMov = Input.GetAxisRaw("Vertical");
        }
        else if (m_DeviceType == "Handheld")
        {
            xMov = variableJoystick.Direction.x;
            zMov = variableJoystick.Direction.y;
            variableJoystick.gameObject.SetActive(true);
        }
        else
        {
            print("unknown Device");
        }

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        // final movement vector
        Vector3 _velocity = (moveHorizontal + moveVertical).normalized * speed;

        // apply movement
        motor.Move(_velocity);

        // calculate rotation as a 3D vector
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;

        // apply rotation
        motor.Rotate(_rotation);

        // calculate camera rotation as a 3D vector
        float xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = xRot * lookSensitivity;

        // apply camera rotation
        motor.RotateCamera(_cameraRotationX);

        // calculate the thruster force based on player input
        Vector3 _thrusterForce = Vector3.zero;

        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(JointSpring);
        }

        // apply thruster forse
        motor.ApplyThruster(_thrusterForce);

    }

    private void SetJointSettings (float _JointSpring)
    {
        joint.yDrive = new JointDrive { mode = JointMode, positionSpring = _JointSpring, maximumForce = JointMaxForse};
    }
}
