using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyGenerateStars : MonoBehaviour
{
	public GameObject starPrefab;
	public int nbStars;

	private void Start()
	{
		GenerateStars(nbStars);
	}

	public void GenerateStars(int n)
	{
		for(int i=0; i<n; i++)
		{
			GameObject s =  Instantiate(starPrefab, transform);
			s.transform.position = Vector3.zero;

			s.transform.Rotate(Random.Range(10f, 90f), 0f, 0f, Space.Self);
			s.transform.Rotate(0f, Random.Range(0f, 360f), 0f, Space.World);

			s.transform.GetChild(0).localScale *= 0.5f;
		}
	}

}
