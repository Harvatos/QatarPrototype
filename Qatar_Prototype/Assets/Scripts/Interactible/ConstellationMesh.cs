using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationMesh : MonoBehaviour
{
	[Header("References")]
	public GameObject objRef;

	public void ShowObject()
	{
		objRef.SetActive(true);
	}

}
