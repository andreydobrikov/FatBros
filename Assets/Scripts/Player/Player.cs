using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Variables

    public float speed_X = 3f;
    public float speed_Z = 3f;
    public enum ControlTypes { Keybord, VirtualJoystick }
    public ControlTypes controlType = ControlTypes.Keybord;

    private static Transform instance = null;

    private CharacterController controller = null;
    private Vector3 moveDirection = Vector3.zero;    
    private VCAnalogJoystickBase rotateJoystick = null, moveJoystick = null;

    #endregion

    #region Properties

    public static Transform Instance
    {
        get 
        {
            if (Player.instance == null)
            {
                Debug.LogError("Player.instance isn't assigned!!!");
                Debug.Break();
            }

            return Player.instance;
        }
        private set { Player.instance = value; }
    }

    #endregion

    #region Unity functions

    private void Awake()
    {
        controller = this.GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("Unit.Start() " + this.name + " 'controller' component isn't found!");
            Debug.Break();
            return;
        }

        Instance = this.transform;
    }

    private void Start()
    {
        rotateJoystick = VCAnalogJoystickBase.GetInstance("RotateJoystic");
        moveJoystick = VCAnalogJoystickBase.GetInstance("MoveJoystic");
        if (rotateJoystick == null || moveJoystick == null)
        {
            Debug.LogError("UnitPlayer.Start() " + this.name + " 'rotateJoystick' or 'moveJoystick' component isn't assigned!!!");
            Debug.Break();
            return;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void FixedUpdate()
    {
        MoveUnit();
        RotateUnit();
    }

    #endregion

    #region Player functions

    private void MoveUnit()
    {
        switch (controlType)
        {
            case ControlTypes.Keybord:
                moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed_X, 0f, Input.GetAxis("Vertical") * speed_Z);
                break;
            case ControlTypes.VirtualJoystick:
                moveDirection = new Vector3(moveJoystick.AxisX * speed_X, 0f, moveJoystick.AxisY * speed_Z);
                break;
        }

        moveDirection = Camera.mainCamera.transform.TransformDirection(moveDirection);
        controller.SimpleMove(moveDirection);
    }

    private void RotateUnit()
    {
        if (rotateJoystick.AngleDegrees != 0)
            this.transform.localRotation = Quaternion.Euler(new Vector3(0f, -rotateJoystick.AngleDegrees + 45f, 0f));
        else
        {
            Vector3 lookDirection = moveDirection + this.transform.position;
            this.transform.LookAt(new Vector3(lookDirection.x, this.transform.position.y, lookDirection.z));
        }
    }

    #endregion
}
