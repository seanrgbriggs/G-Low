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

    WaypointScript furthest_waypoint;

    private const float off_time = 1.5f; //time to return when off-stage;
	private float cur_off_time = 0.0f;

    PlayerAbilities abil;

	GameController gc;
    List<WaypointScript> waypoints;

    Color base_col;

    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody>();
        abil = GetComponent<PlayerAbilities>();

		primedForLap = false;
		num_laps = 0;
		distance = 0;

        base_col = GetComponent<Renderer>().material.GetColor("_EmissionColor");
    }

    public void ReadyFields()
    {
        gc = FindObjectOfType<GameController>();

        waypoints = new List<WaypointScript>(gc.getWaypoints());

        Camera cam = GetComponentInChildren<Camera>();
        float f = gc.getNumPlayers() == 2 ? 1.0f : 0.5f;

        Rect camRect = new Rect((id % 2) * 0.5f, (id / 2) * 0.5f, 0.5f, f);

        cam.rect = camRect;

        GameObject ui = Instantiate(gc.UIPrefabs[id]);
        ui.transform.parent = transform;
        ui.transform.localPosition = Vector3.zero;
        ui.transform.localRotation = Quaternion.identity;
        ui.GetComponent<Camera>().rect = camRect;

        HandleLapping();
    }

    // Update is called once per frame
    void Update () {

            //rb.AddForce(hit.normal * 9.8f * (2.0f - hit.distance));
            //transform.Rotate(Vector3.Cross(transform.up, hit.normal), Mathf.Min(Vector3.Angle(transform.up, hit.normal), 10 * Time.deltaTime), Space.World);

		HandleRaycast ();
        HandleDimming();
		HandleLapping ();
 		//GetComponentInChildren<Camera>().transform.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * Time.deltaTime * 60);
    }

	void HandleRaycast() {
		RaycastHit hit;

        GetComponentInChildren<Camera>().transform.RotateAround(transform.position, transform.up, Input.GetAxis("HLook" + id) * Time.deltaTime * 60);

        foreach (Transform wheel in wheels) {
			if (Physics.Raycast(wheel.position, -transform.up, out hit, 20.0f)) {
				rb.AddForceAtPosition(hit.normal * 9.8f * (2.0f - hit.distance) * 0.25f, wheel.position, ForceMode.Acceleration);

                
                rb.AddForce(transform.forward * Input.GetAxis("Vertical"+id) * 5, ForceMode.Acceleration);
				rb.AddTorque(transform.up * Input.GetAxis("Horizontal"+id) * 1f, ForceMode.Acceleration);

			}
		}

		if (false) {// Input.GetButton("Sudoku"+id)) {
            if (cur_off_time < off_time) {
                cur_off_time += Time.deltaTime;
            } else {
                rb.velocity = -rb.velocity;
                RaycastHit hit2;

                    if (Physics.Raycast(transform.position, -transform.up, out hit2, 20.0f))
                    {
                        transform.up = hit2.normal;
                    }else
                    {
                        transform.position = furthest_waypoint.transform.position+ furthest_waypoint.transform.up * 4;
                        transform.LookAt(furthest_waypoint.transform.position + furthest_waypoint.transform.forward, furthest_waypoint.transform.up);
                    }

                cur_off_time = 0;
                print(transform.up);
            }
        }else
        {
            cur_off_time = 0;
        }

	}

    void HandleDimming()
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", base_col * (1 - cur_off_time / off_time));
    }

    void HandleLapping(){
       
        waypoints.Sort((x, y) => x.distanceFrom(transform.position).CompareTo(y.distanceFrom(transform.position)));

        distance = WaypointScript.distBetween (waypoints [0], waypoints [1], transform.position);
		if (!primedForLap && distance > 0.5f && distance < 0.55f) {
			primedForLap = true;
		} else if (primedForLap && (distance > 0.98f || distance < 0.03f)) {
			num_laps++;
            furthest_waypoint = waypoints[0];
            primedForLap = false;
		}
 
        if(furthest_waypoint == null)
        {
            furthest_waypoint = waypoints[0];
        }else if(furthest_waypoint != waypoints[0] && distance > waypoints[0].value && waypoints[0].value > furthest_waypoint.value)
        {
            furthest_waypoint = waypoints[0];
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

	public PlayerAbilities getAbilities(){
		return abil;
	}

}
