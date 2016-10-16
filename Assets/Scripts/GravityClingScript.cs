using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityClingScript : MonoBehaviour {

	Rigidbody rb;
	SphereCollider col;

	private List<Vector3> points;
	public Vector3 gravity;
	public float gravityMagnitude;

	public double Float;

	// Use this for initialization
	void Start () {
		col = GetComponent<SphereCollider> ();
		rb = GetComponent<Rigidbody> ();
		points = new List<Vector3> ();
 	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		if (Physics.Raycast (transform.position, -transform.up, out hit, (float)Float * 4)) {
			gravity = -hit.normal;
		}

		float y_val = transform.rotation.eulerAngles.y;

		transform.position = Vector3.Lerp (transform.position, hit.point + (float) Float * hit.normal, 0.05f);

		//transform.LookAt(transform.forward, Vector3.Lerp(transform.up, hit.normal, 0.01f));

		rb.velocity = 5 * (Input.GetAxis ("Vertical") * transform.forward);//+ Input.GetAxis("Horizontal") * transform.right));
		print(rb.velocity + " " + transform.forward);
		//transform.rotation = Quaternion.Euler (0,  y_val + Input.GetAxis ("Horizontal"), 0);
	}


}
