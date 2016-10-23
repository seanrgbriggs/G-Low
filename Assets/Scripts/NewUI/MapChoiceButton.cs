using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapChoiceButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler {
    public GameObject prefab;
    public string mapName;

    void Start() {
    }

    void UpdatePrefab() {
        GetComponentInParent<MapSelect>().UpdateViewObject(prefab);

        MapSelect.map = mapName;
    }

    public void Done() {
        SceneManager.LoadScene("CharSel", LoadSceneMode.Single);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        UpdatePrefab();
    }

    public void OnSelect(BaseEventData eventData) {
        UpdatePrefab();
    }
}