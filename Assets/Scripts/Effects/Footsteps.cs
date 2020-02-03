using UnityEngine;

namespace WraithavenGames.FirstSight
{
	public class Footsteps : MonoBehaviour
	{
		[Header("Footstep Settings")]
		[SerializeField] private float _walkingStride = 2.5f;
		[SerializeField] private float _runningStride = 4.5f;
		[SerializeField] private float _strideAdjustTime = 0.1f;
		[SerializeField,Range(0f,2f)] private float _minFootstepPitch = 0.75f;
		[SerializeField,Range(0f,2f)] private float _maxFootstepPitch = 1.25f;
		[SerializeField,Range(0f,1f)] private float _steroBalance = 0.5f;
		[SerializeField,Range(0f,1f)] private float _walkingVolume = 0.25f;
		[SerializeField,Range(0f,1f)] private float _runningVolume = 0.5f;

		[Header("Landing Settings")]
		[SerializeField,Range(0f,2f)] private float _minLandingPitch = 0.75f;
		[SerializeField,Range(0f,2f)] private float _maxLandingPitch = 1.25f;
		[SerializeField,Range(0f,1f)] private float _landingVolume = 1f;

		[Header("Jumping Settings")]
		[SerializeField,Range(0f,2f)] private float _minJumpingPitch = 1.25f;
		[SerializeField,Range(0f,2f)] private float _maxJumpingPitch = 1.75f;
		[SerializeField,Range(0f,1f)] private float _jumpingVolume = 0.65f;

		[Header("Audio Clips")]
		[SerializeField] private AudioClip[] _footstepSounds;
		[SerializeField] private AudioClip[] _landingSounds;
		[SerializeField] private AudioClip[] _jumpingSounds;

		[Header("Dependencies")]
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private PlayerController _controller;

		private bool _leftFootNext;
		private Vector3 _lastPosition;
		private float _sinceLastFootstep;
		private bool _wasGrounded;
		private bool _onLeftFoot;
		private float _stride;
		private float _lastLandingTime;
		private float _lastJumpingTime;

		private void Start()
		{
			_stride = _walkingStride;
			_wasGrounded = true;
			_lastLandingTime = float.MinValue;
			_lastJumpingTime = float.MinValue;
		}

		private void Update()
		{
			float goalStride = _controller.IsRunning ? _runningStride : _walkingStride;
			_stride = Mathf.MoveTowards(_stride, goalStride, Time.deltaTime / _strideAdjustTime);

			if (_controller.IsGrounded)
			{
				if(_wasGrounded)
				{
					Vector3 pos = transform.position;
					Vector3 posDelta = pos - _lastPosition;
					posDelta.y = 0f;

					_sinceLastFootstep += posDelta.magnitude;
					_lastPosition = pos;

					if (_sinceLastFootstep >= _stride)
					{
						_sinceLastFootstep -= _stride;
						PlayFootstepSound();
					}
				}
				else
					PlayLandingSound();
				_wasGrounded = true;
			}
			else if (_controller.IsAboutToLand)
				PlayLandingSound();
			else
			{
				if (_wasGrounded && _controller.IsCurrentlyJumping)
					PlayJumpingSound();

				_sinceLastFootstep = 0f;
				_wasGrounded = false;
			}
		}

		private void PlayFootstepSound()
		{
			int randomIndex = Random.Range(0, _footstepSounds.Length);
			AudioClip sound = _footstepSounds[randomIndex]; 

			_audioSource.panStereo = _onLeftFoot ? -_steroBalance : _steroBalance;
			_onLeftFoot = !_onLeftFoot;

			float volume = _controller.IsRunning ? _runningVolume : _walkingVolume;

			_audioSource.pitch = Random.Range(_minFootstepPitch, _maxFootstepPitch);
			_audioSource.PlayOneShot(sound, volume);
		}

		private void PlayLandingSound()
		{
			if (Time.time - _lastLandingTime < 0.1f)
				return;

			_lastLandingTime = Time.time;

			int randomIndex = Random.Range(0, _landingSounds.Length);
			AudioClip sound = _landingSounds[randomIndex]; 

			_audioSource.panStereo = 0f;

			_audioSource.pitch = Random.Range(_minLandingPitch, _maxLandingPitch);
			_audioSource.PlayOneShot(sound, _landingVolume);
		}

		private void PlayJumpingSound()
		{
			if (Time.time - _lastJumpingTime < 0.1f)
				return;

			_lastJumpingTime = Time.time;

			int randomIndex = Random.Range(0, _jumpingSounds.Length);
			AudioClip sound = _jumpingSounds[randomIndex]; 

			_audioSource.panStereo = 0f;

			_audioSource.pitch = Random.Range(_minJumpingPitch, _maxJumpingPitch);
			_audioSource.PlayOneShot(sound, _jumpingVolume);
		}
	}
}
