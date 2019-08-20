using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStarColor : MonoBehaviour
{
	[ColorUsageAttribute(true, true)] public Color color1;
	[ColorUsageAttribute(true, true)] public Color color2;
	[ColorUsageAttribute(true, true)] public Color color3;
	public Vector2 MinMaxEmissive;
	public AnimationCurve intensityCurve;

	private float timer;
	private float timerSpeed;
	private Material m;
	private Color initColor;

	private void Start()
	{
		timer = Random.Range(0f, 1f);
		timerSpeed = Random.Range(0.2f, 0.4f);

		m = GetComponent<Renderer>().material;

		Color c1 = Color.Lerp(color1, color2, Random.value); //Set Temperature
		Color c2 = Color.Lerp(c1, color3, Random.value);	 //Set Saturation
		Color c3 = c2 * Random.Range(0.2f, 1f);				 //Set Intensity

		m.SetColor("_EmissiveColor", c3);
		initColor = c3;
	}

	private void Update()
	{
		timer += Time.deltaTime * timerSpeed;
		while(timer > 1)
		{
			timer -= 1;
		}
		m.SetColor("_EmissiveColor", initColor * intensityCurve.Evaluate(timer));
	}
}
