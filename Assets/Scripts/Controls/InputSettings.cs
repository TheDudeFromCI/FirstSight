using UnityEngine;

namespace WraithavenGames.FirstSight
{
    public class InputSettings : MonoBehaviour
    {
        [Header("Move")]
        [Tooltip("Settings for the sideways movement input.")]
        [SerializeField] private AxisInputMethod moveX = new AxisInputMethod("Horizontal");
        [Tooltip("Settings for the forwards and backwards movement input.")]
        [SerializeField] private AxisInputMethod moveZ = new AxisInputMethod("Vertical");

        [Header("Look")]
        [Tooltip("Settings for the horizontal camera input.")]
        [SerializeField] private AxisSensitiveInputMethod camX = new AxisSensitiveInputMethod("Mouse X", 100f);
        [Tooltip("Settings for the vertical camera input.")]
        [SerializeField] private AxisSensitiveInputMethod camY = new AxisSensitiveInputMethod("Mouse Y", 100f);

        [Header("Modifiers")]
        [Tooltip("Settings the the run button.")]
        [SerializeField] private KeyInputMethod run = new KeyInputMethod("left shift");
        [Tooltip("Settings for the jump button.")]
        [SerializeField] private KeyInputMethod jump = new KeyInputMethod("space");

        public float MoveX { get { return moveX.GetValue(); } }
        public float MoveZ { get { return moveZ.GetValue(); } }

        public float LookX { get { return camX.GetValue(); } }
        public float LookY { get { return camY.GetValue(); } }

        public bool IsRunning { get { return run.IsHeld(); } }
        public bool IsJumping { get { return jump.IsHeld(); } }
        public bool HasJumped { get { return jump.IsDown(); } }
    }

    [System.Serializable]
    public class KeyInputMethod
    {
        [Tooltip("Whether to use a direct key, or button via project input preferences.")]
        [SerializeField] private InputType type;
        [Tooltip("The name of the key of button to use.")]
        [SerializeField] private string value;

        public KeyInputMethod(string value)
        {
            type = InputType.Key;
            this.value = value;
        }

        public bool IsDown()
        {
            if (type == InputType.Key)
                return Input.GetKeyDown(value);
            if (type == InputType.Button)
                return Input.GetButtonDown(value);

            return false;
        }

        public bool IsHeld()
        {
            if (type == InputType.Key)
                return Input.GetKey(value);
            if (type == InputType.Button)
                return Input.GetButton(value);

            return false;
        }
    }

    [System.Serializable]
    public class AxisInputMethod
    {
        [Tooltip("The name of the axis to use in the project input preferences.")]
        [SerializeField] private string axis;

        public AxisInputMethod(string axis)
        {
            this.axis = axis;
        }

        public float GetValue()
        {
            return Input.GetAxis(axis);
        }
    }

    [System.Serializable]
    public class AxisSensitiveInputMethod
    {
        [Tooltip("The name of the axis to use in the project input preferences.")]
        [SerializeField] private string axis;
        [Tooltip("The sensitivity of this axis. Use negative values to invert the direction.")]
        [SerializeField] private float sensitivity = 1f;

        public AxisSensitiveInputMethod(string axis, float sensitivity)
        {
            this.axis = axis;
            this.sensitivity = sensitivity;
        }

        public float GetValue()
        {
            return Input.GetAxis(axis) * sensitivity;
        }
    }

    public enum InputType
    {
        Key,
        Button
    }
}