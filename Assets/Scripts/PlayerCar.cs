using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCar : MonoBehaviour {
    private Rigidbody rb;
    public Transform[] wheels;

	float distance;

	int num_laps;
	bool primedForLap;

	GameController gc;
	Queue<WaypointScript> waypoints;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

		primedForLap = false;
		num_laps = 0;
		distance = 0;

		gc = GameObject.FindGameObjectWithTag ("Controller").GetComponent<GameController>();

		waypoints = new Queue<WaypointScript> ();
		WaypointScript[] wp_arr = gc.getWaypoints ();
		foreach (WaypointScript wp in wp_arr) {
			waypoints.Enqueue (wp);
		}
	}
	
	// Update is called once per frame
	void Update () {

            //rb.AddForce(hit.normal * 9.8f * (2.0f - hit.distance));
            //transform.Rotate(Vector3.Cross(transform.up, hit.normal), Mathf.Min(Vector3.Angle(transform.up, hit.normal), 10 * Time.deltaTime), Space.World);

		HandleRaycast ();

		GetComponentInChildren<Camera>().transform.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * Time.deltaTime * 60);
    }

	void HandleRaycast(){
		RaycastHit hit;
		foreach (Transform wheel in wheels) {
			if (Physics.Raycast(wheel.position, -transform.up, out hit, 20.0f)) {

				rb.AddForceAtPosition(hit.normal * 9.8f * (2.0f - hit.distance) * 0.25f, wheel.position, ForceMode.Acceleration);


				rb.AddForce(transform.forward * Input.GetAxis("Vertical") * 5, ForceMode.Acceleration);
				rb.AddTorque(transform.up * Input.GetAxis("Horizontal") * 1, ForceMode.Acceleration);
							}
		}

	}

	void HandleLapping(){
		if (!primedForLap && distance > 0.5f && distance < 0.55f) {
			
		}
	}
}
