using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChoiceButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler {
    public MeshRenderer targetMeshRenderer;
    public MeshFilter targetMeshFilter;

    private MeshRenderer myMeshRenderer;
    private MeshFilter myMeshFilter;

    public GameObject prefab;

    void Start() {
        myMeshFilter = GetComponentInChildren<MeshFilter>();
        myMeshRenderer = GetComponentInChildren<MeshRenderer>();

        myMeshFilter.mesh = prefab.GetComponent<MeshFilter>().sharedMesh;
        myMeshRenderer.material = prefab.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void UpdateMesh() {
        targetMeshRenderer.material = myMeshRenderer.material;
        targetMeshFilter.mesh = myMeshFilter.mesh;

        GetComponentInParent<CharacterSelectPanel>().SetChoice(prefab);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        UpdateMesh();
    }

    public void OnSelect(BaseEventData eventData) {
        UpdateMesh();
    }
}