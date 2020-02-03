using UnityEngine;

namespace WraithavenGames.FirstSight
{
	public class MouseController : MonoBehaviour
	{
		[Header("Dependencies")]
		[SerializeField] private Transform _cam;
		[SerializeField] private InputSettings _inputSettings;

		[Tooltip("Optional Field")]
		[SerializeField] private ScreenShake _shake;

		private float _yaw;
		private float _pitch;

		private float LookX { get{ return _inputSettings == null ? 0f : _inputSettings.LookX; } }
		private float LookY { get{ return _inputSettings == null ? 0f : _inputSettings.LookY; } }

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
			if (_cam == null)
				return;

			_yaw += LookX * Time.unscaledDeltaTime;
			_pitch -= LookY * Time.unscaledDeltaTime;

			_yaw %= 360f;
			_pitch = Mathf.Clamp(_pitch, -89f, 89f);

			transform.localRotation = Quaternion.Euler(0f, _yaw, 0f);
			_cam.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

			if (_shake != null)
				_shake.ShakeCamera(_cam);
		}
	}
}
