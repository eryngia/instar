
// Shop button for buying max health.
public class ShopHealthMaxInteractable : ShopInteractable {
    
    protected override void Buy() {
        if (CanBuy()) {
            GameManager.Instance.SpendScore(GetPrice());
            GameManager.Instance.AddMaxHealth(1);
            GameManager.Instance.GainHealth(1);
        }
    }

    public override bool CanBuy() {
        return GameManager.Instance.GetScore() >= GetPrice();
    }

}