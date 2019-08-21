﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

	private Rigidbody body;
	private Camera characterCam;

	private Vector3 velocity;
	private Vector3 characterRotation;
	private Vector3 startingCamPos;
	private Vector3 startingCamRot;

	private Vector3 cameraRotation;
	private Vector3 currentCamRot = Vector3.zero;
	public Vector3 currentCamPos = Vector3.zero;

	private float tempDelta;

	public float movingSpeed = 7;
	public float rotationSpeed = 30;

	public float cameraMovingSpeed = 7;
	public float cameraRotationgSpeed = 200;
	public float cameraRepositioningSpeed = 1;


	public float camTiltSpeed = 3;
	public float camMinTilt = -20;
	public float camMaxTilt = 30;

	private Vector3 smoothRef = Vector3.zero;

	

	public float jumpForce = 1000;
	public float airTime = 10;
	public float jumpGravity = 1;
	public float idleAirTime = 3;

	public bool grounded = true;
	public bool camIdel;
	public bool collided;

	public float interactionRange = 5;

	

	void Start()
    {
		InitCharacter();
	}

	private void InitCharacter() {
		CenterMouse();
		body = GetComponent<Rigidbody>();
		characterCam = GetComponentInChildren<Camera>();
		characterCam.gameObject.transform.SetParent(null);
	}

	private void CenterMouse() {
		Cursor.lockState = CursorLockMode.Locked;
	}
    
    void Update()
    {
		//CHARACTER MOVEMENT
		float _xMovement = Input.GetAxis("Horizontal");
		float _yMovement = Input.GetAxis("Vertical");

		Vector3 _horizontalMovement = transform.right * _xMovement;
		Vector3 _verticalMovement = transform.forward * _yMovement;

		velocity = (_horizontalMovement + _verticalMovement) * movingSpeed;
		
		//CHARACTER ROTATION
		float _yRotation = Input.GetAxisRaw("Mouse X") * rotationSpeed*2;
		characterRotation = new Vector3(0, _yRotation, 0);

		float _xRotation = Input.GetAxisRaw("Mouse Y") * rotationSpeed/2;
		cameraRotation = new Vector3(_xRotation, _yRotation, 0);
		//STATS
		SetGrounded();
	
	}

	private void FixedUpdate() {
		tempDelta = Time.fixedDeltaTime;
		SetAirTime();

		PerformMovement();
		PerformRotation();
		CameraFollow();
		PerformCameraRotation();
		//CONTROLLED
		Jump();
		Interact();
	}

	private void PerformMovement() {
		if (velocity != Vector3.zero) {
			body.MovePosition(body.position + velocity * tempDelta);
		}
	}

	private void PerformRotation() {
		if (velocity.magnitude == 0) return;
		if (grounded) {
			if (characterRotation == Vector3.zero) {
				body.MoveRotation(body.rotation * Quaternion.identity);
				body.angularVelocity = Vector3.zero;
			}
			else {
				body.MoveRotation(body.rotation * Quaternion.Euler(characterRotation));
			}
		}
	}

	private void PerformCameraRotation() {
		if (!grounded) return;
		if (velocity.magnitude != 0) {
			//if (camIdel) {
			//	camFloatingPoint.position = Vector3.Lerp(camFloatingPoint.localPosition, GetPlayerBehindPos(), tempDelta * cameraRepositioningSpeed);
			//	//camFloatingPoint.localPosition = startingCamPos;
			//	//	camFloatingPoint.localRotation = Quaternion.Slerp(camFloatingPoint.localRotation, Quaternion.Euler(startingCamRot), tempDelta * cameraRepositioningSpeed * 2);
			//	Debug.Log((camFloatingPoint.localPosition - startingCamPos).sqrMagnitude);
			//	if ((camFloatingPoint.localPosition - startingCamPos).sqrMagnitude < 100) {
			//		camIdel = false;
			//	}
			//}
			camIdel = false;
		} else {
			if(!camIdel) {
				currentCamRot = transform.eulerAngles;
				currentCamPos = GetPlayerBehindPos();
				camIdel = true;
			}

			//currentCamPos.y = Mathf.Clamp(currentCamPos.y,0, startingCamPos.y);

			//currentCamPos.y -= cameraRotation.x;
			//currentCamPos.y = Mathf.Clamp(currentCamPos.y, 0.5f, 3);
			//characterCam.transform.position = Vector3.Lerp(characterCam.transform.position, currentCamPos, tempDelta * camTiltSpeed/2);
			
			currentCamRot.x -= cameraRotation.x;
			currentCamRot.x = Mathf.Clamp(currentCamRot.x, camMinTilt, camMaxTilt);

			currentCamRot.y += cameraRotation.y;
			characterCam.transform.rotation = Quaternion.Slerp(characterCam.transform.transform.rotation, Quaternion.Euler(currentCamRot), tempDelta * camTiltSpeed*2);
		}	
	}

	private void CameraFollow() {
		if (velocity.magnitude == 0 && grounded) return;

		characterCam.transform.position = Vector3.SmoothDamp(characterCam.transform.position, GetPlayerBehindPos(), ref smoothRef, tempDelta * cameraMovingSpeed);
		characterCam.transform.rotation = Quaternion.Slerp(characterCam.transform.rotation, Quaternion.Euler(GetThirdPersonRot()), tempDelta * cameraRotationgSpeed);
	}

	public Vector3 GetPlayerBehindPos() {
		Vector3 camFollowPos = transform.position - transform.forward * 4;
		camFollowPos.y += 1.84f;
		return camFollowPos;
	}

	public Vector3 GetThirdPersonRot() {
		Vector3 camAngle = transform.eulerAngles;
		camAngle.x = 10;
		return camAngle;
	}


	private void Jump() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (grounded) {
				body.AddForce(Vector3.up * airTime, ForceMode.Impulse);
			}
		}
	}

	private void SetGrounded() {
		bool _hittedGround = false;
		RaycastHit jumpHit;
		if (Physics.Raycast(transform.position, Vector3.down, out jumpHit,1.25f)) {
			if (jumpHit.collider.tag == "Ground") {
				_hittedGround = true;
			}
		}
		grounded = _hittedGround;

		if(grounded)
			collided = false;

	}

	private void SetAirTime() {
		if(collided) {
			body.AddForce(Vector3.down * 1000);
			return;
		}

		if (!grounded)
			body.AddForce(Vector3.down * jumpGravity);

		float _speed = velocity.magnitude;
		if (_speed < 1) _speed = idleAirTime;
		airTime = _speed * jumpForce;
	}

	private void Interact() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			Collider[] nearColliders = Physics.OverlapSphere(transform.position, interactionRange);
			for (int i = 0; i < nearColliders.Length; i++) {
				if (nearColliders[i].transform.tag == "Interactable") {
					Rigidbody interactable = nearColliders[i].transform.GetComponent<Rigidbody>();
					interactable.AddForce(transform.forward * 1000);
				}
			}
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (!grounded) {
			if (collision.transform.tag == "Ground") {
				collided = true;
			}
		}
	}
}