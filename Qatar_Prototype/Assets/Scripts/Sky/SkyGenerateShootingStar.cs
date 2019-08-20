using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyGenerateShootingStar : MonoBehaviour
{
	public GameObject shootingStarPrefab;
	public float rateDelay;

	private float timer = 0;

	public void Update()
	{
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			timer = Random.Range(rateDelay / 2f, 2 * rateDelay);

			GameObject s = Instantiate(shootingStarPrefab, transform);
			s.transform.position = Vector3.zero;

			s.transform.Rotate(Random.Range(10f, 90f), 0f, 0f, Space.Self);
			s.transform.Rotate(0f, Random.Range(0f, 360f), 0f, Space.World);

			s.transform.GetChild(0).localScale *= Random.Range(0.25f, 0.5f);
		}
	}
}
