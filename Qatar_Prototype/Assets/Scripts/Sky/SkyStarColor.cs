using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStarColor : MonoBehaviour
{
	public Color color1;
	public Color color2;
	public Color color3;
	public Vector2 MinMaxEmissive;

	private Material m;
	private float initIntensity;
	private float intensity;

	private void Start()
	{
		m = GetComponent<Renderer>().material;
		Color c = Color.Lerp(color1, color2, Random.value);
		m.SetColor("_EmissiveColor", Color.Lerp(c, color3, Random.value));

		intensity = Random.Range(MinMaxEmissive.x, MinMaxEmissive.y);
		initIntensity = Random.Range(MinMaxEmissive.x, MinMaxEmissive.y);
		m.SetFloat("_EmissiveIntensity", intensity);
	}

	private void Update()
	{
		intensity = Mathf.Clamp(intensity + Random.Range(-0.1f, 0.1f), initIntensity - 0.5f, initIntensity + 0.5f);
		m.SetFloat("_EmissiveIntensity", intensity);
	}
}
