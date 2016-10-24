using UnityEngine;
using System.Collections;

public class OrbPartPhysics : MonoBehaviour {

	public Vector3 center;
	public Vector3 delta;
	public Vector3 init;
	public Vector3 direction;

	//float bounciness;
	public float min_bounce;

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		center = transform.parent.position;
		delta = Vector3.zero;
		init = transform.position;


		//bounciness = (init - center).magnitude;
		direction = (init - center).normalized;

		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		center = transform.parent.position;
		rb.AddForce (center - transform.position, ForceMode.Acceleration);
		rb.AddTorque (transform.up * 4);

		//delta = (min_bounce + Mathf.Pow(Mathf.PingPong (Time.time, Mathf.Sqrt(bounciness - min_bounce)), 2)) * direction;

		//transform.position = center + delta; //Vector3.Lerp (transform.position, center + delta, 0.05f);

		//transform.RotateAround (center, transform.up, 10f);

	}
}
