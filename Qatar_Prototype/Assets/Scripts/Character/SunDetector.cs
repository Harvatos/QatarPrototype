using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDetector : MonoBehaviour
{
	public Transform sunLightReference;
	public SkyController skyControllerRef;
	public Transform[] detectorPointsReferences;
	public float delayBetweenRaycast;

	public Transform line;

	private float timer = 0;
	private bool isInSun = false;

	public bool isInSunLight()
	{
		return isInSun;
	}

	private void Update()
	{
		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			print(isInSun);
			timer = delayBetweenRaycast;
			isInSun = false;

			line.forward = -sunLightReference.forward;

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
