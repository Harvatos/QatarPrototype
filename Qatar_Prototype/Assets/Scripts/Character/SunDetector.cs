using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDetector : MonoBehaviour
{
	public Transform[] detectorPointsReferences;
	public float delayBetweenRaycast;

	private SkyController skyControllerRef;
	private Transform sunLightReference;
	private float timer = 0;
	private bool isInSun = false;

	private void Start()
	{
		skyControllerRef = SkyController.instance;
		sunLightReference = skyControllerRef.dayLightTransform;
	}

	public bool isInSunLight()
	{
		return isInSun;
	}

	private void Update()
	{
		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			timer = delayBetweenRaycast;
			isInSun = false;

			if (skyControllerRef.isDay)
			{
				foreach (Transform t in detectorPointsReferences)
				{
					if (!Physics.Raycast(t.position, -sunLightReference.forward, 2000f))
					{
						isInSun = true;
						break;
					}
				}
			}
		}
	}

}
