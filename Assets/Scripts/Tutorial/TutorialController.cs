using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour {
    AudioSource src;
    PlayerCar playerCar;
    TutorialAICar enemyCar;
    public AudioClip[] tutorialClips;

    public GameObject enemyPrefab;
    public bool waitForFinish = false;
    public bool waitForFinish2 = false;
    
    public GameObject[] enableForUltimate;

	// Use this for initialization
	void Start () {
        src = GetComponents<AudioSource>()[1];
        playerCar = FindObjectOfType<PlayerCar>();

        Invoke("BeginTutorial", 1.0f);

        foreach (GameObject obj in enableForUltimate) {
            obj.SetActive(false);
        }
	}
	
	void BeginTutorial() {

        PlayClip(0);
        Invoke("CamControls", 5.0f);
    }

    void CamControls() {
        PlayClip(1);
        Invoke("AccelerateControl", 5.0f);
    }

    void AccelerateControl() {
        PlayClip(2);
        Invoke("DecelerateControl", 7.0f);
    }

    void DecelerateControl() {
        PlayClip(3);
        Invoke("BrakeControl", 6.0f);
    }

    void BrakeControl() {
        PlayClip(4);
        Invoke("RespawnControl", 4.0f);
    }

    void RespawnControl() {
        playerCar.GetComponent<Rigidbody>().AddForce(Vector3.up * 100, ForceMode.VelocityChange);
        PlayClip(5);
        Invoke("Objectives", 10.0f);
    }

    void Objectives() {
        playerCar.ResetToStart();
        PlayClip(6);
        playerCar.GetComponent<Rigidbody>().isKinematic = true;
        Invoke("Race", 9.0f);

    }

    void Race() {
        GameObject enemy = Instantiate(enemyPrefab);
        enemyCar = enemy.GetComponent<TutorialAICar>();
        enemy.transform.position = new Vector3(-306, -6.32f, -8.2f);
        enemyCar.ReadyFields();
        
        playerCar.GetComponent<Rigidbody>().isKinematic = false;
        PlayClip(7);
        waitForFinish = true;
    }

    void Abilities() {
        PlayClip(8);
        waitForFinish = false;
        playerCar.ResetToStart();
        playerCar.transform.position += new Vector3(0, 0, -200);
        playerCar.GetComponent<Rigidbody>().isKinematic = true;

        enemyCar.ResetToStart();
        enemyCar.GetComponent<Rigidbody>().isKinematic = true;

        Invoke("Nitrous", 9.0f);
    }

    void Nitrous() {
        PlayClip(9);
        Invoke("UseNitrous", 3.0f);
    }

    void UseNitrous() {
        playerCar.GetComponent<Rigidbody>().isKinematic = false;
        enemyCar.GetComponent<Rigidbody>().isKinematic = false;
        waitForFinish2 = true;
    }

    void Ultimate() {
        waitForFinish2 = false;
        playerCar.ResetToStart();
        playerCar.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(enemyCar.gameObject);

        foreach (GameObject obj in enableForUltimate) {
            obj.SetActive(true);
        }

        PlayClip(10);
        Invoke("UseUltimate", 8);
    }

    void UseUltimate() {
        playerCar.GetComponent<Rigidbody>().isKinematic = false;
    }

    void AbilitiesRecap() {
        PlayClip(11);
        Invoke("Outro", 5.0f);
    }

    void Outro() {
        PlayClip(12);
        Invoke("End", 6.0f);
    }

    void End() {
        SceneManager.LoadScene("Menu2", LoadSceneMode.Single);
    }

    void PlayClip(int index) {
        src.Stop();
        src.clip = tutorialClips[index];
        src.Play();
    }

    public void PlayerHitTrigger() {
        print("HI");
        Invoke("AbilitiesRecap", 1.0f);
    }

    public void PlayerCrossedFinish() {
        if (waitForFinish) {
            Invoke("Abilities", 1.0f);
        }

        if (waitForFinish2) {
            Invoke("Ultimate", 1.0f);
        }
    }
}
