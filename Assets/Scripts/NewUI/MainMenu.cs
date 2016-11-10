using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play() {
        SceneManager.LoadScene("MapSel", LoadSceneMode.Single);
    }

    public void Tutorial() {
        SceneManager.LoadScene("tutorial", LoadSceneMode.Single);
    }

    public void Quit() {
        Application.Quit();
    }
}
