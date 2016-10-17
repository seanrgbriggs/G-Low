using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCar : MonoBehaviour {
    

	private Rigidbody rb;
    public Transform[] wheels;
	public int id;


	float distance;
	int num_laps;
	bool primedForLap;

	private Vector3 last_safe_pos;
	private const float off_time = 1.5f; //time to return when off-stage;
	private float cur_off_time = 0.0f;

	float ab_cooldown;
	float ult_cooldown;

	GameController gc;
	List<WaypointScript> waypoints;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

		primedForLap = false;
		num_laps = 0;
		distance = 0;

		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		 
		waypoints = new List<WaypointScript> ();
		waypoints.AddRange (gc.getWaypoints ());
	}
	
	// Update is called once per frame
	void Update () {

            //rb.AddForce(hit.normal * 9.8f * (2.0f - hit.distance));
            //transform.Rotate(Vector3.Cross(transform.up, hit.normal), Mathf.Min(Vector3.Angle(transform.up, hit.normal), 10 * Time.deltaTime), Space.World);

		HandleRaycast ();
		HandleLapping ();
 		//GetComponentInChildren<Camera>().transform.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * Time.deltaTime * 60);
    }

	void HandleRaycast() {
		RaycastHit hit;
		bool safe = false;

        GetComponentInChildren<Camera>().transform.RotateAround(transform.position, transform.up, Input.GetAxis("HLook" + id) * Time.deltaTime * 60);

        foreach (Transform wheel in wheels) {
			if (Physics.Raycast(wheel.position, -transform.up, out hit, 20.0f)) {

				rb.AddForceAtPosition(hit.normal * 9.8f * (2.0f - hit.distance) * 0.25f, wheel.position, ForceMode.Acceleration);


				rb.AddForce(transform.forward * Input.GetAxis("Vertical"+id) * 5, ForceMode.Acceleration);
				rb.AddTorque(transform.up * Input.GetAxis("Horizontal"+id) * 1f, ForceMode.Acceleration);
				last_safe_pos = transform.position;
				if (!safe)
					safe = true;
			}
		}
		if (!safe) {
			print (name + "is unsafe");
			if (cur_off_time < off_time) {
				cur_off_time += Time.deltaTime;
			}else{
				print (name + "is too slow");
				rb.velocity = -rb.velocity;
				transform.position = last_safe_pos + rb.velocity.normalized;
				GameObject track = GameObject.FindGameObjectWithTag ("Track");
				print (track.name);
				transform.up = transform.position - track.GetComponent<Collider> ().ClosestPointOnBounds (transform.position);

				cur_off_time = 0;
			}
		}

	}

	void HandleLapping(){
		waypoints.Sort ((x, y) => x.distanceFrom(transform.position).CompareTo(y.distanceFrom(transform.position)));
		distance = WaypointScript.distBetween (waypoints.ToArray () [0], waypoints.ToArray () [1], transform.position);
		if (!primedForLap && distance > 0.5f && distance < 0.55f) {
			primedForLap = true;
		} else if (primedForLap && (distance > 0.98f || distance < 0.03f)) {
			num_laps++;
			primedForLap = false;
		}
	}

 

	protected virtual void UseUltimate(){}

	protected virtual void UseAbility(){}

	public float getDistance(){
		return distance;
	}

	public int getLaps(){
		return num_laps;
	}

	public float getAbilCooldown(){
		return ab_cooldown;
	}

	public float getUltCooldown(){
		return ult_cooldown;
	}
}
