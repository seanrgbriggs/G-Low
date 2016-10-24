using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class freshbeatsyomang : MonoBehaviour {

    public bool likesSelection;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        string scene_name = SceneManager.GetActiveScene().name;

        bool toDestroy = scene_name != "CharSel" && scene_name != "MapSel";
        if ((likesSelection && toDestroy) || (!likesSelection && !toDestroy))
        {
            Destroy(this.gameObject);
        }
	}
}
