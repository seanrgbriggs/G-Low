using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbPartGenerator : MonoBehaviour {

	public GameObject part_prefab;
	public int coverage = 3;
	public float radius = 5;

	GameObject[] parts;

	// Use this for initialization
	void Start () {
		HyperCubeRejection ();
	}

	void HyperCubeRejection (){
		List<GameObject> newparts = new List<GameObject> ();
		float a = 4 * Mathf.PI / coverage;
		float d = Mathf.Sqrt (a);
		float m_theta = Mathf.Round (Mathf.PI / d);
		float d_theta = Mathf.PI / m_theta;
		float d_phi = a / d_theta;
		for (int m = 0; m < m_theta; m++) {
			float theta = Mathf.PI * (m + 0.5f) / ( m_theta);
			float m_phi = Mathf.Round (2 * Mathf.PI * Mathf.Sin (theta) / d_phi);
			for (int n = 0; n < m_phi; n++) {
				float phi = 2 * Mathf.PI * n / m_phi;
				Vector3 delta = new Vector3 (Mathf.Sin (theta) * Mathf.Cos (phi), Mathf.Sin (theta) * Mathf.Sin (phi), Mathf.Cos (theta));
				delta *= radius;
				GameObject new_orb_part = (GameObject) Instantiate(part_prefab, transform.position + delta, Quaternion.identity);
				new_orb_part.transform.SetParent (transform);
				newparts.Add (new_orb_part);
			}
		}

		parts = newparts.ToArray();

		return;
	}
	
	// Update is called once per frame
	void Update () {
		OrbPartPhysics part = parts [0].GetComponent<OrbPartPhysics>();
		print (part.direction + " " + part.delta);	
	}
}
