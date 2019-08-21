using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAnimator : MonoBehaviour
{
	private Rigidbody body;
	private Animator anim;
	private Controller controller;

	private float movingSpeed;
	private Vector3 previousPosition;

	private void Start() {
		body = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		controller = GetComponent<Controller>();
	}

	private void Update() {
		anim.SetBool("grounded", controller.grounded);
		anim.SetBool("usingAstrolab", controller.startMode);
	}

	private void FixedUpdate() {
		movingSpeed = (transform.position - previousPosition).sqrMagnitude / Time.fixedDeltaTime;
		previousPosition = transform.position;
		anim.SetFloat("velocity", movingSpeed);
	}
}
