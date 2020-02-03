using UnityEngine;

namespace WraithavenGames.FirstSight
{
	public class PushObjects : MonoBehaviour
	{
		[SerializeField] private float _pushPower = 10f;

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody body = hit.rigidbody;

			if (body == null || body.isKinematic)
				return;

			if (hit.moveDirection.y < -0.1f)
				return;

			body.AddForceAtPosition(-hit.normal * _pushPower, hit.point);
		}
	}
}
