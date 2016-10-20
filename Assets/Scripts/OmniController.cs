using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OmniController : Receiver {

    public static int max_players = 4;
	public static string character_select = "CharSel";
	public static string map_select = "VincentScene";
	public static string default_level = "Menu";

	public static string level { get; set; }
	public static GameObject[] characters { get; set; }

	static bool globalStart = false;

	void GlobalStart(){
		if (!globalStart) {
			characters = new GameObject[max_players];

		}
	}
	// Use this for initialization
	void Start () {
        level = default_level;
		GlobalStart ();
     }
	

    public void StartGame() {
        int i = 0;
 		foreach(GameObject spawnpoint in GameObject.FindGameObjectsWithTag("Spawnpoint"))
        {
            if (characters[i] == null)
            {
                 continue;
            }
            GameObject player = (GameObject) Instantiate(characters[i], spawnpoint.transform.position, spawnpoint.transform.rotation);
			player.GetComponent<PlayerCar> ().id = i++;
        }
     }

    public void EndGame() {
        SceneManager.LoadScene(character_select);
        level = default_level;
		System.Array.Clear (characters, 0, max_players);
    }
		

	public override void Receive(int id, Object obj, string label){
	//	print (label + ": " + obj.name);

		if (label == "character") {

			if (obj == null) {
				characters [id] = null;
				return;
			} 

			characters [id] = ((GameObject)obj);

			GameObject[] models = GameObject.FindGameObjectsWithTag ("Player");
			System.Array.Sort (models, ((x, y) => x.name.CompareTo (y.name)));
	
			models [id].GetComponent<MeshFilter> ().mesh = characters [id].GetComponent<MeshFilter> ().sharedMesh;
			models [id].GetComponent<MeshRenderer> ().material = characters [id].GetComponent<MeshRenderer> ().sharedMaterial;

			if (numLocked () >= numActiveCursors ()) {
				//SceneManager.LoadScene (map_select);
				//TODO: remove these two lines

				level = "VincentScene";
				SceneManager.LoadScene(level);

			}
			
		} else if (label == "map") {

			level = obj.name;
			SceneManager.LoadScene(level);


		} else if (label == "menu") {
			if (obj.name.Contains ("lay")) {
				SceneManager.LoadScene (character_select);
			}
		}
	}

	public int numLocked(){
		return max_players - System.Array.FindAll (characters, (x) => x == null).Length;
	}

	public int numActiveCursors(){
		return System.Array.FindAll (FindObjectsOfType<Cursor> (), (x) => x.isActive ()).Length;
	}

}
