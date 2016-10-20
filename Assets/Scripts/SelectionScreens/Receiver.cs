using UnityEngine;

public abstract class Receiver : MonoBehaviour{

	public string reciever_label;

	public abstract void Receive(int id, Object toRecieve, string label = "");

}
