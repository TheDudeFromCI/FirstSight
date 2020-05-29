using UnityEngine;

#pragma warning disable CS0649

namespace WraithavenGames.FirstSight
{
    public class MouseController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Transform cam;
        [SerializeField] private InputSettings inputSettings;

        [Tooltip("Optional Field")]
        [SerializeField] private ScreenShake shake;

        private float yaw;
        private float pitch;

        private float LookX { get { return inputSettings == null ? 0f : inputSettings.LookX; } }
        private float LookY { get { return inputSettings == null ? 0f : inputSettings.LookY; } }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            if (cam == null)
                return;

            yaw += LookX * Time.unscaledDeltaTime;
            pitch -= LookY * Time.unscaledDeltaTime;

            yaw %= 360f;
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
            cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            if (shake != null)
                shake.ShakeCamera(cam);
        }
    }
}
