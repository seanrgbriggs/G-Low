using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSelectPanel : MonoBehaviour {
    public int id;
    public CanvasGroup group;
    public CanvasGroup initial;

    private MeshRenderer[] meshes;
    private Button[] choiceButtons;

    public bool started { get; private set; }
    public bool ready { get; private set; }

    public static GameObject[] selections = new GameObject[4];

    public Text statusText;

    void Start() {
        started = false;
        ready = false;

        meshes = GetComponentsInChildren<MeshRenderer>();
        choiceButtons = GetComponentInChildren<GridLayoutGroup>().GetComponentsInChildren<Button>();

        SetMode(false);

        selections[id] = null;

        StandaloneInputModule input = GetComponent<StandaloneInputModule>();
        input.horizontalAxis = "Horizontal" + id;
        input.verticalAxis = "Vertical" + id;
        input.submitButton = "Ultimate" + id;
        input.cancelButton = "Ability" + id;
    }
    
	// Update is called once per frame
	void Update() {
	    if (Input.GetButtonDown("Ultimate" + id)) {
            if (!started) {
                started = true;
                SetMode(true);
                choiceButtons[0].Select();
            } else {
                ready = true;
                statusText.text = "WAITING FOR PLAYERS";
            }
        }

        if (Input.GetButtonDown("Ability" + id)) {
            ready = false;
            statusText.text = "PRESS RIGHT BUMPER";
        }
	}

    public void SetChoice(GameObject choice) {
        if (started) {
            selections[id] = choice;
        }
    }

    public void SetMode(bool active) {
        initial.interactable = !active;
        initial.blocksRaycasts = !active;
        initial.alpha = active ? 0.0f : 1.0f;

        group.interactable = active;
        group.blocksRaycasts = active;
        group.alpha = active ? 1.0f : 0.0f;
        
        foreach (MeshRenderer mesh in meshes) {
            mesh.enabled = active;
        }
    }
}
