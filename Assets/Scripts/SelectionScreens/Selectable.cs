using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selectable : MonoBehaviour {

	public Object contents;

	public Selectable left { set; get; } 
	public Selectable right{ set; get; }

	public Selectable up {set; get;}
	public Selectable down { set; get; }

	public Selector parent { set; get; }
	List<int> cursors;

	void Awake(){
		cursors = new List<int> ();
	}

	public void registerCursor(Cursor c){
		c.SetPointer (this);
		cursors.Add (c.id);
	}

	public bool isSelected(){
		return cursors.Count > 0;
	}

	public bool isSelectedBy(int i){
		return cursors.IndexOf (i) >= 0;
	}

	public bool Pass(int i, Selectable sel){
		if (!isSelectedBy (i)) {
			return false;
		}
		cursors.Remove (i);
		sel.cursors.Add (i);
		parent.cursors [i].SetPointer (sel);
		return true;
	}

	public bool PassLeft(int i){
		return Pass (i, left);
	}

	public bool PassRight(int i){
		return Pass (i, right);
	}

	public bool PassUp(int i){
		return Pass (i, up);
	}

	public bool PassDown(int i){
		return Pass (i, down);
	}
}

