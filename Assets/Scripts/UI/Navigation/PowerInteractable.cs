using UnityEngine;
using UnityEngine.UI;

// Button for choosing a power.
public class PowerInteractable : Interactable {
    
    private PowerSO power;

    private void Start() {
        power = GameManager.Instance.GetPowerChoice();
        GetComponent<Image>().sprite = power.sprite;
        GetComponent<Highlight>().highlight.GetComponent<Image>().sprite = power.sprite;
    }

    // Set new power in game manager.
    public override bool Interact() {
        GameManager.Instance.ChoosePower(power);
        return true;
    }

    // Returns power SO of this button.
    public PowerSO GetPower() {
        return power;
    }

}