using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject[] spells = new GameObject[4];

	public float moveSpeed;
	private Rigidbody rb;

	private Vector3 moveInput;
	private Vector3 moveVelocity;

	public Camera mainCamera;	

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		moveVelocity = moveInput * moveSpeed;

		Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float rayLength;

		if (groundPlane.Raycast(cameraRay, out rayLength)){
			Vector3 pointToLook = cameraRay.GetPoint(rayLength);
			Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

			transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));

		}

		if (Input.GetButtonUp("Fire1")){
			Instantiate(spells[0], transform.position, transform.rotation);
		}

		// shield
		if (Input.GetButtonUp("Fire2")){
			Quaternion rotation = transform.rotation;
			rotation.y -= 90f;
			GameObject shield = Instantiate(spells[1], transform.position, rotation);
			shield.GetComponent<ShieldController>().Bind(gameObject);
		}
	}

	void FixedUpdate() {
		rb.velocity = moveVelocity;
	}
}
