using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class freshbeatsyomang : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        string scene_name = SceneManager.GetActiveScene().name;

        if (scene_name != "CharSel" && scene_name != "MapSel")
        {
            Destroy(this.gameObject);
        }
	}
}
