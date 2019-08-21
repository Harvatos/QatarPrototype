using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationMesh : MonoBehaviour
{
	[Header("References")]
	public GameObject objRef;

	private Material m;
	private bool isDisplayed = false;
	private float timer = 1;

	private void Start()
	{
		m = objRef.GetComponent<Renderer>().material;
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
			m.SetFloat("_AlphaClip", timer);
		}
	}

}
