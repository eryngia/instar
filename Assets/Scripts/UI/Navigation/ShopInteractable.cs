
using UnityEngine;

// Button for buying something from shop.
public abstract class ShopInteractable : Interactable {
    
    [SerializeField] private int price;

    public int GetPrice() {
        return price;
    }

    public abstract bool CanBuy();

    protected abstract void Buy();

    // Buy if sufficient currency and conditions met.
    public override bool Interact() {
        if (CanBuy()) {
            Buy();
            return true;
        } else {
            return false;
        }
    }

}