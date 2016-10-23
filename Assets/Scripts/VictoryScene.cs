using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour {
    public GameObject[] markers;

	// Use this for initialization
	void Start () {
	    for (int i = 0; i < markers.Length; i++) {
            GameObject marker = markers[i];
            if (FinishLine.winners != null && FinishLine.winners[i] != null) {
                GameObject obj = Instantiate(FinishLine.winners[i]);
                obj.transform.parent = marker.transform.parent;
                obj.transform.localPosition = marker.transform.localPosition;
                obj.transform.localRotation = marker.transform.localRotation;

                Color base_col = GameController.staticColors[i] * 2;
                
                foreach (MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>()) {
                    mesh.material = Instantiate(mesh.material);
                    mesh.material.SetColor("_EmissionColor", base_col);
                }
            }

            Destroy(marker);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")) {
            if (Time.timeSinceLevelLoad > 1.0f) {
                SceneManager.LoadScene("Menu2", LoadSceneMode.Single);
            }
        }
	}
}
