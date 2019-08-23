using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationMesh : MonoBehaviour
{
	[Header("References")]
	public GameObject objRef;

	private Material[] m;
	private bool isDisplayed = false;
	private float timer = 1;

	private void Start()
	{
		Renderer[] rends = objRef.GetComponentsInChildren<Renderer>();
		m = new Material[rends.Length];
		for (int i = 0; i < rends.Length; i++)
		{
			m[i] = rends[i].material;
		}
	}

	public void ShowObject()
	{
		objRef.SetActive(true);
		isDisplayed = true;
	}

	private void Update()
	{
		if(isDisplayed && timer > 0)
		{
			timer -= Time.deltaTime * 0.25f;
			foreach(Material sm in m)
			{
				if(sm) { sm.SetFloat("_AlphaClip", timer); }
			}
		}
	}

}
