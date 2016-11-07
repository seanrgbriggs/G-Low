using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WaypointPlace : MonoBehaviour {

    public Vector3[] rays;

    public bool set = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (set) {
            foreach (Vector3 dir in rays) {
                RaycastHit hit;


                if (Physics.Raycast(transform.position - dir, dir, out hit, 2.0f)) {
                    print(hit.normal);

                    transform.position = hit.point + hit.normal * 6;
                    transform.up = hit.normal;
                    
                    return;
                }
            }

            set = false;
        }
    }
}
