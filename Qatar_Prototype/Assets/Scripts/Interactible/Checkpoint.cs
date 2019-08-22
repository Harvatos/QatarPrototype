using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public float areaRadius = 4;

	private Transform playerTransform;
	private CharacterLife charlife;

	private void Start()
	{
		playerTransform = PlayerSingleton.instance.transform;
		charlife = playerTransform.GetComponent<CharacterLife>();
	}

	//When ya click on 'Use', do action if in radius
	private void Update()
	{
		//Player Must be in Radius
		if (Vector3.Distance(transform.position, playerTransform.position) <= areaRadius)
		{
			charlife.lastCheckpoint = transform;
		}
	}

	//Draw them nice lookin' bubbles
	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0.5f, 0f, 0.1f);
		Gizmos.DrawSphere(transform.position, areaRadius);


		Gizmos.color = new Color(1, 0.5f, 0f, 0.33f);
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}
