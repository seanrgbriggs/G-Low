using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selectable : MonoBehaviour {

	public Object contents;
	public string label = "";

	public Selectable left { set; get; } 
	public Selectable right{ set; get; }

	public Selectable up {set; get;}
	public Selectable down { set; get; }

	List<int> cursors;

	void Start(){
		cursors = new List<int> ();
	}

	public bool isSelected(){
		return cursors.Count > 0;
	}

	public bool isSelectedBy(int i){
		return cursors.IndexOf (i) >= 0;
	}

	public bool PassLeft(int i){
		if (!isSelectedBy (i)) {
			return false;
		}
		cursors.Remove (i);
		left.cursors.Add (i);
		return true;
	}

	public bool PassRight(int i){
		if (!isSelectedBy (i)) {
			return false;
		}
		cursors.Remove (i);
		right.cursors.Add (i);
		return true;
	}

	public bool PassUp(int i){
		if (!isSelectedBy (i)) {
			return false;
		}
		cursors.Remove (i);
		up.cursors.Add (i);
		return true;
	}

	public bool PassDown(int i){
		if (!isSelectedBy (i)) {
			return false;
		}
		cursors.Remove (i);
		down.cursors.Add (i);
		return true;
	}
}

