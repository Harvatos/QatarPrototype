using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{
	[Header("Forces")]
	public float walkSpeed;
	public float jumpForce;
	public float fallSpeed;

	[Header("Ground")]
	[Range(0f, 1f)] public float groundFriction;
	[Range(0f, 1f)] public float airFriction;
	public Transform groundCheckPoint;
	public LayerMask groundLayer;

	[Header("Rotation")]
	public float rotationSpeed;

	private CameraControls camControls;
	private Rigidbody rb;
	private bool isGrounded = false;
	private bool hasJumpedOnce = false;
	private float additionalGravity = 0;
	private float longJumpMultiplier = 1;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		camControls = CameraSingleton.instance.GetComponentInParent<CameraControls>();
	}

	private void Update()
	{
		float dt = Time.deltaTime;

		//Get Inputs
		bool w = Input.GetKey(KeyCode.W);
		bool a = Input.GetKey(KeyCode.A);
		bool s = Input.GetKey(KeyCode.S);
		bool d = Input.GetKey(KeyCode.D);
		bool space = Input.GetKey(KeyCode.Space);
		bool spaceUp = Input.GetKeyUp(KeyCode.Space);

		//Force - Walk
		Vector3 force = new Vector3(0, 0, 0);
		force.x = a ? -1 : (d ? 1 : 0);
		force.z = w ? 1 : (s ? -1 : 0);

		rb.AddForce((transform.forward * force.z + transform.right * force.x) * walkSpeed * dt, ForceMode.VelocityChange);

		//Force - Jump
		if (space)
		{
			rb.AddForce(Vector3.up * jumpForce * longJumpMultiplier * dt, ForceMode.VelocityChange);
			hasJumpedOnce = true;
		}

		if(hasJumpedOnce && longJumpMultiplier > 0)
		{ 
			longJumpMultiplier -= dt * 7.5f;
			if (longJumpMultiplier < 0)
				longJumpMultiplier = 0;

			if (spaceUp)
				longJumpMultiplier = 0;
		}
		
		//Ground
		UpdateGround(dt);

		//Rotate toward camera angle
		if (a || s || d || w)
		{
			RotatePlayer(dt);
		}
	}

	private void UpdateGround(float dt)
	{
		Collider[] colliders = Physics.OverlapSphere(groundCheckPoint.position, groundCheckPoint.localScale.x / 2f, groundLayer);
		isGrounded = colliders.Length > 0;

		//Jump reset
		if(hasJumpedOnce && isGrounded)
		{
			longJumpMultiplier = 1;
			hasJumpedOnce = false;
		}

		//Friction
		float friction = isGrounded ? groundFriction : airFriction;
		Vector3 velocity = rb.velocity;
		velocity.x *= 1f - friction;
		velocity.z *= 1f - friction;
		rb.velocity = velocity;

		//Quick Fall
		if (!isGrounded)
		{
			additionalGravity -= dt * fallSpeed;
			rb.velocity += new Vector3(0, additionalGravity, 0);
		}
		else
		{
			additionalGravity = 0;
		}

		//Cancel Very Small velocities
		/*
		print(rb.velocity.y);
		float minSpeed = -0.5f;
		if(isGrounded && rb.velocity.y < 0 && rb.velocity.y > minSpeed)
		{
			rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, rb.velocity.z);
		}
		*/
	}

	public void RotatePlayer(float dt)
	{
		transform.rotation = camControls.pivotYAxis.rotation; //Quaternion.Lerp(transform.rotation, camControls.pivotYAxis.rotation, dt * rotationSpeed);
	}
}
