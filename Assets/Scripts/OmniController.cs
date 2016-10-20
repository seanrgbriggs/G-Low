using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OmniController : Receiver {

    public int max_players = 4;
    public string character_select;
	public string map_select;
    public string default_level;

    public string level { get; set; }
    public GameObject[] characters { get; set; }

	// Use this for initialization
	void Start () {
        level = default_level;
        characters = new GameObject[max_players];
        DontDestroyOnLoad(gameObject);
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
			player.GetComponent<PlayerCar> ().id = i - 1;
        }
    }

    public void EndGame() {
        SceneManager.LoadScene(character_select);
        level = default_level;
		System.Array.Clear (characters, 0, max_players);
    }
		

	public override void Receive(int id, Object obj, string label){
		print (label + ": " + obj.name);

		if (label == "character") {

			if (obj == null) {
				characters [id] = null;
				return;
			} 

			characters [id] = ((GameObject) obj);

			GameObject[] models = GameObject.FindGameObjectsWithTag ("Player");
			System.Array.Sort (models, ((x, y) => x.name.CompareTo (y.name)));
	
			models [id].GetComponent<MeshFilter> ().mesh = characters[id].GetComponent<MeshFilter> ().sharedMesh;
			models [id].GetComponent<MeshRenderer> ().material = characters [id].GetComponent<MeshRenderer> ().sharedMaterial;

			if (numLocked () >= numActiveCursors ()) {
				SceneManager.LoadScene (map_select);
			}
			
		} else if (label == "map") {

			level = obj.name;
			StartGame ();

		}
	}

	public int numLocked(){
		return max_players - System.Array.FindAll (characters, (x) => x == null).Length;
	}

	public int numActiveCursors(){
		return System.Array.FindAll (FindObjectsOfType<Cursor> (), (x) => x.isActive ()).Length;
	}

}
