
// Shop button for buying healing.
public class ShopHealthInteractable : ShopInteractable {
    
    protected override void Buy() {
        if (CanBuy()) {
            GameManager.Instance.SpendScore(GetPrice());
            GameManager.Instance.GainHealth(1);
        }
    }

    public override bool CanBuy() {
        return GameManager.Instance.GetHealth() < GameManager.Instance.GetMaxHealth() 
            && GameManager.Instance.GetScore() >= GetPrice();
    }

}