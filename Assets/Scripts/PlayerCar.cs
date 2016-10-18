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

    PlayerAbilities abil;

	GameController gc;
    List<WaypointScript> waypoints;

    public GameObject[] UIPrefabs;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        abil = GetComponent<PlayerAbilities>();

		primedForLap = false;
		num_laps = 0;
		distance = 0;
    }

    public void AssignStuffAndShit()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        safe_pos_history = new Stack<Vector3>();
        safe_pos_history_b = new Stack<Vector3>();
        safe_pos_history.Push(transform.position);

        waypoints = new List<WaypointScript>(gc.getWaypoints());

        Camera cam = GetComponentInChildren<Camera>();
        GameController ctrl = FindObjectOfType<GameController>();
        float f = ctrl.getNumPlayers() == 2 ? 1.0f : 0.5f;

        Rect camRect = new Rect((id % 2) * 0.5f, (id / 2) * 0.5f, 0.5f, f);

        cam.rect = camRect;

        GameObject ui = Instantiate(UIPrefabs[id]);
        ui.transform.parent = transform;
        ui.transform.localPosition = Vector3.zero;
        ui.transform.localRotation = Quaternion.identity;
        ui.GetComponent<Camera>().rect = camRect;
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
            if (cur_off_time < off_time) {
                cur_off_time += Time.deltaTime;
            } else {
                rb.velocity = -rb.velocity;
                RaycastHit hit2;
                //while (!Physics.Raycast(transform.position, -transform.up, out hit, 20.0f))
                for (int i = 0; i < 10; i++)
                {
                    if (Physics.Raycast(transform.position, -transform.up, out hit2, 20.0f))
                    {
                        transform.up = hit2.normal;
                        break;
                    }
                    if (safe_pos_history_b.Count > 0)
                    {
                        transform.position = safe_pos_history_b.Pop();
                    }
                    else if(safe_pos_history.Count > 0)
                    {
                        transform.position = safe_pos_history.Pop();
                    }else
                    {
                        transform.position += rb.velocity;
                    }
                }
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
