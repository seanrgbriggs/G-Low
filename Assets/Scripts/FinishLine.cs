using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour {
    private GameController gc;

    public static GameObject[] winners;

    private int place = 0;
    

	// Use this for initialization
	void Start () {
        winners = new GameObject[4];
        gc = FindObjectOfType<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        PlayerCar car = other.GetComponent<PlayerCar>();
        if (car != null && car.enabled) {
            print(car.getLaps());
            if (car.getLaps() >= gc.laps_max) {
                winners[place++] = car.victoryPrefab;

                if (place >= gc.getNumPlayers()) {
                    SceneManager.LoadScene("Victory", LoadSceneMode.Single);
                }

                car.enabled = false;
                car.GetComponent<PlayerAbilities>().enabled = false;
                car.GetComponent<Rigidbody>().isKinematic = true;
                car.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
