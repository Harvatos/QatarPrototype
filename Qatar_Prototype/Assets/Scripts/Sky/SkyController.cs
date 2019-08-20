using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
	[Header("References")]
	public Transform skyRotationTarget;
	public Transform skyDayTransform;
	public Transform skyNightTransform;

	[Header("Values")]
	public float rotationSpeed;

	public bool isDay { get; private set; }
	private bool skyControlsActivated = false;

	private void Start()
	{
		ToggleDayCycle(true);
	}

	private void Update()
	{
		float dt = Time.deltaTime;

		//Activate sky controls
		if (Input.GetKeyDown(KeyCode.Keypad5))
		{
			skyRotationTarget.rotation = skyDayTransform.rotation;
			skyControlsActivated = !skyControlsActivated;
		}

		//Toggle Day/Night
		if (Input.GetKeyDown(KeyCode.Keypad7))
			ToggleDayCycle(!isDay);

		//Rotate Sky
		RotateSky(dt);
	}

	private void ToggleDayCycle(bool isDay)
	{
		this.isDay = isDay;

		skyDayTransform.gameObject.SetActive(isDay);
		skyNightTransform.gameObject.SetActive(!isDay);
	}

	private void RotateSky(float dt)
	{
		//Return if not in control
		if (!skyControlsActivated)
			return;

		//Get input
		float skyHorizontalRot = Input.GetKey(KeyCode.Keypad4) ? -rotationSpeed : (Input.GetKey(KeyCode.Keypad6) ? rotationSpeed : 0);
		float skyVerticalRot= Input.GetKey(KeyCode.Keypad2) ? -rotationSpeed : (Input.GetKey(KeyCode.Keypad8) ? rotationSpeed : 0);

		//Rotate Sky Target

		/*
		Quaternion rotLocal = skyRotationTarget.localRotation;
		rotLocal.eulerAngles += new Vector3(skyVerticalRot * dt, 0, 0);
		skyRotationTarget.localRotation = rotLocal;
		*/

		Quaternion rotGlobal = skyRotationTarget.rotation;
		rotGlobal.eulerAngles += new Vector3(0, skyHorizontalRot * dt, 0);
		skyRotationTarget.rotation = rotGlobal;

		//Smooth Rotation
		skyDayTransform.rotation = Quaternion.Lerp(skyDayTransform.rotation, skyRotationTarget.rotation, dt);
		skyNightTransform.rotation = Quaternion.Lerp(skyNightTransform.rotation, skyRotationTarget.rotation, dt);
	}
}
