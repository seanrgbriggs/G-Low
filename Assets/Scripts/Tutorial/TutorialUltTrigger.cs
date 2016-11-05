using UnityEngine;
using System.Collections;

public class TutorialUltTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        PlayerCar car = other.GetComponent<PlayerCar>();
        if (car != null && car.enabled) {
            FindObjectOfType<TutorialController>().PlayerHitTrigger();
        }
    }
}
