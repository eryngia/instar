
// Shop button for buying max ammo.
public class ShopAmmoMaxInteractable : ShopInteractable {
    
    protected override void Buy() {
        if (CanBuy()) {
            GameManager.Instance.SpendScore(GetPrice());
            GameManager.Instance.AddMaxAmmo(1);
            GameManager.Instance.ReloadAmmo();
        }
    }

    public override bool CanBuy() {
        return GameManager.Instance.GetScore() >= GetPrice();
    }

}