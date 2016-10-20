using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

	public int id { set; get; }
	public float cooldown { set; get; }
	Selectable pointer;

	public Vector3 cursor_offset = Vector3.zero;

	public virtual void Start(){
		cooldown = -1;
	}

	public virtual void Update(){
		if (!isReady()) {
			cooldown -= Time.deltaTime;
		}
	}

	public void SetPointer (Selectable point) {
		pointer = point;
		transform.position = point.transform.position + cursor_offset;
	}

	public Selectable GetPointer(){
		return pointer;
	}
	
	public bool isActive(){
		return cooldown > -1;
	}

	public bool isReady(){
		return isActive () && cooldown <= 0;
	}


	public void deactivate(){
		cooldown = -1;
	}
}
