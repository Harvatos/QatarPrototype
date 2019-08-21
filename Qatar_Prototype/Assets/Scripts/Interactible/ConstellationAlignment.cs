using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstellationAlignment : MonoBehaviour
{
	[Header("Reference")]
	public SkyConstellation constellationRef;
	public Transform pointOfInterest;

	[Header("Requirements")]
	public float positionRadius = 2f;
	public float skyAngleY;
	public int constellationIndex;

	[Header("Tolerance")]
	public float rotationSpeedTolerance = 7;
	public float angleTolerance = 5;

	[Header("Events")]
	public UnityEvent CompletedEvent;

	private SkyController skyController;
	private Transform playerTransform;
	private Camera cam;
	private bool isCompleted = false;

	private void Start()
	{
		skyController = SkyController.instance;
		playerTransform = PlayerSingleton.instance.transform;
		cam = CameraSingleton.instance.GetComponent<Camera>();
	}

	private void Update()
	{
		//Must be incomplete
		if (isCompleted)
			return;

		//Index must be >=
		if (constellationRef.index < constellationIndex)
			return;

		//Player Must Be in zone
		if (Vector3.Distance(transform.position, playerTransform.position) > positionRadius)
			return;

		//Sky Angle Must be good
		if (Mathf.Abs(skyAngleY - skyController.skyNightTransform.eulerAngles.y) > angleTolerance)
			return;

		//Must be slow enought
		if (skyController.GetRotationSpeed() > rotationSpeedTolerance)
			return;

		//Must look at constellation
		Vector3 viewport = cam.WorldToViewportPoint(transform.position);
		float tolerance = 0.2f;
		if (viewport.x > (1 - tolerance) || viewport.x < tolerance || viewport.y > (1 - tolerance) || viewport.y < tolerance)
			return;

		//IS GOOD
		Alignment();
	}

	private void Alignment()
	{
		isCompleted = true;
		skyController.SetSkyControls(false);
		skyController.skyRotationTarget.eulerAngles = new Vector3(0, skyAngleY, 0);
		CompletedEvent.Invoke();
	}

	//Draw that bubble
	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0f, 1f, 0.5f, 0.1f);
		Gizmos.DrawSphere(transform.position, positionRadius);
	}
}
