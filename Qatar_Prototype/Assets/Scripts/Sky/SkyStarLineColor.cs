using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStarLineColor : MonoBehaviour
{
	public Renderer lineRenderer;

	private float shineMultiplier = 1;
	private int shineInfluence = 10;
	private Material m;

	private void Start()
	{
		m = lineRenderer.material;
	}

	private void Update()
	{
		//Reduce shine multiplier
		if (shineMultiplier > 1f / shineInfluence)
		{
			shineMultiplier -= Time.deltaTime * 10;
		}

		if (shineMultiplier < 1f / shineInfluence)
		{
			shineMultiplier = 1f / shineInfluence;
		}
		
		m.SetColor("_EmissiveColor",  Color.white * shineMultiplier * shineInfluence);
	}

	public void Shine(float intensity)
	{
		shineMultiplier = intensity;
	}
}
