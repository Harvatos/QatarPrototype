using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
	public static PlayerSingleton instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
	}
}
