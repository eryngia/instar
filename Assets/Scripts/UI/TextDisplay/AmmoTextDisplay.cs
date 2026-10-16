
// Text display for ammo.
public class AmmoTextDisplay : TextDisplay {

    protected override int GetValue() {
        return GameManager.Instance.GetAmmo();
    }

    private void Awake() {
        GameManager.Instance.OnAmmoChanged += OnVariableChangedHandler;
    }

    private void OnDisable() {
        GameManager.Instance.OnAmmoChanged -= OnVariableChangedHandler;
    }

}
