﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
	public static CanvasController instance;
	private void Awake()
	{
		if (instance == null)
			instance = this;
	}

	public TextMeshProUGUI skyAngleTextRef;
	public TextMeshProUGUI interactibleTextRef;
	public Image interactibleIconRef;

	private bool interactibleIsVisible = false;

	private void Update()
	{
		//Display Sky Angle
		skyAngleTextRef.text = "Sky Angle: " + Mathf.Round(SkyController.instance.skyNightTransform.eulerAngles.y);
		skyAngleTextRef.text += "\nRot Speed: " + Mathf.Round(SkyController.instance.GetRotationSpeed());

		//Smooth display of interactible
		Color c = interactibleTextRef.color;
		c.a += Time.deltaTime * (interactibleIsVisible ? 2 : -2);

		c.a = Mathf.Clamp(c.a, 0f, 1f);

		interactibleIconRef.color = c;
		interactibleTextRef.color = c;
	}

	//Display the tooltip of an interactible
	public void DisplayInteractible(string tooltip)
	{
		interactibleIsVisible = true;

		interactibleTextRef.text = tooltip;
	}

	//Hide the tooltip of an interactible
	public void HideInteractible()
	{
		interactibleIsVisible = false;
	}
}