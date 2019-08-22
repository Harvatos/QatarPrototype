using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterLife : MonoBehaviour
{
	public float sunlightDamagePerSecond;
	public float shadowRegenPerSecond;

	private Volume heatStrokeVolume;
	private TextMeshProUGUI lifeIndicator;
	private SkyController skyController;
	private SunDetector sd;
	private float HP = 100;

	private void Start()
	{
		skyController = SkyController.instance;
		sd = GetComponent<SunDetector>();
		heatStrokeVolume = CameraSingleton.instance.transform.GetChild(0).GetComponent<Volume>();
		lifeIndicator = CanvasController.instance.lifeTextRef;
	}

	private void Update()
	{
		//night
		if (!skyController.isDay)
		{
			HP = 100;
		}

		//day
		else
		{
			//Sunlight
			if (sd.isInSunLight())
				HP -= Time.deltaTime * sunlightDamagePerSecond;

			//Shadow
			else
				HP += Time.deltaTime * shadowRegenPerSecond;
		}

		//Clamp
		HP = Mathf.Clamp(HP, 0, 100);

		//Indicator
		heatStrokeVolume.weight = (100f - HP) / 100f;
		lifeIndicator.text = Mathf.Round(HP).ToString() + "HP";
	}
}
