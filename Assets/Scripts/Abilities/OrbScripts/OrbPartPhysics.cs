using UnityEngine;
using System.Collections;

public class OrbPartPhysics : MonoBehaviour {

	public Vector3 center;
	public Vector3 delta;
	public Vector3 init;
	public Vector3 direction;

    float bounciness;
	public float min_bounce;
    Vector3 axis;

 	Rigidbody rb;

    public GameObject deathParticles;

    // Use this for initialization
    void Start () {
		center = transform.parent.position;
		delta = Vector3.zero;


		init = transform.localPosition;
		bounciness = init.magnitude * Random.value;
		direction = init.normalized;

        axis = new Vector3(Random.value, Random.value, Random.value);
        axis.Normalize();

        rb = GetComponent<Rigidbody> ();

        Color line_base_color = transform.parent.GetComponent<PlayerCar>().base_col;
        line_base_color *= 0.5f;
        line_base_color.a *= 0.5f;
        GetComponent<LineRenderer>().SetColors(line_base_color, Color.white);

    }

    // Update is called once per frame
    void Update () {

		center = transform.parent.position;
        delta = direction * (min_bounce + Mathf.Sin(Time.time) * bounciness);

        transform.position = center + delta;
        transform.RotateAround(center, axis, 15f);

        GetComponent<LineRenderer>().SetPosition(1, transform.InverseTransformPoint(transform.parent.position));
        //GetComponent<LineRenderer>().SetPosition(1, -transform.localPosition);
        DeathLock(!GetComponent<MeshRenderer>().enabled);


    }

    Vector3 RotateDelta(Vector3 delta, float angle) {
        Vector3 temp_delta = delta.normalized * delta.magnitude;

        delta.x = temp_delta.x * Mathf.Cos(angle) - temp_delta.z * Mathf.Sin(angle);
        delta.z = temp_delta.x * Mathf.Sin(angle) + temp_delta.z * Mathf.Cos(angle);
        //print("Before: " + delta);
        return delta;
    }

    public void SpawnParticles()
    {
        GameObject particles = (GameObject)Instantiate(deathParticles, transform.position, transform.rotation);
        particles.transform.parent = transform.parent;
        ParticleSystem system = particles.GetComponent<ParticleSystem>();

        ParticleSystem.ShapeModule shape = system.shape;
        shape.mesh = GetComponent<MeshFilter>().mesh;

        system.startColor = transform.parent.GetComponent<PlayerCar>().base_col;
    }

    bool lastDeathState = false;
    void DeathLock(bool dead) {
        if (dead != lastDeathState) {
            lastDeathState = dead;
            if (dead) {
                SpawnParticles();
            }
        }
    }
}
