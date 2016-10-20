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
    public float hoverDist = 3.0f;

    Vector3 camAngles;

    public float handBrakePower = 1.0f;
    private float drag;
    private bool onTrack;

    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody>();
        abil = GetComponent<PlayerAbilities>();

		primedForLap = false;
		num_laps = 0;
		distance = 0;

        base_col = GetComponent<Renderer>().material.GetColor("_EmissionColor");
        drag = rb.drag;
    }

    public void ReadyFields()
    {
        gc = FindObjectOfType<GameController>();

        waypoints = new List<WaypointScript>(gc.getWaypoints());

        Camera cam = GetComponentInChildren<Camera>();
        float f = gc.getNumPlayers() == 2 ? 1.0f : 0.5f;

        Rect camRect;
        if (gc.getNumPlayers() == 1) {
            camRect = new Rect(0, 0, 1, 1);
        } else if (gc.getNumPlayers() == 2) {
            camRect = new Rect(id * 0.5f, 0, 0.5f, 1);
        } else {
            camRect = new Rect((id % 2) * 0.5f, (id / 2) * 0.5f, 0.5f, 1);
        }

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

        Camera cam = GetComponentInChildren<Camera>();
        
        cam.transform.eulerAngles = camAngles;
        Vector3 v = cam.transform.localEulerAngles;
        v.x = 0;
        v.y += Input.GetAxis("Horizontal" + id) * Time.deltaTime * 120;
        v.z = 0;
        cam.transform.localEulerAngles = v;
        cam.transform.position = transform.position - cam.transform.forward * 10;
        v = cam.transform.localPosition;
        v.y = 3.25f;
        cam.transform.localPosition = v;

        camAngles = cam.transform.eulerAngles;
        
        foreach (Transform wheel in wheels) {
            onTrack = false;
			if (Physics.Raycast(wheel.position, -transform.up, out hit, 20.0f)) {
                onTrack = true;
                rb.AddForceAtPosition(hit.normal * 9.8f * (hoverDist - hit.distance) * 0.25f, wheel.position, ForceMode.Acceleration);


                if (Input.GetButton("Brake"+id)) {
                    rb.drag = handBrakePower;
                } else {
                    rb.drag = drag;
                    rb.AddForce(transform.forward * Input.GetAxis("HLook" + id) * 5 * gc.playerSpeedMultiplier, ForceMode.Acceleration);
                    //rb.AddTorque(transform.up * Input.GetAxis("Horizontal"+id) * 1f, ForceMode.Acceleration);

                    float h = Vector3.Angle(transform.forward, cam.transform.forward) * 0.05f * Mathf.Abs(Input.GetAxis("HLook" + id));
                    if (h > 2) {
                        h = 2;
                    }

                    rb.AddTorque(Vector3.Cross(transform.forward, cam.transform.forward).normalized * h, ForceMode.Acceleration);
                }

            }
		}

		if (Input.GetButton("Sudoku"+id)) {
            if (cur_off_time < off_time) {
                cur_off_time += Time.deltaTime;
            } else {
                rb.velocity = Vector3.zero;
                transform.position = furthest_waypoint.transform.position;
                transform.LookAt(furthest_waypoint.transform.position + furthest_waypoint.transform.forward, furthest_waypoint.transform.up);
                   

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
        float brightness;
        float max = 50 * gc.playerSpeedMultiplier;
        if (onTrack) {
            brightness = Mathf.Max((rb.velocity.magnitude - max) / 10, 1);
        } else {
            brightness = rb.velocity.magnitude / max;
        }
        Material m = GetComponent<Renderer>().material;
        m.SetColor("_EmissionColor", base_col * brightness);// (1 - cur_off_time / off_time));
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
