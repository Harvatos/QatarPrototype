using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	public LayerMask groundMask;
	public LayerMask interactable;

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

	public float camThirdPersonDistance = 5;
	public float camThirdPersonAbove = 2;
	public float camThirdPersonRight = 2;

	private Vector3 smoothRef = Vector3.zero;

	public float jumpForce = 1000;
	public float airTime = 10;
	public float jumpGravity = 1;
	public float idleAirTime = 3;

	public bool grounded = true;
	public bool camIdel;
	public bool collided;

	public float interactionRange = 5;

	private CapsuleCollider charCollider;

	public bool startMode;

	void Start()
    {
		InitCharacter();
	}

	private void InitCharacter() {
		CenterMouse();
		body = GetComponent<Rigidbody>();
		charCollider = GetComponentInChildren<CapsuleCollider>();
		characterCam = GetComponentInChildren<Camera>();
		characterCam.gameObject.transform.SetParent(null);

		startingCamRot = characterCam.transform.eulerAngles;
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

		if (Input.GetKeyDown(KeyCode.Q)) {
			startMode = !startMode;
		}
	
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
		if (camIdel) return;
		if (startMode) return;
		if (velocity != Vector3.zero) {
			body.MovePosition(body.position + velocity * tempDelta);
		}
	}

	private void PerformRotation() {
		if (startMode) return;
		//if (velocity.magnitude == 0) return;
		if (grounded) {
			if (characterRotation != Vector3.zero) {
				body.MoveRotation(body.rotation * Quaternion.Euler(characterRotation));
				//Vector3 charachterRotation = transform.eulerAngles;
			//	charachterRotation.z = 0;
			//	transform.eulerAngles = charachterRotation;
			}
		}
	}

	private void PerformCameraRotation() {
		if (velocity.magnitude != 0) {
			if(camIdel) {
				Vector3 targetRotation = currentCamRot;
				targetRotation.x = 0;
				targetRotation.z = 0;

				Vector3 direction =  characterCam.transform.forward * 100;
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, direction, tempDelta * cameraRotationgSpeed * 10, 0.0f));

				Debug.Log(Vector3.Angle(transform.forward * 100, direction));
				if (Vector3.Angle(transform.forward * 100, direction) < 10) {
					camIdel = false;
				}
			}
		} else {
			if (!startMode) return;

			if(!camIdel) {
				currentCamRot = transform.eulerAngles;
				currentCamPos = GetPlayerBehindPos();
				camIdel = true;
			}

			currentCamRot.x -= cameraRotation.x;
			currentCamRot.x = Mathf.Clamp(currentCamRot.x, camMinTilt, camMaxTilt);
			currentCamRot.y += cameraRotation.y;

			characterCam.transform.rotation = Quaternion.Slerp(characterCam.transform.transform.rotation, Quaternion.Euler(currentCamRot), tempDelta * camTiltSpeed);
			
			float xAngleDiffenreace = Mathf.Abs(Mathf.DeltaAngle(startingCamRot.x,characterCam.transform.eulerAngles.x));

			float ratio = camThirdPersonDistance*(camMaxTilt / xAngleDiffenreace);
			ratio = Mathf.Clamp(ratio, 0, camThirdPersonDistance);
	
			Vector3 targetBehindPos = transform.position - transform.forward * ratio;
			float zOffSet = targetBehindPos.z;

			Vector3 targetRightPos = transform.position - transform.right * ratio;
			float xOffSet = targetRightPos.x;

			Vector3 camTargetPos = transform.position;
			camTargetPos.z = zOffSet;
			camTargetPos.x = xOffSet;

			//	float leftRation = camThirdPersonRight*(camMaxTilt / xAngleDiffenreace);
			//	Debug.Log(leftRation);
			////	leftRation = Mathf.Clamp(ratio, 0, 2);
			//	Vector3 targetLeftPos = transform.right * ratio;
			//	float xOffSet = targetLeftPos.z;
			//	camTargetPos.x = xOffSet;

			characterCam.transform.position = Vector3.Lerp(characterCam.transform.position, targetBehindPos, tempDelta * camTiltSpeed);
		}
	}

	private void CameraFollow() {
		if (startMode) return;
		characterCam.transform.position = Vector3.SmoothDamp(characterCam.transform.position, GetPlayerBehindPos(), ref smoothRef, tempDelta * cameraMovingSpeed);
		characterCam.transform.rotation = Quaternion.Slerp(characterCam.transform.rotation, Quaternion.Euler(GetThirdPersonRot()), tempDelta * cameraRotationgSpeed);
	}

	public Vector3 GetPlayerBehindPos() {
		Vector3 camFollowPos = transform.position - transform.forward * camThirdPersonDistance;
		camFollowPos.y += camThirdPersonAbove;
		return camFollowPos;
	}

	public Vector3 GetThirdPersonRot() {
		Vector3 camAngle = transform.eulerAngles;
		camAngle.x = 10;
		camAngle.z = 0;
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
		if (Physics.Raycast(transform.position, Vector3.down, out jumpHit,1.25f, groundMask)) {
			_hittedGround = true;
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
		if (Input.GetKeyDown(KeyCode.E)) {
			Collider[] nearColliders = Physics.OverlapSphere(transform.position, interactionRange, interactable);
			for (int i = 0; i < nearColliders.Length; i++) {
				Rigidbody interactable = nearColliders[i].transform.GetComponent<Rigidbody>();
				interactable.AddForce(transform.forward * 1000);
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
