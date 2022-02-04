using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
	public LayerMask pushLayers;
	public bool canPush;
	[Range(0.5f, 5f)] public float strength = 1.1f;

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (canPush) PushRigidBodies(hit);
	}

	private void PushRigidBodies(ControllerColliderHit hit)
	{
		// Sprawdzamy czy trafiony obiekt jest niekinetyczny
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) return;

		// Sprawdzamy czy pchamy dobrą warstwę
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0) return;

		// Bez pchania obiektów pod nami
		if (hit.moveDirection.y < -0.3f) return;

		// Obliczanie kierunku pchania z kierunku ruchu
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
		body.AddForce(pushDir * strength, ForceMode.Impulse);
	}
}