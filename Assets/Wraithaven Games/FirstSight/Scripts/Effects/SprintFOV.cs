using UnityEngine;

#pragma warning disable CS0649, CS0108

namespace WraithavenGames.FirstSight
{
    public class SprintFOV : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float restingFOV = 60f;
        [SerializeField] private float runningFOV = 70f;
        [SerializeField] private float fovAdjustTime = 0.15f;

        [Header("Dependencies")]
        [SerializeField] private Camera camera;
        [SerializeField] private PlayerController controller;

        private float fovLean;

        private void Start()
        {
            fovLean = 0f;
        }

        private void OnValidate()
        {
            if (camera != null)
                camera.fieldOfView = restingFOV;
        }

        private void LateUpdate()
        {
            float goalFOV = controller.IsRunning ? 1f : 0f;
            fovLean = Mathf.MoveTowards(fovLean, goalFOV, Time.deltaTime / fovAdjustTime);
            camera.fieldOfView = Mathf.Lerp(restingFOV, runningFOV, fovLean);
        }
    }
}