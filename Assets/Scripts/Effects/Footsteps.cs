using UnityEngine;

#pragma warning disable CS0649

namespace WraithavenGames.FirstSight
{
    public class Footsteps : MonoBehaviour
    {
        [Header("Footstep Settings")]
        [SerializeField] private float walkingStride = 2.5f;
        [SerializeField] private float runningStride = 4.5f;
        [SerializeField] private float strideAdjustTime = 0.1f;
        [SerializeField, Range(0f, 2f)] private float minFootstepPitch = 0.75f;
        [SerializeField, Range(0f, 2f)] private float maxFootstepPitch = 1.25f;
        [SerializeField, Range(0f, 1f)] private float steroBalance = 0.5f;
        [SerializeField, Range(0f, 1f)] private float walkingVolume = 0.25f;
        [SerializeField, Range(0f, 1f)] private float runningVolume = 0.5f;

        [Header("Landing Settings")]
        [SerializeField, Range(0f, 2f)] private float minLandingPitch = 0.75f;
        [SerializeField, Range(0f, 2f)] private float maxLandingPitch = 1.25f;
        [SerializeField, Range(0f, 1f)] private float landingVolume = 1f;

        [Header("Jumping Settings")]
        [SerializeField, Range(0f, 2f)] private float minJumpingPitch = 1.25f;
        [SerializeField, Range(0f, 2f)] private float maxJumpingPitch = 1.75f;
        [SerializeField, Range(0f, 1f)] private float jumpingVolume = 0.65f;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private AudioClip[] landingSounds;
        [SerializeField] private AudioClip[] jumpingSounds;

        [Header("Dependencies")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private PlayerController controller;

        private bool leftFootNext;
        private Vector3 lastPosition;
        private float sinceLastFootstep;
        private bool wasGrounded;
        private bool onLeftFoot;
        private float stride;
        private float lastLandingTime;
        private float lastJumpingTime;

        private void Start()
        {
            stride = walkingStride;
            wasGrounded = true;
            lastLandingTime = float.MinValue;
            lastJumpingTime = float.MinValue;
        }

        private void Update()
        {
            float goalStride = controller.IsRunning ? runningStride : walkingStride;
            stride = Mathf.MoveTowards(stride, goalStride, Time.deltaTime / strideAdjustTime);

            if (controller.IsGrounded)
            {
                if (wasGrounded)
                {
                    Vector3 pos = transform.position;
                    Vector3 posDelta = pos - lastPosition;
                    posDelta.y = 0f;

                    sinceLastFootstep += posDelta.magnitude;
                    lastPosition = pos;

                    if (sinceLastFootstep >= stride)
                    {
                        sinceLastFootstep -= stride;
                        PlayFootstepSound();
                    }
                }
                else
                    PlayLandingSound();
                wasGrounded = true;
            }
            else if (controller.IsAboutToLand)
            {
                sinceLastFootstep = 0f;
                lastPosition = transform.position;

                PlayLandingSound();
            }
            else
            {
                if (wasGrounded && controller.IsCurrentlyJumping)
                    PlayJumpingSound();

                sinceLastFootstep = 0f;
                wasGrounded = false;
            }
        }

        private void PlayFootstepSound()
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            AudioClip sound = footstepSounds[randomIndex];

            audioSource.panStereo = onLeftFoot ? -steroBalance : steroBalance;
            onLeftFoot = !onLeftFoot;

            float volume = controller.IsRunning ? runningVolume : walkingVolume;

            audioSource.pitch = Random.Range(minFootstepPitch, maxFootstepPitch);
            audioSource.PlayOneShot(sound, volume);
        }

        private void PlayLandingSound()
        {
            if (Time.time - lastLandingTime < 0.1f)
                return;

            lastLandingTime = Time.time;

            int randomIndex = Random.Range(0, landingSounds.Length);
            AudioClip sound = landingSounds[randomIndex];

            audioSource.panStereo = 0f;

            audioSource.pitch = Random.Range(minLandingPitch, maxLandingPitch);
            audioSource.PlayOneShot(sound, landingVolume);
        }

        private void PlayJumpingSound()
        {
            if (Time.time - lastJumpingTime < 0.1f)
                return;

            lastJumpingTime = Time.time;

            int randomIndex = Random.Range(0, jumpingSounds.Length);
            AudioClip sound = jumpingSounds[randomIndex];

            audioSource.panStereo = 0f;

            audioSource.pitch = Random.Range(minJumpingPitch, maxJumpingPitch);
            audioSource.PlayOneShot(sound, jumpingVolume);
        }
    }
}
