using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleArea : MonoBehaviour
{
	public string interactibleTooltip = "Use";
	public float areaRadius = 4;
	[Range(0f, 1f)] public float screenPositionTolerance = 0.5f;
	public bool onlyUseOnce = true;
	public UnityEvent EventCalled;

	private Transform playerTransform;
	private Camera cam;
	private bool tooltipIsDisplayed;
	private bool usedOnce = false;

	private void Start()
	{
		playerTransform = PlayerSingleton.instance.transform;
		cam = CameraSingleton.instance.GetComponent<Camera>();
	}

	//When ya click on 'Use', do action if in radius
	private void Update()
	{
		if(onlyUseOnce && usedOnce)
		{
			if (tooltipIsDisplayed)
			{
				tooltipIsDisplayed = false;
				CanvasController.instance.HideInteractible();
			}
			return;
		}

		bool hideTooltip = false;

		//Player Must be in Radius
		if (Vector3.Distance(transform.position, playerTransform.position) <= areaRadius)
		{
			//Object Must be visible & centered
			if (IsInScreenView())
			{
				//Display Tooltip
				if (!tooltipIsDisplayed)
				{
					tooltipIsDisplayed = true;
					CanvasController.instance.DisplayInteractible(interactibleTooltip);
				}

				//Detect input
				if (Input.GetKeyDown(KeyCode.E))
				{
					//Do Action
					usedOnce = true;
					EventCalled.Invoke();
				}
			}

			//hide Tooltip
			else
				hideTooltip = true;
		}

		//hide Tooltip
		else
			hideTooltip = true;
		
		if (hideTooltip && tooltipIsDisplayed)
		{
			tooltipIsDisplayed = false;
			CanvasController.instance.HideInteractible();
		}
	}

	//Return true if transform is in screen view
	private bool IsInScreenView()
	{
		Vector3 viewport = cam.WorldToViewportPoint(transform.position);

		float tolerance = 0.5f - (0.5f * screenPositionTolerance);

		return (viewport.z > 0.5f && viewport.x < (1 - tolerance) && viewport.x > (tolerance) && viewport.y < (1 - tolerance) && viewport.y > (tolerance));
	}
	
	//Draw them nice lookin' bubbles
	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 0.5f, 1f, 0.1f);
		Gizmos.DrawSphere(transform.position, areaRadius);
	}
}
