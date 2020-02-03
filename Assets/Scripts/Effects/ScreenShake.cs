using UnityEngine;

namespace WraithavenGames.FirstSight
{
    public class ScreenShake : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("How much the screen shakes at maximum trama.")]
        [SerializeField] private float shakePower = 45f;
        [Tooltip("How fast the screen shakes at maximum trama.")]
        [SerializeField] private float shakeSpeed = 10f;
        [Tooltip("How willingly more trama is added to the current trama level.")]
        [SerializeField] private float traumaSensitivity = 1f;
        [Tooltip("How quickly trama falls away.")]
        [SerializeField] private float traumaFalloff = 5f;
        [Tooltip("Whether the falloff is in scaled or unscaled time.")]
        [SerializeField] private bool scaledTime = false;

        private float trauma;
        private Vector3 cameraRot;

        private void Update()
        {
            // Shake screen
            if (trauma > 0f)
            {
                float time = Time.time * shakeSpeed;
                cameraRot.x = Mathf.PerlinNoise(time, 0);
                cameraRot.y = Mathf.PerlinNoise(time, 100);
                cameraRot.z = Mathf.PerlinNoise(time, 200);
                cameraRot *= trauma * trauma * shakePower;
            }
            else
                cameraRot = Vector3.zero;

            // Apply falloff
            float falloff = traumaFalloff * (scaledTime ? Time.deltaTime : Time.unscaledDeltaTime);
            trauma = Mathf.Clamp01(trauma - falloff);
        }

        public void ShakeCamera(Transform cam)
        {
            cam.Rotate(cameraRot);
        }

        public void Hit(float power)
        {
            trauma = Mathf.Clamp01(trauma + power * traumaSensitivity);
        }
    }
}
