using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selector : MonoBehaviour {

	public Selectable[] menu;
	public string label = "";
	public string rec_label = "";


	public int gridAssign; //Assign nodes in a grid pattern, assuming the provided number of rows.
	public bool doPhysicalAssignment; //Assign nearby nodes by world location

	public bool disallowHorizontal;
	public bool disallowVertical;

	public const float max_cooldown = 0.1f;

	public List<Cursor> cursors;

	Receiver rec;


	// Use this for initialization
	void Start () {

		SetupMenu ();
		foreach (Selectable s in menu) {
			s.parent = this;
		}

		foreach (Cursor c in cursors) {
			c.id = cursors.IndexOf (c);
			menu [0].registerCursor(c);
		}

		foreach (Receiver r in FindObjectsOfType<Receiver>()) {
			if (r.reciever_label == rec_label) {
				rec = r;
				break;
			}
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
		}else{

			int row_offset = (gridAssign > 1) ? menu.Length/ gridAssign : 0; 

			for (int i = 0; i < menu.Length; i++) {
				menu [i].left = menu [(i + menu.Length - 1) % menu.Length];
				menu [i].right = menu [(i + menu.Length + 1) % menu.Length];

				if (gridAssign > 1) {
					menu [i].up = menu [(i + menu.Length - row_offset) % menu.Length];
					menu [i].down = menu [(i + menu.Length + row_offset) % menu.Length];
				} else {
					menu [i].up = menu [i].left; 
					menu [i].down = menu [i].right;
				}
		
			}
		}

		if (disallowHorizontal) {
			for (int i = 0; i < menu.Length; i++) {
				menu [i].left = menu [i];
				menu [i].right = menu [i];
			}
		}
		if (disallowVertical) {
			for (int i = 0; i < menu.Length; i++) {
				menu [i].up = menu [i];
				menu [i].down = menu [i];
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
		
		for (int i = 0; i < cursors.Count; i++) {
			if (cursors [i].cooldown <= 0) {

				bool button_pushed = true;
				if (Input.GetButtonDown ("Ultimate" + i)) {
					rec.Receive (i, cursors [i].GetPointer().contents, label);
				} else if (Input.GetButtonDown ("Ability" + i)) {
					rec.Receive (i, null, label);
				} else if (Input.GetAxis ("Horizontal" + i) > 0) {
					cursors [i].GetPointer().PassRight (i);
				} else if (Input.GetAxis ("Horizontal" + i) < 0) {
					cursors [i].GetPointer().PassLeft (i);
				} else if (Input.GetAxis ("Vertical" + i) > 0) {
					cursors [i].GetPointer().PassUp (i);
				} else if (Input.GetAxis ("Vertical" + i) < 0) {
					cursors [i].GetPointer().PassDown (i);
				} else {
					button_pushed = false;
				}

				if (button_pushed) {
					cursors [i].cooldown = max_cooldown;
				}
			}

		}		
	}
}
