using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterLife : MonoBehaviour
{
	public TextMeshProUGUI lifeIndicator;
	public float sunlightDamagePerSecond;
	public float shadowRegenPerSecond;
	public Volume heatStrokeVolume;

	private SunDetector sd;
	private float HP = 100;

	private void Start()
	{
		sd = GetComponent<SunDetector>();
	}

	private void Update()
	{
		if (sd.isInSunLight())
			HP -= Time.deltaTime * sunlightDamagePerSecond;

		else
			HP += Time.deltaTime * shadowRegenPerSecond;

		HP = Mathf.Clamp(HP, 0, 100);

		heatStrokeVolume.weight = HP / 100f;
		lifeIndicator.text = Mathf.Round(HP).ToString() + "HP";
	}
}
