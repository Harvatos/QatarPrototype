using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
	[Header("References")]
	public Transform pivotYAxis;
	public Transform pivotXAxis;
	public Transform camTransform;

	[Header("Offset")]
	public float camOffsetZ;
	public float camOffsetX;
	public LayerMask camStuckOnLayers;

	[Header("Smoothers")]
	public float mouseSensitivity;
	public float positionSmooth;
	public float rotationSmooth;

	[Header("Limiters")]
	public float rotYMin;
	public float rotYMax;

	private Transform targetTransform;
	private Quaternion rotTarget;
	private float rotX = 0;
	private float rotY = 0;

	private void Start()
	{
		targetTransform = PlayerSingleton.instance.transform;
	}

	private void Update()
	{
		float dt = Time.deltaTime;

		//Input Rotation
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");
		rotX += mouseX * mouseSensitivity * dt;
		rotY += mouseY * mouseSensitivity * dt;
		
		rotY = Mathf.Clamp(rotY, rotYMin, rotYMax);

		//Smooth Transition
		transform.position = Vector3.Lerp(transform.position, targetTransform.position, positionSmooth * dt);

		//Smooth Rotation
		//pivotYAxis.localRotation = Quaternion.Lerp(pivotYAxis.localRotation, Quaternion.Euler(0, rotY, 0), dt * rotationSmooth);
		//pivotXAxis.localRotation = Quaternion.Lerp(pivotYAxis.localRotation, Quaternion.Euler(rotX, 0, 0), dt * rotationSmooth);
		pivotYAxis.localEulerAngles = new Vector3(0,rotX,0);
		pivotXAxis.localEulerAngles = new Vector3(rotY,0,0);

		//cam OffSet
		float altitudeValue = ((rotY - rotYMin) / (rotYMax - rotYMin));
		float offSetX = Mathf.Lerp(camTransform.localPosition.x, Mathf.Max(0, camOffsetX * (2 * (1f - altitudeValue) - 1.25f)), dt * 2);
		float offSetZ = Mathf.Lerp(camTransform.localPosition.z, -2.5f + (altitudeValue * camOffsetZ), dt * 2);

		RaycastHit hit = new RaycastHit();
		Vector3 direction = (camTransform.position - transform.position).normalized;
		if (Physics.Raycast(transform.position, direction, out hit, 10, camStuckOnLayers))
		{
			offSetZ = Mathf.Max(offSetZ, -hit.distance + 1f);
		}

		Vector3 targetOffset = new Vector3(offSetX, 0, offSetZ);
		camTransform.localPosition = targetOffset;// Vector3.Lerp(camTransform.localPosition, targetOffset, dt * 2);
	}
}
