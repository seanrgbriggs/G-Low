using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbPartGenerator : MonoBehaviour {

	public GameObject part_prefab;
	public int coverage = 3;
	public float radius = 5;

	public GameObject[] parts { set; get; }

    Stack<GameObject> enabled_parts;
    Stack<GameObject> disabled_parts;

    // Use this for initialization
    void Awake () {
		HyperCubeRejection ();

        enabled_parts = new Stack<GameObject>(parts);
        disabled_parts = new Stack<GameObject>();
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
                new_orb_part.GetComponent<Renderer>().material.SetColor("_EmissionColor", GetComponent<PlayerCar>().base_col);
				newparts.Add (new_orb_part);
                
			}
		}

		parts = newparts.ToArray();

		return;
	}

    public bool Disable() {
        if (enabled_parts.Count <= 0) {
            return false;
        }

        GameObject part = enabled_parts.Pop();
        part.SetActive(false);
        disabled_parts.Push(part);
        return true;
    }
    public bool Enable()
    {
        if (disabled_parts.Count <= 0)
        {
            return false;
        }

        GameObject part = disabled_parts.Pop();
        part.SetActive(true);
        part.GetComponent<OrbPartPhysics>().SpawnParticles();
        enabled_parts.Push(part);
        return true;
    }
}
