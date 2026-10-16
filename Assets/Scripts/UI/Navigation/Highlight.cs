using UnityEngine;
using UnityEngine.UI;

// Add colored shadow copy under UI element.
public class Highlight : MonoBehaviour {
    [SerializeField] private Vector2 offset = new Vector2(0.1f, -0.1f);
    [SerializeField] private Color color = Color.red;
    public GameObject highlight;
    public bool original = true;

    private void Start() {
        // prevent copy from creating copies
        if (!original) {
            Destroy(this);
            return;
        }

        // create highlight object with highlight color and offset position
        highlight = Instantiate(gameObject, transform.parent);
        highlight.name = gameObject.name + "Highlight"; 
        highlight.GetComponent<Highlight>().original = false;
        highlight.transform.SetSiblingIndex(transform.GetSiblingIndex()); 
        highlight.transform.position = (Vector2) transform.position + offset;
        highlight.GetComponent<Graphic>().color = color;
        highlight.transform.SetAsFirstSibling();

        Destroy(highlight.GetComponent<Interactable>());
    }

    // Hide the highlight.
    public void Hide() {
        highlight.GetComponent<Graphic>().color = Color.clear;
    }

    // Show the highlight.
    public void Show() {
        highlight.GetComponent<Graphic>().color = color;
    }

}