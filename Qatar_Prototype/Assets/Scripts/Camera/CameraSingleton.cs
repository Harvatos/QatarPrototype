using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingleton : MonoBehaviour
{
	public static CameraSingleton instance;
	private void Awake()
	{
		if (instance == null)
			instance = this;
	}
}
