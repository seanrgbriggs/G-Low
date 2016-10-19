using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OmniController : MonoBehaviour {

    public int max_players = 4;
    public string character_select;

    string level;
    GameObject[] characters;

	// Use this for initialization
	void Start () {
        level = "Menu";
        characters = new GameObject[max_players];
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void StartGame() {
        SceneManager.LoadScene(level);
        int i = 0;
        foreach(GameObject spawnpoint in GameObject.FindGameObjectsWithTag("Spawnpoint"))
        {
            if (characters[i] == null)
            {
                continue;
            }
            GameObject player = (GameObject) Instantiate(characters[i++], spawnpoint.transform.position, spawnpoint.transform.rotation);
            
        }
    }

    public void EndGame() {
        SceneManager.LoadScene(character_select);
    }

}
