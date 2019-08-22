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
	[HideInInspector] public ConstellationAlignment lastConstellationAlignment;

	public bool isDay { get; private set; }
	public bool skyControlsActivated { get; private set; }
	private Transform playerTransform;
	private CharacterControls playerControls;
	private float cantControlSkyTimer = 0;
	private CanvasController canvas;

	private void Start()
	{
		SetDayCycle(true);
		playerTransform = PlayerSingleton.instance.transform;
		playerControls = playerTransform.GetComponent<CharacterControls>();
		canvas = CanvasController.instance;
	}

	private void Update()
	{
		float dt = Time.deltaTime;

		//Rotate Sky
		RotateSky(dt);

		//Timers
		if (cantControlSkyTimer > 0)
		{
			cantControlSkyTimer -= dt;

			if (cantControlSkyTimer <= 0)
			{
				cantControlSkyTimer = 0;
				SetSkyControls(false);
			}

			return;
		}

		//Activate sky controls
		if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Q))
		{
			SetSkyControls(!skyControlsActivated);
		}

		//Toggle Day/Night
		if (Input.GetKeyDown(KeyCode.Keypad7))
			ToggleDayCycle();

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
		//Get input
		if (cantControlSkyTimer == 0 && skyControlsActivated)
		{
			float skyHorizontalRot = Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.A) ? -rotationSpeed : (Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.D) ? rotationSpeed : 0);
			float skyVerticalRot = Input.GetKey(KeyCode.Keypad2) ? -rotationSpeed : (Input.GetKey(KeyCode.Keypad8) ? rotationSpeed : 0);

			//Rotate Sky Target
			/*
			Quaternion rotLocal = skyRotationTarget.localRotation;
			rotLocal.eulerAngles += new Vector3(skyVerticalRot * dt, 0, 0);
			skyRotationTarget.localRotation = rotLocal;
			*/

			Quaternion rotGlobal = skyRotationTarget.rotation;
			rotGlobal.eulerAngles += new Vector3(0, skyHorizontalRot * dt, 0);
			skyRotationTarget.rotation = rotGlobal;
		}

		//Smooth Rotation
		skyDayTransform.rotation = Quaternion.Lerp(skyDayTransform.rotation, skyRotationTarget.rotation, dt * 3);
		skyNightTransform.rotation = Quaternion.Lerp(skyNightTransform.rotation, skyRotationTarget.rotation, dt * 3);
	}

	//Get the difference of the targetRotation and the current rotation
	public float GetRotationSpeed()
	{
		return Mathf.Abs(skyRotationTarget.eulerAngles.y - skyNightTransform.eulerAngles.y);
	}

	//Set sky controls
	public void SetSkyControls(bool hasControls)
	{
		if (cantControlSkyTimer > 0)
			return;

		//Cam Target
		if (lastConstellationAlignment != null)
		{
			CameraControls cc = CameraSingleton.instance.GetComponentInParent<CameraControls>();
			if (lastConstellationAlignment.IsPlayerInRadius() && hasControls)
			{
				cc.AddCamTarget(lastConstellationAlignment.camTarget);
				Vector3 lookAt = lastConstellationAlignment.pointOfInterest.position;
				lookAt.y = 0;
				playerControls.RotatePlayer(lookAt, 1);
			}
			else
			{
				cc.StopAligning();
			}
		}

		//Snap Rotation
		if (hasControls)
			skyRotationTarget.rotation = skyDayTransform.rotation;

		//Player Animation
		CharacterAnimation.instance.SendAstrolabeTrigger(hasControls);

		//Other Stuff
		canvas.UpdateAstrolabeText(hasControls);
		skyControlsActivated = hasControls;
		playerControls.SetPlayerControls(hasControls);
	}

	//Set time of no controls
	public void DisableSkyControlForAWhile(float cooldown)
	{
		cantControlSkyTimer = cooldown;
	}
}
