using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialFinishLine : MonoBehaviour {
    private TutorialController tc;

    // Use this for initialization
    void Start() {
        tc = FindObjectOfType<TutorialController>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        PlayerCar car = other.GetComponent<PlayerCar>();
        if (car != null && car.enabled && car.getLaps() > 0) {
            tc.PlayerCrossedFinish();
        }
    }
}
