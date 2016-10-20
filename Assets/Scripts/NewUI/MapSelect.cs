using UnityEngine;
using System.Collections;

public class MapSelect : MonoBehaviour {
    public GameObject curViewObj;

    public static string map = "";

	// Use this for initialization
	void Start () {
        map = "";
	}

    public void UpdateViewObject(GameObject prefab) {
        GameObject newObj = Instantiate(prefab);
        newObj.transform.parent = curViewObj.transform.parent;
        newObj.transform.localPosition = curViewObj.transform.localPosition;
        newObj.transform.localRotation = curViewObj.transform.localRotation;
        newObj.transform.localScale = curViewObj.transform.localScale;

        Destroy(curViewObj);
        curViewObj = newObj;
    }
}
