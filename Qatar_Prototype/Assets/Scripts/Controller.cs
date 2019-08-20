using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

	private Rigidbody body;
	private Camera characterCam;

	private Vector3 velocity;
	private Vector3 characterRotation;

	private float tempDelta;

	public float movingSpeed = 7;
	public float rotationSpeed = 30;

	public float cameraMovingSpeed = 7;
	public float cameraRotationgSpeed = 200;

	private Transform camFloatingPoint;
	private Vector3 smoothRef = Vector3.zero;

	public int groundedState = 0;
	public float jumpForce = 1000;
	public int airTime = 10;

	void Start()
    {
		InitCharacter();
	}

	private void InitCharacter() {
		CenterMouse();

		body = GetComponent<Rigidbody>();
		characterCam = GetComponentInChildren<Camera>();

		camFloatingPoint = characterCam.transform.parent;
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
		float _yRotation = Input.GetAxisRaw("Mouse X") * rotationSpeed;
		characterRotation = new Vector3(0, _yRotation, 0);

		
		Jump();
		

		//TESTING
		if (Input.GetKeyDown(KeyCode.P)) {
			Debug.Log("distance is : " + (characterCam.transform.position - camFloatingPoint.position).sqrMagnitude);
		}
	}


	private void FixedUpdate() {
		tempDelta = Time.fixedDeltaTime;
		PerformMovement();
		PerformRotation();
		CameraFollow();
	}

	private void PerformMovement() {
		if (velocity != Vector3.zero)
			body.MovePosition(body.position + velocity * tempDelta);
	}

	private void PerformRotation() {
		if(groundedState == 0)
			body.MoveRotation(body.rotation * Quaternion.Euler(characterRotation));
	}

	private void CameraFollow() {
		characterCam.transform.position = Vector3.SmoothDamp(characterCam.transform.position, camFloatingPoint.transform.position, ref smoothRef, tempDelta * cameraMovingSpeed);
		characterCam.transform.rotation = Quaternion.RotateTowards(characterCam.transform.rotation, camFloatingPoint.transform.rotation, tempDelta * cameraRotationgSpeed);
	}

	private void Jump() {
		if (groundedState == 0) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				groundedState = 1;
				body.AddForce(Vector3.up * tempDelta * jumpForce, ForceMode.Impulse);
				StartCoroutine(JumpReset());
			}
		}
		if (groundedState == 2) {
			body.AddForce(Vector3.down * tempDelta * jumpForce/2, ForceMode.Acceleration);
		}
	}

	private IEnumerator JumpReset() {
		yield return new WaitForSeconds(airTime);
		groundedState = 2;
		while (true) {
			yield return new WaitForSeconds(.1f);
			RaycastHit jumpHit;
			if (Physics.Raycast(transform.position,Vector3.down, out jumpHit,1)) {
				if (jumpHit.collider.tag == "Ground") {
					groundedState = 0;
					yield break;
				}
			}
		}
	}

	//private void ResetJump() {
	//	if (jumpingCounter > 0) {
	//		jumpingCounter--;
	//	}
		
	//}
}
