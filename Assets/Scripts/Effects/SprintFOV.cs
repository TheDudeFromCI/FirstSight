using UnityEngine;

namespace WraithavenGames.FirstSight
{
	public class SprintFOV : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private float _restingFOV = 60f;
		[SerializeField] private float _runningFOV = 70f;
		[SerializeField] private float _fovAdjustTime = 0.15f;

		[Header("Dependencies")]
		[SerializeField] private Camera _camera;
		[SerializeField] private PlayerController _controller;

		private float _fovLean;

		private void Start()
		{
			_fovLean = 0f;
		}

		private void OnValidate()
		{
			if (_camera != null)
				_camera.fieldOfView = _restingFOV;
		}

		private void LateUpdate()
		{
			float goalFOV = _controller.IsRunning ? 1f : 0f;
			_fovLean = Mathf.MoveTowards(_fovLean, goalFOV, Time.deltaTime / _fovAdjustTime);
			_camera.fieldOfView = Mathf.Lerp(_restingFOV, _runningFOV, _fovLean);
		}
	}
}