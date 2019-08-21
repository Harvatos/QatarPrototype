using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkyController : MonoBehaviour
{
	public static SkyController instance;
	private void Awake()
	{
		if (instance == null)
			instance = this;
	}

	[Header("References")]
	public Transform skyRotationTarget;
	public Transform skyDayTransform;
	public Transform skyNightTransform;
	public Transform dayLightTransform;

	[Header("Post Processing")]
	public Volume cameraVolume;
	public VolumeProfile profileDay;
	public VolumeProfile profileNight;

	[Header("Values")]
	public float rotationSpeed;

	public bool isDay { get; private set; }
	private bool skyControlsActivated = false;
	private Transform playerTransform;

	private void Start()
	{
		SetDayCycle(true);
		playerTransform = PlayerSingleton.instance.transform;
	}

	private void Update()
	{
		float dt = Time.deltaTime;

		//Activate sky controls
		if (Input.GetKeyDown(KeyCode.Keypad5))
		{
			SetSkyControls(!skyControlsActivated);
		}

		//Toggle Day/Night
		if (Input.GetKeyDown(KeyCode.Keypad7))
			ToggleDayCycle();

		//Rotate Sky
		RotateSky(dt);

		//Follow Player
		transform.position = playerTransform.position;
	}

	public void ToggleDayCycle()
	{
		SetDayCycle(!isDay);
	}

	public void SetDayCycle(bool isDay)
	{
		this.isDay = isDay;

		skyDayTransform.gameObject.SetActive(isDay);
		skyNightTransform.gameObject.SetActive(!isDay);

		cameraVolume.profile = isDay ? profileDay : profileNight;
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

	//Get the difference of the targetRotation and the current rotation
	public float GetRotationSpeed()
	{
		return Mathf.Abs(skyRotationTarget.eulerAngles.y - skyNightTransform.eulerAngles.y);
	}

	//Set sky controls
	public void SetSkyControls(bool hasControls)
	{
		skyRotationTarget.rotation = skyDayTransform.rotation;
		skyControlsActivated = hasControls;
	}
}
