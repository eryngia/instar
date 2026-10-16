using TMPro;
using UnityEngine;

// Text display for price.
public class PriceTextDisplay : MonoBehaviour {

    [SerializeField] ShopInteractable item;
    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color unavailableColor = Color.red;
    private TextMeshProUGUI textUI;


    private void Start() {
        textUI = GetComponent<TextMeshProUGUI>();
        textUI.text = item.GetPrice().ToString();
    }

    // Color text based on availability.
    private void Update() {
        textUI.color = item.CanBuy() ? availableColor : unavailableColor;
    }


}