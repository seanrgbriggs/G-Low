﻿using UnityEngine;
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

    public Color base_col { set; get; }
    public float hoverDist = 3.0f;
    public float raycastStart = 3.0f;

    Vector3 camAngles;

    public float handBrakePower = 1.0f;
    private float drag;
    private bool onTrack;

    public GameObject victoryPrefab;

    private MeshRenderer[] meshes;

    public static int LAYER_DEFAULT;
    public static int LAYER_SPECTRAL;

    public float thrustPower = 5.0f;

    public GameObject deathParticles;
    public GameObject respawnParticles;
    private bool canRespawn = true;

    public AudioClip respawnSound;
    public AudioClip deathSound;

    private Vector3 startPos;
    private Quaternion startRot;

    public bool enableGravity = true;

    // Use this for initialization
    void Awake () {
        LAYER_DEFAULT = LayerMask.NameToLayer("Ignore Raycast");
        LAYER_SPECTRAL = LayerMask.NameToLayer("Spectral");


        rb = GetComponent<Rigidbody>();
        abil = GetComponent<PlayerAbilities>();

		primedForLap = false;
		num_laps = 0;
		distance = 0;

        gameObject.layer = LAYER_DEFAULT;
        
        drag = rb.drag;

    }

    void Start() {
        furthest_waypoint = gc.getWaypoints()[0];

        startPos = transform.position;
        startRot = transform.rotation;
        meshes = GetComponentsInChildren<MeshRenderer>();

    }

    public void ResetToStart() {
        if (IsInvoking("Respawn")) {
            CancelInvoke("Respawn");
            Respawn();
        }

        transform.position = startPos;
        transform.rotation = startRot;

        camAngles = new Vector3(0, 0, 0);
        num_laps = 0;
        primedForLap = false;
        cur_off_time = 0;
        onTrack = true;
        
        foreach (WaypointScript waypoint in gc.getWaypoints()) {
            if (waypoint.id == 0) {
                furthest_waypoint = waypoint;
            }
        }

        HandleCamera();
    }

    void SpawnParticles(GameObject prefab)
    {
        GameObject particles = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
        ParticleSystem system = particles.GetComponent<ParticleSystem>();

        ParticleSystem.ShapeModule shape = system.shape;
        shape.mesh = GetComponent<MeshFilter>().mesh;

        system.startColor = base_col;
    }

    void SpawnDeathParticles()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = false;
        }

        rb.isKinematic = true;

        SpawnParticles(deathParticles);
    }

    public void ReadyFields()
    {
        gc = FindObjectOfType<GameController>();

        waypoints = new List<WaypointScript>(gc.getWaypoints());

        Camera cam = GetComponentInChildren<Camera>();

        Rect camRect;
        if (gc.getNumPlayers() == 1) {
            camRect = new Rect(0, 0, 1, 1);
        } else if (gc.getNumPlayers() == 2) {
            camRect = new Rect(id * 0.5f, 0, 0.5f, 1);
        } else {
            camRect = new Rect((id % 2) * 0.5f, (id / 2) * 0.5f, 0.5f, 1);
        }

        base_col = gc.playerColors[id] * 2;

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

        HandleCamera();
		HandleRaycast ();
        HandleDimming();
		HandleLapping ();

        abil.enabled = onTrack;
        //GetComponentInChildren<Camera>().transform.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * Time.deltaTime * 60);
     }

    void HandleCamera() {
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
    }

	void HandleRaycast() {
		RaycastHit hit;

        int mask = (1 << LayerMask.NameToLayer("Track")) | (1 << LayerMask.NameToLayer("Default"));

        Camera cam = GetComponentInChildren<Camera>();
        foreach (Transform wheel in wheels) {
            onTrack = false;
			if (Physics.Raycast(wheel.position + transform.up * raycastStart, -transform.up, out hit, 50.0f, mask)) {
                onTrack = true;
                if (enableGravity) {
                    float f = 10.0f;
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Track")) {
                        f = 50.0f;
                    }
                    rb.AddForceAtPosition(hit.normal * f * (hoverDist - hit.distance + raycastStart) * 0.25f, wheel.position, ForceMode.Acceleration);
                }


                if (Input.GetButton("Brake"+id)) {
                    rb.drag = handBrakePower;
                } else {
                    rb.drag = drag;
                    rb.AddForce(transform.forward * Input.GetAxis("HLook" + id) * thrustPower * gc.playerSpeedMultiplier, ForceMode.Acceleration);
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
                Die();
            }
        }else
        {
            cur_off_time = 0;
        }

	}

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("KillYou"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (canRespawn)
        {
            canRespawn = false;
            SpawnDeathParticles();

            GetComponent<AudioSource>().PlayOneShot(deathSound, 1);
            Invoke("PlayRezNoise", 0.5f);
            Invoke("Respawn", 1.5f);
        }
    }

    void PlayRezNoise() {
        GetComponent<AudioSource>().PlayOneShot(respawnSound, 0.25f);
    }

    void Respawn()
    {
        canRespawn = true;
        rb.velocity = Vector3.zero;
        transform.position = furthest_waypoint.transform.position;
        transform.rotation = furthest_waypoint.transform.rotation;
        camAngles = furthest_waypoint.transform.rotation.eulerAngles;
           
        cur_off_time = 0;

        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = true;
        }

        rb.isKinematic = false;

        SpawnParticles(deathParticles);
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

        foreach (MeshRenderer mesh in meshes) {
            mesh.material.SetColor("_EmissionColor", base_col * brightness);
        }
    }

    void HandleLapping() {

        if (!onTrack) {
            return;
        }

        waypoints.Sort((x, y) => x.distanceFrom(transform.position).CompareTo(y.distanceFrom(transform.position)));

        distance = WaypointScript.distBetween(waypoints[0], waypoints[1], transform.position);
        /*if (distance > 0.5f && distance < 0.55f) {
            if (waypoints[0].id == 0 || waypoints[1].id == 0) {
                if (primedForLap) {
                    num_laps++;
                    furthest_waypoint = waypoints[0];
                    primedForLap = false;
                    print("LAP");
                }
            } else {
                if (!primedForLap) {
                    primedForLap = true;
                    print("PRIMED");
                }
            }
        }*/

        if (!primedForLap)
        {
            if (distance > 0.5f && distance < 0.55f)
            {
                primedForLap = true;
            }
        }
        else if (furthest_waypoint.id == 0) {
            primedForLap = false;
            num_laps++;
        }

        if (furthest_waypoint == null)
        {
            furthest_waypoint = waypoints[0];
        } else if (waypoints[0] == gc.getWaypoints()[0]) {
            furthest_waypoint = waypoints[0];
        } else if (furthest_waypoint != waypoints[0] && distance > waypoints[0].value && waypoints[0].value > furthest_waypoint.value)
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
