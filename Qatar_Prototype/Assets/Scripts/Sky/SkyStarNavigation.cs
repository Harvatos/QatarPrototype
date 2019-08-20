using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStarNavigation : MonoBehaviour
{
	private float lyDistance;
	private float altitude;

	private void Start()
	{
		altitude = transform.eulerAngles.x;
		lyDistance = Random.Range(0.05f, 1f);
	}

	private void Update()
	{
		float lyDistanceSpeedMultiplier = 10f;
		float altitudeSpeedMultiplier = 0.1f;
		altitude += Time.deltaTime * (altitude * altitudeSpeedMultiplier) * ((1f - lyDistance) * lyDistanceSpeedMultiplier);

		if(altitude > 90)
		{
			altitude = 1f;
		}

		Vector3 rot = transform.eulerAngles;
		rot.x = altitude;
		transform.eulerAngles = rot;

		float lyDistanceSizeMultiplier = 2f;
		float altitudeSizeMultiplier = 5f;
		transform.GetChild(0).localScale = Vector3.one * (1200 * (altitudeSizeMultiplier * altitude / 90f) * (lyDistanceSizeMultiplier * (1f - lyDistance)));
	}
}
