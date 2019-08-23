using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDissolve : MonoBehaviour
{
	public bool dissolve;

	private Material m;
	private float timer;

	private void Start()
	{
		m = GetComponent<Renderer>().material;
		timer = 0f;
	}

	private void Update()
	{
		if(dissolve && (timer >= 0))
		{
			timer += Time.deltaTime;
			m.SetFloat("_AlphaClip", timer);
		}
	}

	public void StartDissolve()
	{
		dissolve = true;
	}
}
