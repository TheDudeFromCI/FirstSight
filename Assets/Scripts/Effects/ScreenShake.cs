using UnityEngine;

namespace WraithavenGames.FirstSight
{
	public class ScreenShake : MonoBehaviour
	{
		[Header("Settings")]
		[Tooltip("How much the screen shakes at maximum trama.")]
		[SerializeField] private float _shakePower = 45f;
		[Tooltip("How fast the screen shakes at maximum trama.")]
		[SerializeField] private float _shakeSpeed = 10f;
		[Tooltip("How willingly more trama is added to the current trama level.")]
		[SerializeField] private float _traumaSensitivity = 1f;
		[Tooltip("How quickly trama falls away.")]
		[SerializeField] private float _traumaFalloff = 5f;
		[Tooltip("Whether the falloff is in scaled or unscaled time.")]
		[SerializeField] private bool _scaledTime = false;

		private float _trauma;
		private Vector3 _cameraRot;

		private void Update()
		{
			// Shake screen
			if (_trauma > 0f)
			{
				float time = Time.time * _shakeSpeed;
				_cameraRot.x = Mathf.PerlinNoise(time, 0);
				_cameraRot.y = Mathf.PerlinNoise(time, 100);
				_cameraRot.z = Mathf.PerlinNoise(time, 200);
				_cameraRot *= _trauma * _trauma * _shakePower;
			}
			else
				_cameraRot = Vector3.zero;

			// Apply falloff
			float falloff = _traumaFalloff * (_scaledTime ? Time.deltaTime : Time.unscaledDeltaTime);
			_trauma = Mathf.Clamp01(_trauma - falloff);
		}

		public void ShakeCamera(Transform cam)
		{
			cam.Rotate(_cameraRot);
		}

		public void Hit(float power)
		{
			_trauma = Mathf.Clamp01(_trauma + power * _traumaSensitivity);
		}
	}
}
