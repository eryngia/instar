using System;
using UnityEngine;
using TMPro;

// Update text to a value whenever an event is triggered.
public abstract class TextDisplay : MonoBehaviour {

    private TextMeshProUGUI textUI;
    protected int defaultValue;
    [SerializeField] protected string displayLabel;

    protected abstract int GetValue();

    private void Start() {
        textUI = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    protected void OnVariableChangedHandler(object sender, EventArgs e) {
        UpdateText();
    }

    // Update text to current value.
    private void UpdateText() {
        textUI.text = displayLabel + GetValue();
    }
    
}
