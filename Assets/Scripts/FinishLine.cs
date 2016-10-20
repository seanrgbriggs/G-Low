using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {
    private GameController gc;

	// Use this for initialization
	void Start () {
        gc = FindObjectOfType<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        PlayerCar car = other.GetComponent<PlayerCar>();
        if (car != null) {
            if (car.getLaps() >= gc.laps_max) {
                FindObjectOfType<OmniController>().EndGame();
            }
        }
    }
}
