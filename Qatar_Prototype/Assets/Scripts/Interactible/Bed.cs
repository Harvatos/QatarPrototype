using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public void Sleep()
	{
		CanvasController.instance.blackScreenAnim.SetTrigger("FadeIn");
		StartCoroutine( FadeOut() );
	}

	IEnumerator FadeOut()
	{
		yield return new WaitForSeconds(0.5f);

		SkyController.instance.ToggleDayCycle();
	}
}
