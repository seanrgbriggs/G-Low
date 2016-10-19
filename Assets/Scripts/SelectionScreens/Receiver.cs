using UnityEngine;

public abstract class Receiver : MonoBehaviour{

	public abstract void Receive(int id, Object toRecieve, string label = "");

}
