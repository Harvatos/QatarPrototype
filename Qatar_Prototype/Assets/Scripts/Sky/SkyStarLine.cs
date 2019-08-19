using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStarLine : MonoBehaviour
{
	public Transform starTarget;
	public GameObject linePrefab;

	private void Start()
	{
		TraceLine();
	}

	public void TraceLine()
	{
		if (starTarget != null)
		{
			//Create Line
			GameObject line = Instantiate(linePrefab, transform.parent);

			//Set Position
			line.transform.localPosition = transform.localPosition;

			//Set Rotation
			line.transform.LookAt(starTarget);

			//Set Length
			line.transform.localScale = new Vector3(1, 1, Vector3.Distance(transform.position, starTarget.position) / 10);
		}
	}
}
