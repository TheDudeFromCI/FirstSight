using UnityEngine;

namespace WraithavenGames.FirstSight
{
    public class InputSettings : MonoBehaviour
    {
        [Header("Move")]
        [Tooltip("Settings for the sideways movement input.")]
        [SerializeField] private AxisInputMethod _moveX = new AxisInputMethod("Horizontal");
        [Tooltip("Settings for the forwards and backwards movement input.")]
        [SerializeField] private AxisInputMethod _moveZ = new AxisInputMethod("Vertical");

        [Header("Look")]
        [Tooltip("Settings for the horizontal camera input.")]
        [SerializeField] private AxisSensitiveInputMethod _camX = new AxisSensitiveInputMethod("Mouse X", 100f);
        [Tooltip("Settings for the vertical camera input.")]
        [SerializeField] private AxisSensitiveInputMethod _camY = new AxisSensitiveInputMethod("Mouse Y", 100f);

        [Header("Modifiers")]
        [Tooltip("Settings the the run button.")]
        [SerializeField] private KeyInputMethod _run = new KeyInputMethod("left shift");
        [Tooltip("Settings for the jump button.")]
        [SerializeField] private KeyInputMethod _jump = new KeyInputMethod("space");

        public float MoveX { get{ return _moveX.GetValue(); } }
        public float MoveZ { get{ return _moveZ.GetValue(); } }

        public float LookX { get{ return _camX.GetValue(); }}
        public float LookY { get{ return _camY.GetValue(); }}

        public bool IsRunning { get{ return _run.IsHeld(); } }
        public bool IsJumping { get{ return _jump.IsHeld(); } }
        public bool HasJumped { get{ return _jump.IsDown(); } }
    }

    [System.Serializable]
    public class KeyInputMethod
    {
        [Tooltip("Whether to use a direct key, or button via project input preferences.")]
        [SerializeField] private InputType _type;
        [Tooltip("The name of the key of button to use.")]
        [SerializeField] private string _value;

        public KeyInputMethod(string value)
        {
            _type = InputType.Key;
            _value = value;
        }

        public bool IsDown()
        {
            if (_type == InputType.Key)
                return Input.GetKeyDown(_value);
            if (_type == InputType.Button)
                return Input.GetButtonDown(_value);

            return false;
        }

        public bool IsHeld()
        {
            if (_type == InputType.Key)
                return Input.GetKey(_value);
            if (_type == InputType.Button)
                return Input.GetButton(_value);

            return false;
        }
    }

    [System.Serializable]
    public class AxisInputMethod
    {
        [Tooltip("The name of the axis to use in the project input preferences.")]
        [SerializeField] private string _axis;

        public AxisInputMethod(string axis)
        {
            _axis = axis;
        }

        public float GetValue()
        {
            return Input.GetAxis(_axis);
        }
    }

    [System.Serializable]
    public class AxisSensitiveInputMethod
    {
        [Tooltip("The name of the axis to use in the project input preferences.")]
        [SerializeField] private string _axis;
        [Tooltip("The sensitivity of this axis. Use negative values to invert the direction.")]
        [SerializeField] private float _sensitivity = 1f;

        public AxisSensitiveInputMethod(string axis, float sensitivity)
        {
            _axis = axis;
            _sensitivity = sensitivity;
        }

        public float GetValue()
        {
            return Input.GetAxis(_axis) * _sensitivity;
        }
    }

    public enum InputType
    {
        Key,
        Button
    }
}