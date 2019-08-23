using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class StarPickup : MonoBehaviour
{
	public Animator[] anims;
	public SkyConstellation constellation;
	public GameObject prisonLightActive;
	public GameObject prisonLightInactive;
	public VisualEffect[] coreRings;

	public void ReleaseStar()
	{
		foreach(Animator a in anims)
		{
			a.SetTrigger("Death");
		}

		StartCoroutine( AddStar() );
		foreach(VisualEffect ringFX in coreRings)
		{
			if(ringFX) { ringFX.SetFloat("Power", 0); }
		}
	}

	IEnumerator AddStar()
	{
		yield return new WaitForSeconds(4.25f);

		constellation.AddStar();
		prisonLightActive.SetActive(false);
		prisonLightInactive.SetActive(true);
	}
}
