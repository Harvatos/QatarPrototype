using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class SandfallController : MonoBehaviour
{
	public VisualEffect[] mainFullFall;
	public VisualEffect[] secondaryFullFall;
	public float fullSecondaryDelay = 1f;
	public VisualEffect[] mainBlockedFall;
	public VisualEffect[] secondaryBlockedFall;
	public float blockedSecondaryDelay = 1f;

	public Collider col;

	public void BlockFall()
	{
		foreach(VisualEffect vfx in mainFullFall)
		{ if(vfx) { vfx.SetBool("Emit", false); } }

		Invoke("StopFullSecondary", fullSecondaryDelay);

		foreach (VisualEffect vfx in mainBlockedFall)
		{ if (vfx) { vfx.SetBool("Emit", true); } }

		Invoke("StartBlockedSecondary", blockedSecondaryDelay);

		col.enabled = false;
	}

	public void UnblockFall()
	{
		foreach (VisualEffect vfx in mainFullFall)
		{ if (vfx) { vfx.SetBool("Emit", true); } }

		Invoke("StartFullSecondary", fullSecondaryDelay);

		foreach (VisualEffect vfx in mainBlockedFall)
		{ if (vfx) { vfx.SetBool("Emit", false); } }

		Invoke("StopBlockedSecondary", blockedSecondaryDelay);

		col.enabled = true;
	}

	private void StartFullSecondary()
	{
		foreach (VisualEffect vfx in secondaryFullFall)
		{ if (vfx) { vfx.SetBool("Emit", true); } }
	}
	private void StopFullSecondary()
	{
		foreach (VisualEffect vfx in secondaryFullFall)
		{ if (vfx) { vfx.SetBool("Emit", false); } }
	}
	private void StartBlockedSecondary()
	{
		foreach (VisualEffect vfx in secondaryBlockedFall)
		{ if (vfx) { vfx.SetBool("Emit", true); } }
	}
	private void StopBlockedSecondary()
	{
		foreach (VisualEffect vfx in secondaryBlockedFall)
		{ if (vfx) { vfx.SetBool("Emit", false); } }
	}
}
