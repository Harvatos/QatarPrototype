using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyConstellation : MonoBehaviour
{
	public struct Line
	{

	}

	public GameObject linePrefab;
	public GameObject[] defaultStars;
	public GameObject[] additionalStars;



	private int index = -1;

	public void AddAStar()
	{
		index++;

		additionalStars[index].SetActive(true);
	}
}
