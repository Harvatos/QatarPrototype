using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	public static CharacterAnimation instance;
	private void Awake()
	{
		if (instance == null)
			instance = this;
	}

	public Animator anim;
	public CharacterControls charControls;
	private SkyController skyController;

	private void Update()
	{
		int index = charControls.isWalking ? (charControls.isRunning ? 2 : 1) : 0;
		anim.SetInteger("State", index);
	}

	public void SendAstrolabeTrigger(bool usingAstrolabe)
	{
		if(usingAstrolabe)
			anim.SetTrigger("StartAstrolabe");

		else
			anim.SetTrigger("StopAstrolabe");
	}

}
