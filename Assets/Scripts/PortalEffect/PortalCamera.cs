using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
	public Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;

	// Update is called once per frame
	void LateUpdate()
	{
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
		float angularDifference = Quaternion.Angle(portal.rotation, otherPortal.rotation);

		Vector3 rotatedOffset = Quaternion.AngleAxis(angularDifference, Vector3.up) * playerOffsetFromPortal;
		transform.position = portal.position + (rotatedOffset);

		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifference, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
		transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
	}
}
