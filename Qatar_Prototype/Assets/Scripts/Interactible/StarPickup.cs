using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPickup : MonoBehaviour
{
	public Animator[] anims;
	public SkyConstellation constellation;

	public void ReleaseStar()
	{
		foreach(Animator a in anims)
		{
			a.SetTrigger("Death");
		}

		StartCoroutine( AddStar() );
	}

	IEnumerator AddStar()
	{
		yield return new WaitForSeconds(4.25f);

		constellation.AddStar();
	}
}
