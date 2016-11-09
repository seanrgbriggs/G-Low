using UnityEngine;
using System.Collections;

public class SphereProjectile : MonoBehaviour {

    Rigidbody rb;
    float hoverDist = 3.0f;
    float speed = 30.0f;

	GameObject shooter;
    Color base_col;

	public float drag = 5.0f;
	public float existence = 15.0f;

	// Use this for initialization
	void Start () {
      rb = GetComponent<Rigidbody>();
      rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.parent != null) {
            print(transform.parent.name + " is the parent of " + name);
			Rigidbody prb = transform.parent.GetComponent<Rigidbody> ();
			prb.AddForce (-prb.velocity, ForceMode.Acceleration);
			drag -= Time.deltaTime;
			existence = 15;
		} else {
			existence -= Time.deltaTime;
		}

		if (drag < 0 || existence < 0) {
			Destroy (gameObject);
		}


    }

	void OnTriggerEnter(Collider col){
		if (transform.parent != null && col.tag == "Player" && col.gameObject != shooter) {
			transform.parent = col.transform;
		}
	}

	public void SetShooter(GameObject go){
		shooter = go;
	}
}
