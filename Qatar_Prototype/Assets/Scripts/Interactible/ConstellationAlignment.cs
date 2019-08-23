using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstellationAlignment : MonoBehaviour
{
	[Header("Reference")]
	public SkyConstellation constellationRef;
	public Transform pointOfInterest;
	public Camera camTarget;

	[Header("Requirements")]
	public float positionRadius = 2f;
	public float skyAngleY;
	public int constellationIndex;

	[Header("Tolerances")]
	public float rotationSpeedTolerance = 7;
	public float angleTolerance = 5;

	[Header("Events")]
	public UnityEvent CompletedEvent;

	private SkyController skyController;
	private Transform playerTransform;
	private Camera cam;
	private bool isCompleted = false;
	private bool debug = false;

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

		if (debug)
			print("OK: 1");

		//Must be night time
		if (skyController.isDay)
			return;

		if (debug)
			print("OK: 2");

		//Index must be >=
		if (constellationRef.index < constellationIndex)
			return;

		if (debug)
			print("OK: 3");
		
		//Player Must Be in zone
		if (!IsPlayerInRadius())
			return;

		if (skyController.lastConstellationAlignment != this)
		{
			skyController.lastConstellationAlignment = this;
		}

		if (debug)
			print("OK: 4");
		
		//Must be sky controlling
		if (!skyController.skyControlsActivated)
			return;

		if (debug)
			print("OK: 3.5");



		//Sky Angle Must be good
		if (Mathf.Abs(skyAngleY - skyController.skyNightTransform.eulerAngles.y) > angleTolerance)
			return;

		if (debug)
			print("OK: 5");

		//Must be slow enought
		if (skyController.GetRotationSpeed() > rotationSpeedTolerance)
			return;

		if (debug)
			print("OK: 6");

		//Must look at constellation
		Vector3 viewport = cam.WorldToViewportPoint(pointOfInterest.position);
		if (viewport.x < 0 || viewport.x > 1 || viewport.y < 0 || viewport.y > 1 || viewport.z < 0)
			return;

		if (debug)
			print("OK: 7");

		//IS GOOD
		Alignment();
	}

	private void Alignment()
	{
		isCompleted = true;
		skyController.DisableSkyControlForAWhile(5f);
		skyController.skyRotationTarget.eulerAngles = new Vector3(0, skyAngleY, 0);
		constellationRef.Shine();

		CompletedEvent.Invoke();
	}

	//Draw that bubble
	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0f, 1f, 0.5f, 0.1f);
		Gizmos.DrawSphere(transform.position, positionRadius);
	}

	public bool IsPlayerInRadius()
	{
		return Vector3.Distance(transform.position, playerTransform.position) <= positionRadius;
	}
}
