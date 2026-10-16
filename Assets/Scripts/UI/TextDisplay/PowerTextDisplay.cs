using TMPro;
using UnityEngine;

// Text display for power selection.
public class PowerTextDisplay : MonoBehaviour {

    [SerializeField] Navigation navigation;

    private TextMeshProUGUI textUI;


    private void Start() {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    // Set text to description of current power selected in navigation.
    private void Update() {
        if (navigation.GetSelection() == null) return;
        PowerSO power = navigation.GetSelection().GetComponent<PowerInteractable>().GetPower();
        if (power == null) return;
        textUI.text = navigation.GetSelection().GetComponent<PowerInteractable>().GetPower().description;
    }


}