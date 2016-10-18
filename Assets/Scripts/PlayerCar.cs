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

    private Stack<Vector3> safe_pos_history;
    private Stack<Vector3> safe_pos_history_b;
    private const int max_pos_buffer_count = 120;

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

        safe_pos_history = new Stack<Vector3>();
        safe_pos_history_b = new Stack<Vector3>();
        safe_pos_history.Push(transform.position);

        waypoints = new List<WaypointScript>(gc.getWaypoints());
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
				if (!safe)
					safe = true;
			}
		}
		if (!safe) {
			print (name + "is unsafe");
            if (cur_off_time < off_time) {
                print(safe_pos_history.Count);
                cur_off_time += Time.deltaTime;
            } else {
                print(name + "is too slow");
                rb.velocity = -rb.velocity;
                while (!Physics.Raycast(transform.position, -transform.up, out hit, 20.0f))
                {
                    if (safe_pos_history_b.Count > 0)
                    {
                        transform.position = safe_pos_history_b.Pop();
                    }
                    else
                    {
                        transform.position = safe_pos_history.Pop();
                    }
                }
                transform.up = hit.normal;
				cur_off_time = 0;
			}
        }else{
            if (safe_pos_history.Count < max_pos_buffer_count)
            {
                safe_pos_history.Push(transform.position);
            }else if(safe_pos_history_b.Count < max_pos_buffer_count * 0.9f)
            {
                safe_pos_history_b.Push(transform.position);
            }else
            {
                safe_pos_history = safe_pos_history_b;
                safe_pos_history_b.Clear();
            }
        }

	}

	void HandleLapping(){

        waypoints.Sort((x, y) => x.distanceFrom(transform.position).CompareTo(y.distanceFrom(transform.position)));

        distance = WaypointScript.distBetween (waypoints [0], waypoints [1], transform.position);
		if (!primedForLap && distance > 0.5f && distance < 0.55f) {
			primedForLap = true;
		} else if (primedForLap && (distance > 0.98f || distance < 0.03f)) {
			num_laps++;
			primedForLap = false;
		}

        if (id == 0)
        {
            print(waypoints[0].id + "-" + waypoints[1].id+": "+distance);
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
