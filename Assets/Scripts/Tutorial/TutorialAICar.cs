using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialAICar : MonoBehaviour {


    private Rigidbody rb;
    public Transform[] wheels;

    WaypointScript furthest_waypoint;

    private const float off_time = 1.5f; //time to return when off-stage;
    private float cur_off_time = 0.0f;

    GameController gc;
    List<WaypointScript> waypoints;
    
    public float hoverDist = 3.0f;
    
    private bool onTrack;

    public static int LAYER_DEFAULT;
    public static int LAYER_SPECTRAL;

    public float thrustPower = 5.0f;

    public Color base_col { set; get; }
    public GameObject deathParticles;
    public GameObject respawnParticles;
    private bool canRespawn = true;

    private MeshRenderer[] meshes;
    public int id;
    float distance;
    int num_laps;
    bool primedForLap;

    private Vector3 startPos;
    private Quaternion startRot;

    // Use this for initialization
    void Awake() {
        LAYER_DEFAULT = LayerMask.NameToLayer("Default");
        LAYER_SPECTRAL = LayerMask.NameToLayer("Spectral");


        rb = GetComponent<Rigidbody>();

        gameObject.layer = LAYER_DEFAULT;
    }

    void Start() {
        furthest_waypoint = gc.getWaypoints()[0];

        meshes = GetComponentsInChildren<MeshRenderer>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    Vector3 nextWaypoint() {
        int wid = furthest_waypoint.id;
        wid++;

        WaypointScript[] waypoints = gc.getWaypoints();

        if (wid >= waypoints.Length) {
            wid = 0;
        }

        foreach (WaypointScript waypoint in waypoints) {
            if (waypoint.id == wid) {
                return waypoint.transform.position;
            }
        }

        throw new System.Exception("No Waypoint");
    }

    public void ResetToStart() {
        if (IsInvoking("Respawn")) {
            CancelInvoke("Respawn");
            Respawn();
        }

        transform.position = startPos;
        transform.rotation = startRot;
        
        num_laps = 0;
        primedForLap = false;
        cur_off_time = 0;
        onTrack = true;

        foreach (WaypointScript waypoint in gc.getWaypoints()) {
            if (waypoint.id == 0) {
                furthest_waypoint = waypoint;
            }
        }
    }

    void SpawnParticles(GameObject prefab) {
        GameObject particles = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
        ParticleSystem system = particles.GetComponent<ParticleSystem>();

        ParticleSystem.ShapeModule shape = system.shape;
        shape.mesh = GetComponent<MeshFilter>().mesh;

        system.startColor = base_col;
    }

    void SpawnDeathParticles() {
        foreach (MeshRenderer mesh in meshes) {
            mesh.enabled = false;
        }

        rb.isKinematic = true;

        SpawnParticles(deathParticles);
    }

    public void ReadyFields() {
        gc = FindObjectOfType<GameController>();

        waypoints = new List<WaypointScript>(gc.getWaypoints());
        
        base_col = gc.playerColors[id] * 2;

        HandleLapping();
    }

    // Update is called once per frame
    void Update() {

        //rb.AddForce(hit.normal * 9.8f * (2.0f - hit.distance));
        //transform.Rotate(Vector3.Cross(transform.up, hit.normal), Mathf.Min(Vector3.Angle(transform.up, hit.normal), 10 * Time.deltaTime), Space.World);
        
        HandleRaycast();
        HandleLapping();
        //GetComponentInChildren<Camera>().transform.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * Time.deltaTime * 60);
    }
    
    void HandleRaycast() {
        RaycastHit hit;

        Vector3 target = nextWaypoint();
        Vector3 dir = target - transform.position;
        dir.y = 0;
        dir = dir.normalized;
        
        foreach (Transform wheel in wheels) {
            onTrack = false;
            if (Physics.Raycast(wheel.position, -transform.up, out hit, 20.0f)) {
                onTrack = true;
                rb.AddForceAtPosition(hit.normal * 9.8f * (hoverDist - hit.distance) * 0.25f, wheel.position, ForceMode.Acceleration);
                
                rb.AddForce(transform.forward * thrustPower * gc.playerSpeedMultiplier, ForceMode.Acceleration);
                //rb.AddTorque(transform.up * Input.GetAxis("Horizontal"+id) * 1f, ForceMode.Acceleration);

                float h = Vector3.Angle(transform.forward, dir) * 0.05f;
                if (h > 2) {
                    h = 2;
                }

                rb.AddTorque(Vector3.Cross(transform.forward, dir).normalized * h, ForceMode.Acceleration);
                

            }
        }

        if (!onTrack) {
            if (cur_off_time < off_time) {
                cur_off_time += Time.deltaTime;
            } else {
                Die();
            }
        } else {
            cur_off_time = 0;
        }

    }

    void OnCollisionEnter(Collision col) {
        if (col.collider.CompareTag("KillYou")) {
            Die();
        }
    }

    public void Die() {
        if (canRespawn) {
            canRespawn = false;
            SpawnDeathParticles();
            
            Invoke("Respawn", 1.5f);
        }
    }

    void Respawn() {
        canRespawn = true;
        rb.velocity = Vector3.zero;
        transform.position = furthest_waypoint.transform.position;
        transform.rotation = furthest_waypoint.transform.rotation;

        cur_off_time = 0;

        foreach (MeshRenderer mesh in meshes) {
            mesh.enabled = true;
        }

        rb.isKinematic = false;

        SpawnParticles(deathParticles);
    }
    
    void HandleLapping() {

        if (!onTrack) {
            return;
        }

        waypoints.Sort((x, y) => x.distanceFrom(transform.position).CompareTo(y.distanceFrom(transform.position)));

        distance = WaypointScript.distBetween(waypoints[0], waypoints[1], transform.position);
        
        if (!primedForLap) {
            if (distance > 0.5f && distance < 0.55f) {
                primedForLap = true;
            }
        } else if (furthest_waypoint.id == 0) {
            primedForLap = false;
            num_laps++;
        }

        if (furthest_waypoint == null) {
            furthest_waypoint = waypoints[0];
        } else if (waypoints[0] == gc.getWaypoints()[0]) {
            furthest_waypoint = waypoints[0];
        } else if (furthest_waypoint != waypoints[0] && distance > waypoints[0].value && waypoints[0].value > furthest_waypoint.value) {
            furthest_waypoint = waypoints[0];
        }

    }

    public float getDistance() {
        return distance;
    }

    public int getLaps() {
        return num_laps;
    }

}
