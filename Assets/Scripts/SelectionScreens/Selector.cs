using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selector : MonoBehaviour {

	public Selectable[] menu;
	public Receiver rec;

	public bool doPhysicalAssignment; //Assign nearby nodes by world location
	public bool vertical; //Otherwise, indicate if the menu is horizontal or vertical

	public const float max_cooldown = 0.1f;
	public int num_players;

	List<float> cursor_cooldowns;

	// Use this for initialization
	void Start () {

		SetupMenu ();

		cursor_cooldowns = new List<float> (num_players);
		for (int i = 0; i < cursor_cooldowns.Count; i++) {
			cursor_cooldowns [i] = -1;
		}
	
	}

	void SetupMenu(){
		if (doPhysicalAssignment) {
			for (int i = 0; i < menu.Length; i++) {
				menu[i].left = trace(menu[i], Vector2.left);
				menu[i].right = trace(menu[i], Vector2.right);
				menu[i].up = trace(menu[i], Vector2.up);
				menu[i].down = trace(menu[i], Vector2.down);
			}
			return;
		}else{
			if(vertical){
				for (int i = 0; i < menu.Length; i++) {
					menu [i].up = menu [(i - 1) % menu.Length];
					menu [i].down = menu [(i + 1) % menu.Length];
				}
			}else{
				for (int i = 0; i < menu.Length; i++) {
					menu [i].left = menu [(i - 1) % menu.Length];
					menu [i].right = menu [(i + 1) % menu.Length];
				}
			}
		}
	}

	Selectable trace(Selectable item, Vector2 direction){
		Selectable toReturn = item;

		RaycastHit2D[] hits = Physics2D.RaycastAll (item.transform.position, direction);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider.gameObject != item.gameObject) {
				toReturn = hit.collider.GetComponent<Selectable>();
				if (toReturn != null && System.Array.IndexOf(menu, toReturn) > -1) {  
					return toReturn;
				}
			}
		}

		hits = Physics2D.RaycastAll (item.transform.position, -direction);
		System.Array.Reverse (hits);

		foreach (RaycastHit2D hit in hits) {
			if (hit.collider.gameObject != item.gameObject) {
				toReturn = hit.collider.GetComponent<Selectable>();
				if (toReturn != null && System.Array.IndexOf(menu, toReturn) > -1) {
					return toReturn;
				}
			}
		}

		return item;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput ();
	}

	void HandleInput(){
		
		for (int i = 0; i < cursor_cooldowns.Count; i++) {

			if (cursor_cooldowns [i] > 0) {
				cursor_cooldowns [i] -= Time.deltaTime;
			} else {
				if (Input.GetButtonDown ("" + i)) {
					
				}else if (Input.GetAxis ("Horizontal" + i) > 0) {
					for (int j = 0; j < menu.Length; j++) {
						if (menu [j].PassRight (i)) {
							break;
						}
					}
					break;
				} else if (Input.GetAxis ("Horizontal" + i) < 0) {
					for (int j = 0; j < menu.Length; j++) {
						if (menu [j].PassLeft (i)) {
							break;
						}
					}
					break;
				} else if (Input.GetAxis ("Vertical" + i) > 0) {
					for (int j = 0; j < menu.Length; j++) {
						if (menu [j].PassUp (i)) {
							break;
						}
					}
					break;
				}else if(Input.GetAxis("Vertical"+i) < 0){
					for (int j = 0; j < menu.Length; j++) {
						if (menu [j].PassDown (i)) {
							break;
						}
					}
				}
			}

		}		
	}
}
