using UnityEngine;

public interface Receiver{

	void Receive(int id, Object toRecieve, string label = "");

}
