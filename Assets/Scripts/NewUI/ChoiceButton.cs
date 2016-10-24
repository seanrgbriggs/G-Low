using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChoiceButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler {
    public MeshRenderer targetMeshRenderer;
    public MeshFilter targetMeshFilter;

    private MeshRenderer myMeshRenderer;
    private MeshFilter myMeshFilter;

    public GameObject prefab;

    public string ability;
    public string ultimate;

    void Start() {
        myMeshFilter = GetComponentInChildren<MeshFilter>();
        myMeshRenderer = GetComponentInChildren<MeshRenderer>();

        myMeshFilter.mesh = prefab.GetComponent<MeshFilter>().sharedMesh;
        myMeshRenderer.material = Instantiate(prefab.GetComponent<MeshRenderer>().sharedMaterial);

        CharacterSelectPanel panel = GetComponentInParent<CharacterSelectPanel>();
        
        Color base_col = panel.colors[panel.id] * 2;
        myMeshRenderer.material.SetColor("_EmissionColor", base_col);
    }

    public void UpdateMesh() {
        targetMeshRenderer.material = myMeshRenderer.material;
        targetMeshFilter.mesh = myMeshFilter.mesh;

        GetComponentInParent<CharacterSelectPanel>().SetChoice(prefab, ability, ultimate);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        UpdateMesh();
    }

    public void OnSelect(BaseEventData eventData) {
        UpdateMesh();
    }
}